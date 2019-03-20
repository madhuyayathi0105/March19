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


public partial class LibraryMod_Library_Card_Master : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
    DataSet ds = new DataSet();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collegecode = string.Empty;
    bool check = false;
    DataTable dtAddFine = new DataTable();
    DataTable dtRenewDays = new DataTable();
    DataRow drCurrentRow;

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
                ddl_CardCatogery.Attributes.Add("onfocus", "frelig()");
                Bindcollege();
                bindddlCatogery();
                getLibPrivil();
                bindBookType();
                bindBookBank();
                BindBatchYear();
                BindStafftype();
                rbStudent.Checked = true;
                bindStaffcategory();
                BindDepartment();
                rbStudent_OnCheckedChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    #region bindmethod

    public void Bindcollege()
    {
        try
        {
            //ddl_library.Items.Clear();
            dtCommon.Clear();
            ddl_collegename.Enabled = false;
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
                ddl_collegename.DataSource = dtCommon;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                ddl_collegename.SelectedIndex = 0;
                ddl_collegename.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
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
            //ListItem lidefault = new ListItem();
            ddl_CardCatogery.Items.Insert(0, "All");

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
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void bindBookType()
    {
        ddlBookType.Items.Clear();
        ddlBookType.Items.Add("All");
        ddlBookType.Items.Add("Book");
        ddlBookType.Items.Add("Periodicals");
        ddlBookType.Items.Add("Project Book");
        ddlBookType.Items.Add("Non-Book Material");
        ddlBookType.Items.Add("Question Bank");
        ddlBookType.Items.Add("Back Volume");
        ddlBookType.Items.Add("Reference Volume");
    }

    protected void bindBookBank()
    {
        ddl_BookBank.Items.Clear();
        ddl_BookBank.Items.Add("All");
        ddl_BookBank.Items.Add("Book Bank");
    }

    protected void BindBatchYear()
    {
        string qry = " select distinct Batch_Year from Registration order by batch_year desc";
        DataTable dtbatchyr = dirAcc.selectDataTable(qry);
        cbl_BatchYear.Items.Clear();
        if (dtbatchyr.Rows.Count > 0)
        {
            cbl_BatchYear.DataSource = dtbatchyr;
            cbl_BatchYear.DataTextField = "Batch_Year";
            cbl_BatchYear.DataValueField = "Batch_Year";
            cbl_BatchYear.DataBind();

            cbl_BatchYearFine.DataSource = dtbatchyr;
            cbl_BatchYearFine.DataTextField = "Batch_Year";
            cbl_BatchYearFine.DataValueField = "Batch_Year";
            cbl_BatchYearFine.DataBind();

            cbl_BatchYearNEW.DataSource = dtbatchyr;
            cbl_BatchYearNEW.DataTextField = "Batch_Year";
            cbl_BatchYearNEW.DataValueField = "Batch_Year";
            cbl_BatchYearNEW.DataBind();
        }
    }

    protected void BindStafftype()
    {
        string CollegeCode = ddl_collegename.SelectedValue.ToString();
        ds.Clear();
        string qry = " SELECT DISTINCT StfType FROM StaffTrans T,StaffMaster M WHERE T.Staff_Code = M.Staff_Code AND M.College_Code ='" + CollegeCode + "' AND T.Latestrec = 1 ORDER BY StfType ";
        ds = d2.select_method_wo_parameter(qry, "Text");

        cbl_StaffType.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_StaffType.DataSource = ds;
            cbl_StaffType.DataTextField = "stftype";
            cbl_StaffType.DataValueField = "stftype";
            cbl_StaffType.DataBind();
        }
    }

    protected void bindStaffcategory()
    {
        try
        {
            cbl_StaffCatogery.Items.Clear();
            string CollegeCode = ddl_collegename.SelectedValue.ToString();
            ds.Clear();
            string Query = " select category_code,category_name,CategoryID,college_code from staffcategorizer where college_code='" + CollegeCode + "' order by category_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_StaffCatogery.DataSource = ds;
                cbl_StaffCatogery.DataTextField = "category_name";
                cbl_StaffCatogery.DataValueField = "category_code";
                cbl_StaffCatogery.DataBind();

                cbl_StaffCatogeryFine.DataSource = ds;
                cbl_StaffCatogeryFine.DataTextField = "category_name";
                cbl_StaffCatogeryFine.DataValueField = "category_code";
                cbl_StaffCatogeryFine.DataBind();

                cbl_StaffCatogeryNEW.DataSource = ds;
                cbl_StaffCatogeryNEW.DataTextField = "category_name";
                cbl_StaffCatogeryNEW.DataValueField = "category_code";
                cbl_StaffCatogeryNEW.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void BindDepartment()
    {
        try
        {
            cbl_Department.Items.Clear();
            cbl_DepartmentFine.Items.Clear();
            cbl_DepartmentNEW.Items.Clear();
            string CollegeCode = ddl_collegename.SelectedValue.ToString();
            ds.Clear();
            if (rbStudent.Checked)
            {
                string Query = " SELECT Course_Name+'-'+Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + CollegeCode + "' ORDER BY Course_Name,Dept_Name";
                //string Query = " select d.degree_code,c.course_name+'-'+dt.dept_name as degreeName,c.course_id,d.college_code from degree d,course c,department dt,deptprivilages dtp where d.dept_code=dt.dept_code and d.course_id=c.course_id and d.degree_code=dtp.degree_code and user_code='" + userCode + "' and d.college_code=dt.college_code and d.college_code=c.college_code and dt.college_code=c.college_code and d.college_code='" + CollegeCode + "' order by c.course_name";
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Department.DataSource = ds;
                    cbl_Department.DataTextField = "Degree";
                    cbl_Department.DataValueField = "Degree_Code";
                    cbl_Department.DataBind();

                    cbl_DepartmentFine.DataSource = ds;
                    cbl_DepartmentFine.DataTextField = "Degree";
                    cbl_DepartmentFine.DataValueField = "Degree_Code";
                    cbl_DepartmentFine.DataBind();

                    cbl_DepartmentNEW.DataSource = ds;
                    cbl_DepartmentNEW.DataTextField = "Degree";
                    cbl_DepartmentNEW.DataValueField = "Degree_Code";
                    cbl_DepartmentNEW.DataBind();
                }
            }
            if (rbStaff.Checked)
            {
                string Query = " select distinct dept_code,dept_name from hrdept_master where 1=1  AND college_code = '" + CollegeCode + "' order by dept_name";
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Department.DataSource = ds;
                    cbl_Department.DataTextField = "dept_name";
                    cbl_Department.DataValueField = "dept_code";
                    cbl_Department.DataBind();

                    cbl_DepartmentFine.DataSource = ds;
                    cbl_DepartmentFine.DataTextField = "dept_name";
                    cbl_DepartmentFine.DataValueField = "dept_code";
                    cbl_DepartmentFine.DataBind();

                    cbl_DepartmentNEW.DataSource = ds;
                    cbl_DepartmentNEW.DataTextField = "dept_name";
                    cbl_DepartmentNEW.DataValueField = "dept_code";
                    cbl_DepartmentNEW.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
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
    }

    protected void rbStudent_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbStudent.Checked)
        {
            txt_BatchYear.Enabled = true;
            txt_Department.Enabled = true;
            txt_StaffCatogery.Enabled = false;
            txt_StaffType.Enabled = false;

            txt_BatchYearNEW.Enabled = true;
            txt_DepartmentNEW.Enabled = true;
            txt_StaffCatogeryNEW.Enabled = false;

            txt_BatchYearFine.Enabled = true;
            txt_DepartmentFine.Enabled = true;
            txt_StaffCatogeryFine.Enabled = false;
            BindDepartment();
        }
        divspread.Visible = false;
    }

    protected void rbStaff_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbStaff.Checked)
        {
            txt_BatchYear.Enabled = false;
            txt_Department.Enabled = true;
            txt_StaffCatogery.Enabled = true;
            txt_StaffType.Enabled = true;

            txt_BatchYearNEW.Enabled = false;
            txt_DepartmentNEW.Enabled = true;
            txt_StaffCatogeryNEW.Enabled = true;

            txt_BatchYearFine.Enabled = false;
            txt_DepartmentFine.Enabled = false;
            txt_StaffCatogeryFine.Enabled = true;
            BindDepartment();
        }
        divspread.Visible = false;
    }

    #region Card Category Popup

    protected void btnAddCardCatogery_OnClick(object sender, EventArgs e)
    {
        txt_CardCatogery.Text = string.Empty;
        PNewCardCatogery.Visible = true;
        DivCard.Visible = true;
    }

    protected void btnDeleteCardCatogery_OnClick(object sender, EventArgs e)
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
                PNewCardCatogery.Visible = false;
                DivCard.Visible = false;
                txt_CardCatogery.Text = string.Empty;
            }
        }
    }

    protected void btn_NewCardcatogeryExit_Click(object sender, EventArgs e)
    {
        PNewCardCatogery.Visible = false;
        DivCard.Visible = false;
        txt_CardCatogery.Text = string.Empty;
    }

    #endregion

    #region CheckboxEvent

    protected void cb_BookBank_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_BookBank.Checked)
            ddl_BookBank.Enabled = true;
        else
            ddl_BookBank.Enabled = false;
    }

    protected void cb_BatchYear_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_BatchYear, cbl_BatchYear, txt_BatchYear, lblBatchYr.Text, "--Select--");
    }

    protected void cbl_BatchYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_BatchYear, cbl_BatchYear, txt_BatchYear, lblBatchYr.Text, "--Select--");
    }

    protected void cb_Department_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_Department, cbl_Department, txt_Department, lblDepartment.Text, "--Select--");
    }

    protected void cbl_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_Department, cbl_Department, txt_Department, lblDepartment.Text, "--Select--");
    }

    protected void cb_StaffCatogery_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_StaffCatogery, cbl_StaffCatogery, txt_StaffCatogery, "Category", "--Select--");       
    }

    protected void cbl_StaffCatogery_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_StaffCatogery, cbl_StaffCatogery, txt_StaffCatogery, "Category", "--Select--");       
    }

    protected void cb_StaffType_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_StaffType, cbl_StaffType, txt_StaffType, lblStaffType.Text, "--Select--"); 
    }

    protected void cbl_StaffType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_StaffType, cbl_StaffType, txt_StaffType, lblStaffType.Text, "--Select--");   
    }

    protected void cb_BatchYearFine_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_BatchYearFine, cbl_BatchYearFine, txt_BatchYearFine, lblBatchYrFine.Text, "--Select--");         
    }

    protected void cbl_BatchYearFine_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_BatchYearFine, cbl_BatchYearFine, txt_BatchYearFine, lblBatchYrFine.Text, "--Select--"); 
    }

    protected void cb_DepartmentFine_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_DepartmentFine, cbl_DepartmentFine, txt_DepartmentFine, lblDepartmentFine.Text, "--Select--");         
    }

    protected void cbl_DepartmentFine_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_DepartmentFine, cbl_DepartmentFine, txt_DepartmentFine, lblDepartmentFine.Text, "--Select--");
    }

    protected void cb_StaffCatogeryFine_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_StaffCatogeryFine, cbl_StaffCatogeryFine, txt_StaffCatogeryFine, lblStaffCategoryFine.Text, "--Select--");       
    }

    protected void cbl_StaffCatogeryFine_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_StaffCatogeryFine, cbl_StaffCatogeryFine, txt_StaffCatogeryFine, lblStaffCategoryFine.Text, "--Select--"); 
    }

    protected void cb_BatchYearNEW_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_BatchYearNEW, cbl_BatchYearNEW, txt_BatchYearNEW, lblBatchYrNEW.Text, "--Select--");         
    }

    protected void cbl_BatchYearNEW_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_BatchYearNEW, cbl_BatchYearNEW, txt_BatchYearNEW, lblBatchYrNEW.Text, "--Select--");   
    }

    protected void cb_DepartmentNEW_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_DepartmentNEW, cbl_DepartmentNEW, txt_DepartmentNEW, lblDepartmentNEW.Text, "--Select--");        
    }

    protected void cbl_DepartmentNEW_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_DepartmentNEW, cbl_DepartmentNEW, txt_DepartmentNEW, lblDepartmentNEW.Text, "--Select--");     
    }

    protected void cb_StaffCatogeryNEW_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_StaffCatogeryNEW, cbl_StaffCatogeryNEW, txt_StaffCatogeryNEW, lblStaffCategoryNEW.Text, "--Select--");        
    }

    protected void cbl_StaffCatogeryNEW_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_StaffCatogeryNEW, cbl_StaffCatogeryNEW, txt_StaffCatogeryNEW, lblStaffCategoryNEW.Text, "--Select--");   
    }

    #endregion

    #region Common Go Event

    protected void btn_MainGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataSet dsload = new DataSet();
            string selectQry = string.Empty;
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string booktype = Convert.ToString(ddlBookType.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedItem.Text);
            string StrBookType = string.Empty;

            if (rbStudent.Checked)
            {
                #region Query
                string batch = getCblSelectedValue(cbl_BatchYear);
                string department = getCblSelectedText(cbl_Department);

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

                selectQry = " SELECT Code,Code_Descp,Batch_Year,No_Of_Token,No_Of_Days,ISNULL(FineType,0) FineType,Fine,ISNULL(Book_Type,'All') Book_Type,ISNULL(Category,'All') Category,ISNULL(StudCategory,'All') StudCategory,ISNULL(CardCat,'All') CardCat,CASE WHEN ISNULL(TransLibCode,'All') = 'All' THEN 'All' ELSE Lib_Name END Lib_Name,0 as 'IsStaff',ISNULL(TransLibCode,'All') TransLibCode  FROM Lib_Master M LEFT JOIN Library L ON M.TransLibCode = L.Lib_Code WHERE Is_Staff = 0 AND Code IN (SELECT CONVERT(nvarchar(5),Course_ID) + '~' + CAST(Dept_Code as nvarchar(5)) FROM Degree WHERE College_Code ='" + college + "')  ";

                if (library != "All")
                {
                    selectQry = selectQry + " AND ISNULL(M.TransLibCode,'All') ='" + library + "' ";
                }
                if (booktype != "All")
                {
                    selectQry = selectQry + " AND ISNULL(Book_Type,'All') = '" + StrBookType + "'";
                }
                if (CardCategory != "All")
                {
                    selectQry = selectQry + " AND ISNULL(CardCat,'All') ='" + CardCategory + "' ";
                }
                if (batch != "")
                {
                    selectQry = selectQry + " AND M.Batch_Year IN ('" + batch + "') ";
                }
                if (department != "")
                {
                    selectQry = selectQry + " AND M.Code_Descp IN('" + department + "')  ";
                }
                selectQry = selectQry + "ORDER BY Batch_Year desc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                #endregion

                int sno = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtLibCardMas = new DataTable();
                    DataRow drow;
                    dtLibCardMas.Columns.Add("SNo", typeof(string));
                    dtLibCardMas.Columns.Add("Batch", typeof(string));
                    dtLibCardMas.Columns.Add("Department", typeof(string));
                    dtLibCardMas.Columns.Add("Code", typeof(string));
                    dtLibCardMas.Columns.Add("No.Of.Token", typeof(string));
                    dtLibCardMas.Columns.Add("No.Of.Days", typeof(string));
                    dtLibCardMas.Columns.Add("Fine", typeof(string));
                    dtLibCardMas.Columns.Add("Book Type", typeof(string));
                    dtLibCardMas.Columns.Add("Category", typeof(string));
                    dtLibCardMas.Columns.Add("Student Category", typeof(string));
                    dtLibCardMas.Columns.Add("Card Category", typeof(string));
                    dtLibCardMas.Columns.Add("Library", typeof(string));
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        string fine = Convert.ToString(ds.Tables[0].Rows[i]["Fine"]);
                        drow = dtLibCardMas.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                        drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Code_Descp"]);
                        drow["Code"] = Convert.ToString(ds.Tables[0].Rows[i]["Code"]);
                        drow["No.Of.Token"] = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_Token"]);
                        drow["No.Of.Days"] = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_Days"]);
                        drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[i]["Fine"]);
                        drow["Book Type"] = Convert.ToString(ds.Tables[0].Rows[i]["Book_Type"]);
                        drow["Category"] = Convert.ToString(ds.Tables[0].Rows[i]["Category"]);
                        drow["Student Category"] = Convert.ToString(ds.Tables[0].Rows[i]["StudCategory"]);
                        drow["Card Category"] = Convert.ToString(ds.Tables[0].Rows[i]["CardCat"]);
                        drow["Library"] = Convert.ToString(ds.Tables[0].Rows[i]["Lib_Name"]);

                        //if (fine == "0")
                        //{
                        //    for (int k = 2; k < SpreadDet.Columns.Count; k++)
                        //    {
                        //        SpreadDet.Sheets[0].Cells[SpreadDet.Sheets[0].RowCount - 1, k].ForeColor = ColorTranslator.FromHtml("green");
                        //    }
                        //}
                        dtLibCardMas.Rows.Add(drow);
                    }
                    chkGridSelectAll.Visible = true;
                    grdLibCardMas.DataSource = dtLibCardMas;
                    grdLibCardMas.DataBind();

                    grdLibCardMas.Visible = true;
                    print.Visible = true;
                    divspread.Visible = true;
                    lblvalidation1.Text = "";
                    txtexcelname.Text = "";
                }
                else
                {
                    divspread.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Found";
                }
            }
            if (rbStaff.Checked)
            {
                #region Query

                string staffCat = getCblSelectedText(cbl_StaffCatogery);
                string staffType = getCblSelectedValue(cbl_StaffType);
                string department = getCblSelectedValue(cbl_Department);

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

                selectQry = "SELECT Code,Staff_Name,Dept_Name,No_Of_Token,No_Of_Days,ISNULL(FineType,0) FineType,Fine,ISNULL(Book_Type,'All') Book_Type,ISNULL(Category,'All') Category,ISNULL(StudCategory,'All') StudCategory,ISNULL(CardCat,'All') CardCat,CASE WHEN ISNULL(TransLibCode,'All') = 'All' THEN 'All' ELSE Lib_Name END Lib_Name,1 as 'IsStaff',ISNULL(TransLibCode,'All') TransLibCode FROM Lib_Master M INNER JOIN StaffMaster S ON M.Code = S.Staff_Code INNER JOIN StaffTrans T ON S.Staff_Code = T.Staff_Code INNER JOIN HrDept_Master D ON D.Dept_Code = T.Dept_Code AND D.College_Code = S.College_Code INNER JOIN StaffCategorizer C ON C.Category_Code = T.Category_Code AND C.College_Code = S.College_Code LEFT JOIN Library L ON M.TransLibCode = L.Lib_Code WHERE M.Code = S.Staff_Code AND Resign = 0 AND Settled = 0  AND T.Latestrec = 1 AND Is_Staff = 1 AND S.College_Code ='" + college + "'";

                if (library != "All")
                {
                    selectQry = selectQry + " AND ISNULL(M.TransLibCode,'All') ='" + library + "' ";
                }
                if (booktype != "All")
                {
                    selectQry = selectQry + " AND ISNULL(Book_Type,'All') = '" + StrBookType + "'";
                }
                if (CardCategory != "All")
                {
                    selectQry = selectQry + " AND ISNULL(CardCat,'All') ='" + CardCategory + "' ";
                }

                if (department != "")
                {
                    selectQry = selectQry + " AND D.Dept_Code IN('" + department + "') and d.dept_name=M.code_descp  ";
                }
                if (staffType != "")
                {
                    selectQry = selectQry + " AND T.StfType IN ('" + staffType + "')  ";
                }
                if (staffCat != "")
                {
                    selectQry = selectQry + " AND C.CAtegory_Name IN('" + staffCat + "') ";
                }
                selectQry = selectQry + "ORDER BY Dept_Name,Staff_Name ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");

                #endregion
                int sno = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtLibCardMas = new DataTable();
                    DataRow drow;
                    dtLibCardMas.Columns.Add("SNo", typeof(string));
                    dtLibCardMas.Columns.Add("Staff Code", typeof(string));
                    dtLibCardMas.Columns.Add("Staff Name", typeof(string));
                    dtLibCardMas.Columns.Add("Department", typeof(string));
                    dtLibCardMas.Columns.Add("No.Of.Token", typeof(string));
                    dtLibCardMas.Columns.Add("No.Of.Days", typeof(string));
                    dtLibCardMas.Columns.Add("Fine", typeof(string));
                    dtLibCardMas.Columns.Add("Book Type", typeof(string));
                    dtLibCardMas.Columns.Add("Category", typeof(string));
                    dtLibCardMas.Columns.Add("Student Category", typeof(string));
                    dtLibCardMas.Columns.Add("Card Category", typeof(string));
                    dtLibCardMas.Columns.Add("Library", typeof(string));


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        drow = dtLibCardMas.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Staff Code"] = Convert.ToString(ds.Tables[0].Rows[i]["Code"]);
                        drow["Staff Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Staff_Name"]);
                        drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                        drow["No.Of.Token"] = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_Token"]);
                        drow["No.Of.Days"] = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_Days"]);
                        drow["Fine"] = Convert.ToString(ds.Tables[0].Rows[i]["Fine"]);
                        drow["Book Type"] = Convert.ToString(ds.Tables[0].Rows[i]["Book_Type"]);
                        drow["Category"] = Convert.ToString(ds.Tables[0].Rows[i]["Category"]);
                        drow["Student Category"] = Convert.ToString(ds.Tables[0].Rows[i]["StudCategory"]);
                        drow["Card Category"] = Convert.ToString(ds.Tables[0].Rows[i]["CardCat"]);
                        drow["Library"] = Convert.ToString(ds.Tables[0].Rows[i]["Lib_Name"]);
                        dtLibCardMas.Rows.Add(drow);
                    }
                    chkGridSelectAll.Visible = true;
                    grdLibCardMas.DataSource = dtLibCardMas;
                    grdLibCardMas.DataBind();
                    grdLibCardMas.Visible = true;
                    print.Visible = true;
                    divspread.Visible = true;
                    lblvalidation1.Text = "";
                    txtexcelname.Text = "";
                }
                else
                {
                    divspread.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Found";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void grdLibCardMas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[4].Visible = false;
        }
    }

    protected void grdLibCardMas_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex == 7)
        {
            DivCellClick.Visible = true;
            DivFineCellClick.Visible = true;
        }
        //bool BlnIsSel = false;
        //int CheckCount = 0;
        //foreach (GridViewRow gvrow in grdLibCardMas.Rows)
        //{
        //    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");

        //    if (chk.Checked == true)
        //    {
        //        CheckCount++;
        //    }
        //}
        //if (CheckCount < 0)
        //{
        //    imgdiv2.Visible = true;
        //    lbl_alert.Text = "Select the row to update";
        //}
        //else
        //{
        //    //DivCellClick.Visible = true;
        //    //DivFineCellClick.Visible = true;
        //}
    }

    protected void BtnCellClikOk_OnClick(object sender, EventArgs e)
    {
        string varCode = "";
        string VarBatch = "";
        string VarBookType = "";
        string VarCat = "";
        string VarStudCat = "";
        string VarCardCat = "";
        string VarLibName = "";
        int intFineType = 0;
        string NoOfDays = "";
        string sql = "";
        int update = 0;
        int UptCnt = 0;
        if (chkCellNoOfDays.Checked == true)
        {
            NoOfDays = txt_NoDaysCell.Text;
            if (string.IsNullOrEmpty(NoOfDays) || NoOfDays == "0")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter no. of days";
                return;
            }
        }
        if (rbStudent.Checked)
        {
            foreach (GridViewRow gvrow in grdLibCardMas.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                if (chk.Checked == true)
                {
                    VarBatch = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[2].Text);
                    varCode = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[4].Text);
                    VarBookType = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[8].Text);
                    VarCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[9].Text);
                    VarStudCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[10].Text);
                    VarCardCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[11].Text);
                    VarLibName = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[12].Text);

                    if (chkCellFine.Checked == true)
                    {
                        if (RbPerDay.Checked == true)
                        {
                            intFineType = 0;
                        }
                        else
                        {
                            intFineType = 1;
                        }
                        string fine = Convert.ToString(txt_CellClickFine.Text);
                        double fineAmt = 0;
                        if (!string.IsNullOrEmpty(fine))
                        {
                            fineAmt = Convert.ToDouble(fine);
                        }
                        sql = "UPDATE Lib_Master SET FineType ='" + intFineType + "',Fine ='" + fineAmt + "' WHERE Batch_Year ='" + VarBatch + "' AND Code ='" + varCode + "' AND Is_Staff = 0   AND ISNULL(Category,'All') = '" + VarCat + "' AND ISNULL(StudCategory,'All') = '" + VarStudCat + "' AND ISNULL(TransLibCode,'All') ='" + VarLibName + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                        update = d2.update_method_wo_parameter(sql, "text");
                        UptCnt++;
                    }
                    if (chkCellNoOfDays.Checked == true)
                    {
                        string Day = Convert.ToString(txt_NoDaysCell.Text);
                        double NoOfDay = 0;
                        if (!string.IsNullOrEmpty(Day))
                        {
                            NoOfDay = Convert.ToDouble(Day);
                        }
                        string DayRef = Convert.ToString(txt_NoDayRef.Text);
                        double NoOfDayRef = 0;
                        if (!string.IsNullOrEmpty(DayRef))
                        {
                            NoOfDayRef = Convert.ToDouble(DayRef);
                        }
                        sql = "UPDATE Lib_Master SET no_of_days='" + NoOfDay + "',Ref_NoofDays='" + NoOfDayRef + "' WHERE Batch_Year ='" + VarBatch + "' AND Code ='" + varCode + "' AND Is_Staff = 0 AND ISNULL(Category,'All') = '" + VarCat + "' AND ISNULL(StudCategory,'All') = '" + VarStudCat + "' AND ISNULL(TransLibCode,'All') ='" + VarLibName + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                        update = d2.update_method_wo_parameter(sql, "text");
                        UptCnt++;
                    }
                }
            }
        }
        if (rbStaff.Checked)
        {
            foreach (GridViewRow gvrow in grdLibCardMas.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                if (chk.Checked == true)
                {
                    VarBatch = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[2].Text);
                    varCode = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[4].Text);
                    VarBookType = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[8].Text);
                    VarCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[9].Text);
                    VarStudCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[10].Text);
                    VarCardCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[11].Text);
                    VarLibName = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[12].Text);

                    if (chkCellFine.Checked == true)
                    {
                        if (RbPerDay.Checked == true)
                        {
                            intFineType = 0;
                        }
                        else
                        {
                            intFineType = 1;
                        }
                        string fine = Convert.ToString(txt_CellClickFine.Text);
                        double fineAmt = 0;
                        if (!string.IsNullOrEmpty(fine))
                        {
                            fineAmt = Convert.ToDouble(fine);
                        }
                        sql = "UPDATE Lib_Master SET FineType ='" + intFineType + "',Fine ='" + fineAmt + "' WHERE Batch_Year ='" + VarBatch + "' AND Code ='" + varCode + "' AND Is_Staff = 1   AND ISNULL(Category,'All') = '" + VarCat + "' AND ISNULL(StudCategory,'All') = '" + VarStudCat + "' AND ISNULL(TransLibCode,'All') ='" + VarLibName + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                        update = d2.update_method_wo_parameter(sql, "text");
                    }
                    if (chkCellNoOfDays.Checked == true)
                    {
                        string Day = Convert.ToString(txt_NoDaysCell.Text);
                        double NoOfDay = 0;
                        if (!string.IsNullOrEmpty(Day))
                        {
                            NoOfDay = Convert.ToDouble(Day);
                        }
                        string DayRef = Convert.ToString(txt_NoDayRef.Text);
                        double NoOfDayRef = 0;
                        if (!string.IsNullOrEmpty(DayRef))
                        {
                            NoOfDayRef = Convert.ToDouble(DayRef);
                        }
                        sql = "UPDATE Lib_Master SET no_of_days='" + NoOfDay + "',Ref_NoofDays='" + NoOfDayRef + "' WHERE Batch_Year ='" + VarBatch + "' AND Code ='" + varCode + "' AND Is_Staff = 1 AND ISNULL(Category,'All') = '" + VarCat + "' AND ISNULL(StudCategory,'All') = '" + VarStudCat + "' AND ISNULL(TransLibCode,'All') ='" + VarLibName + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                        update = d2.update_method_wo_parameter(sql, "text");
                    }
                }
            }
        }
        if (UptCnt > 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Fine details saved sucessfully";
            DivCellClick.Visible = false;
            btn_MainGo_OnClick(sender, e);
        }
    }

    protected void BtnCellClikExit_OnClick(object sender, EventArgs e)
    {
        DivCellClick.Visible = false;
        DivFineCellClick.Visible = false;
    }

    protected void grdLibCardMas_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void chkCellFine_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkCellFine.Checked == true)
        {
            txt_CellClickFine.Enabled = true;
            txt_NoDaysCell.Enabled = false;
            txt_NoDayRef.Enabled = false;
            RbPerDay.Enabled = true;
            RbPerWeek.Enabled = true;
        }
        else
        {
            txt_CellClickFine.Enabled = false;
            RbPerDay.Enabled = false;
            RbPerWeek.Enabled = false;
        }
        if (chkCellNoOfDays.Checked == true && chkCellFine.Checked == true)
        {
            txt_CellClickFine.Enabled = true;
            txt_NoDaysCell.Enabled = true;
            txt_NoDayRef.Enabled = true;
            RbPerDay.Enabled = true;
            RbPerWeek.Enabled = true;
        }
    }

    protected void chkCellNoOfDays_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkCellNoOfDays.Checked == true)
        {
            txt_CellClickFine.Enabled = false;
            txt_NoDaysCell.Enabled = true;
            txt_NoDayRef.Enabled = true;
            RbPerDay.Enabled = false;
            RbPerWeek.Enabled = false;
        }
        else
        {
            txt_NoDaysCell.Enabled = false;
            txt_NoDayRef.Enabled = false;
        }
        if (chkCellNoOfDays.Checked == true && chkCellFine.Checked == true)
        {
            txt_CellClickFine.Enabled = true;
            txt_NoDaysCell.Enabled = true;
            txt_NoDayRef.Enabled = true;
            RbPerDay.Enabled = true;
            RbPerWeek.Enabled = true;
        }
    }

    protected void RbPerDay_OnCheckedChanged(object sender, EventArgs e)
    {
        RbPerWeek.Checked = false;
    }

    protected void RbPerWeek_OnCheckedChanged(object sender, EventArgs e)
    {
        RbPerDay.Checked = false;
        txt_CellClickFine.Enabled = false;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        int selectCount = 0;
        foreach (GridViewRow gvrow in grdLibCardMas.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
            if (chk.Checked == true)
            {
                selectCount++;
            }
        }
        if (selectCount > 0)
        {
            SureDivDelete.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select the row to delete";
        }
    }

    protected void btn_DeleteYes_Click(object sender, EventArgs e)
    {
        try
        {
            string DegreeCode = string.Empty;
            string selQry = string.Empty;
            string DelQry = string.Empty;
            string StrCount = string.Empty;
            double count = 0;
            int intDelCount = 0;
            int intundelcount = 0;
            int delete = 0;
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);
            foreach (GridViewRow gvrow in grdLibCardMas.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                if (chk.Checked == true)
                {
                    string VarBatch = string.Empty;
                    string VarBookType = string.Empty;
                    string VarCat = string.Empty;
                    string VarStudCat = string.Empty;
                    string VarCardCat = string.Empty;
                    string VarTransLib = string.Empty;
                    string varDegree = string.Empty;
                    string VarTransLibCode = string.Empty;
                    string staffcode = string.Empty;
                    if (rbStudent.Checked)
                    {
                        VarBatch = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[2].Text);
                        varDegree = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[4].Text);
                        VarBookType = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[8].Text);
                        VarCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[9].Text);
                        // VarStudCat = Convert.ToString(SpreadDet.Sheets[0].Cells[rowStud, 9].Text);
                        VarCardCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[11].Text);
                        VarTransLib = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[12].Text);
                        string[] splitdeg = varDegree.Split('~');
                        string C_ID = splitdeg[0];
                        string dept_Code = splitdeg[1];

                        DegreeCode = d2.GetFunction("SELECT Degree_Code FROM Degree WHERE Course_ID =" + C_ID + " AND Dept_Code =" + dept_Code + " AND College_Code =" + college + "");
                        if (VarTransLib != "All")
                        {
                            VarTransLib = d2.GetFunction("select lib_code from library where lib_name='" + VarTransLib + "'");
                        }
                        selQry = "SELECT ISNULL(COUNT(*),0) FROM Borrow B,TokenDetails T,Registration R,Degree G WHERE (B.Roll_No = R.Roll_No) AND B.Token_No = T.Token_No AND B.Is_Staff = 0 AND R.Degree_Code = G.Degree_Code AND Batch_Year =" + VarBatch + " AND R.Degree_Code ='" + DegreeCode + "' AND G.College_Code =" + college + " AND ISNULL(Category,'All') ='" + VarCat + "' AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "'  AND ISNULL(CardCat,'All') ='" + VarCardCat + "' "; //AND ISNULL(StudCategory,'All') ='" + VarStudCat + "' 
                        StrCount = d2.GetFunction(selQry);
                        double.TryParse(StrCount, out count);
                        if (count == 0)
                        {
                            DelQry = "DELETE TokenDetails FROM Registration R WHERE (TokenDetails.Roll_No = R.Roll_No )  AND Batch_Year =" + VarBatch + " AND Degree_Code =" + DegreeCode + " AND Is_Staff = 0  AND ISNULL(Category,'All') ='" + VarCat + "' AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "'";
                            delete = d2.update_method_wo_parameter(DelQry, "Text");

                            DelQry = " DELETE FROM Lib_Master WHERE Batch_Year =" + VarBatch + " AND Code ='" + varDegree + "' AND Is_Staff =0  AND ISNULL(Category,'All') ='" + VarCat + "' AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "'  AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                            intDelCount = intDelCount + delete;
                        }
                        else
                        {
                            intundelcount = intundelcount + 1;
                        }
                    }
                    if (rbStaff.Checked)
                    {
                        staffcode = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[2].Text);
                        VarBookType = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[8].Text);
                        VarCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[9].Text);
                        VarCardCat = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[10].Text);
                        VarTransLib = Convert.ToString(grdLibCardMas.Rows[RowCnt].Cells[11].Text);
                        if (VarTransLib != "All")
                        {
                            VarTransLib = d2.GetFunction("select lib_code from library where lib_name='" + VarTransLib + "'");
                        }

                        selQry = "SELECT ISNULL(COUNT(*),0) FROM Borrow B,TokenDetails T,StaffMaster M WHERE (B.Roll_No = M.Staff_Code ) AND B.Token_No = T.Token_No AND B.Is_Staff = 1 AND M.Staff_Code ='" + staffcode + "' AND M.College_Code =" + college + " AND ISNULL(Category,'All') ='" + VarCat + "' AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "'  AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                        StrCount = d2.GetFunction(selQry);
                        double.TryParse(StrCount, out count);
                        if (count == 0)
                        {
                            DelQry = "DELETE TokenDetails FROM StaffMaster M WHERE (TokenDetails.Roll_No = M.Staff_Code ) AND M.Staff_Code ='" + staffcode + "' AND Is_Staff = 1 AND ISNULL(Category,'All') ='" + VarCat + "'  AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "'  AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                            delete = d2.update_method_wo_parameter(DelQry, "Text");

                            DelQry = "DELETE Lib_Master FROM StaffMaster M WHERE (Lib_Master.Code = M.Staff_Code ) AND M.Staff_Code ='" + staffcode + "' AND Is_Staff = 1 AND ISNULL(Category,'All') ='" + VarCat + "'   AND ISNULL(TransLibCode,'All') ='" + VarTransLib + "' AND ISNULL(Book_Type,'All') ='" + VarBookType + "' AND ISNULL(CardCat,'All') ='" + VarCardCat + "' ";
                            delete = d2.update_method_wo_parameter(DelQry, "Text");
                            intDelCount = intDelCount + delete;
                        }
                        else
                        {
                            intundelcount = intundelcount + 1;
                        }
                    }
                }
            }
            if (intDelCount > 0 && intundelcount > 0)
            {
                SureDivDelete.Visible = false;               
                imgdiv2.Visible = true;                
                lbl_alert.Text = "Deleted sucessfully other than issued card";
            }
            else if (intDelCount > 0 && intundelcount == 0)
            {
                SureDivDelete.Visible = false;
                imgdiv2.Visible = true;
                chkGridSelectAll.Visible = false;
                print.Visible = false;
                lbl_alert.Text = "Selected department are deleted sucessfully"; 
            }
            else if (intDelCount == 0)
            {
                SureDivDelete.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No cards are deleted, cards are issued ";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void btn_DeleteNo_Click(object sender, EventArgs e)
    {
        SureDivDelete.Visible = false;
    }

    #endregion

    protected void cb_FinePerWeek_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_FinePerWeek.Checked)
            txt_FinePerWeek.Enabled = false;
        else
            txt_FinePerWeek.Enabled = true;
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    #region Card Generation

    protected void btn_GenerateCard_OnClick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_NoofCards.Text) && !string.IsNullOrEmpty(txt_NoofDays.Text))
        {
            Surediv.Visible = true;
            LblCancel.Visible = true;
        }
        else
        {
            if (string.IsNullOrEmpty(txt_NoofCards.Text) && string.IsNullOrEmpty(txt_NoofDays.Text))
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter No. Of Cards and No. Of Days";
            }
            else
            {
                if (string.IsNullOrEmpty(txt_NoofCards.Text))
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Enter No. Of Cards";
                }

                if (string.IsNullOrEmpty(txt_NoofDays.Text))
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Enter No. Of Days";
                }
            }
        }
    }

    protected void btnSure_yes_Click(object sender, EventArgs e)
    {
        try
        {
            Surediv.Visible = false;
            string selectQry = string.Empty;
            string selQry = string.Empty;
            string insertQry = string.Empty;
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string library = Convert.ToString(ddlLibrary.SelectedValue);
            string booktype = Convert.ToString(ddlBookType.SelectedValue);
            string CardCategory = Convert.ToString(ddl_CardCatogery.SelectedValue);

            string StrBookType = string.Empty;
            string bookBankCat = string.Empty;
            string StubookBankCat = string.Empty;
            string StrTransLibCode = string.Empty;
            string StrCardCat = string.Empty;
            int intGenCout = 0;
            if (rbStudent.Checked)
            {
                #region Query
                string batch = getCblSelectedValue(cbl_BatchYear);
                string degree = getCblSelectedValue(cbl_Department);

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

                if (cb_BookBank.Checked == true)
                {
                    string BookBankCategory = Convert.ToString(ddl_BookBank.SelectedValue);
                    bookBankCat = "Book Bank";
                    if (BookBankCategory == "Book Bank")
                    {
                        StubookBankCat = "SC/ST Category";
                    }
                    else
                        StubookBankCat = "All";
                }
                else
                {
                    bookBankCat = "All";
                    StubookBankCat = "All";
                }
                if (library != "All")
                    StrTransLibCode = library;
                else
                    StrTransLibCode = "All";
                if (CardCategory != "All")
                    StrCardCat = CardCategory;
                else
                    StrCardCat = "All";
                if (batch == "" && degree == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select the batch and department to generate card";
                    return;
                }
                if (batch != "")
                {
                    batch = " AND Batch_Year IN ('" + batch + "')";
                }
                if (degree != "")
                {
                    //string deg = "";
                    //string[] splitdeg = degree.Split(new string[] { "','" }, StringSplitOptions.None);
                    //for (int i = 0; i < splitdeg.Length; i++)
                    //{
                    //    string[] aa = splitdeg[i].Split('~');
                    //    if (deg == "")
                    //        deg = aa[1];
                    //    else
                    //        deg = deg + "','" + aa[1];
                    //}
                    degree = " AND Degree_Code IN ('" + degree + "')";
                }
                if (degree != "" && batch != "")
                {
                    selectQry = "SELECT COUNT(*) as Count FROM Registration WHERE Exam_Flag ='OK' AND DelFlag = 0 AND CC = 0 " + batch + " " + degree + " ";
                    string count = d2.GetFunction(selectQry);
                    if (count == "0")
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No student to generate card";
                        return;
                    }
                }
                for (int batchYr = 0; batchYr < cbl_BatchYear.Items.Count; batchYr++)
                {
                    if (cbl_BatchYear.Items[batchYr].Selected)
                    {
                        for (int dept = 0; dept < cbl_Department.Items.Count; dept++)
                        {
                        lblDept:
                            if (cbl_Department.Items[dept].Selected)
                            {
                                string Degree_Code = cbl_Department.Items[dept].Value;
                                //string[] DegreeSplit = Degree_Code.Split('~');
                                //string deg = DegreeSplit[1];
                                selectQry = "Select distinct degree.course_id,degree.dept_code,degree.degree_code from degree,registration where degree.college_code='" + college + "'and degree.degree_code=registration.degree_code and registration.batch_year='" + cbl_BatchYear.Items[batchYr].Value + "'and registration.degree_code='" + Degree_Code + "'";

                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQry, "Text");
                                string StrDegCode = "";
                                string StrDegDesc = "";
                                string GetCourseName = "";
                                string GetDeptName = "";
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        string Deg_Code = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                                        string C_id = Convert.ToString(ds.Tables[0].Rows[0]["course_id"]);
                                        string dept_Code = Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);

                                        if (!string.IsNullOrEmpty(C_id) && !string.IsNullOrEmpty(dept_Code))
                                        {
                                            StrDegCode = C_id + "~" + dept_Code;
                                            selectQry = "select Course_Name from course where Course_Id='" + C_id + "'";
                                            GetCourseName = d2.GetFunction(selectQry);
                                            selectQry = "select Dept_Name from department where Dept_code='" + dept_Code + "'";
                                            GetDeptName = d2.GetFunction(selectQry);
                                            StrDegDesc = GetCourseName + "-" + GetDeptName;
                                        }
                                        else
                                        {
                                            dept++;
                                            goto lblDept;
                                        }
                                    }
                                }
                                selQry = "SELECT * FROM Lib_Master WHERE Batch_Year ='" + cbl_BatchYear.Items[batchYr].Value + "' AND Code ='" + StrDegCode + "' AND Is_Staff = 0  ";

                                if (cb_BookBank.Checked == false)
                                {
                                    selQry += " AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All'";
                                }
                                else
                                {
                                    //string BookBankCategory = Convert.ToString(ddl_BookBank.SelectedValue);
                                    if (cb_BookBank.Checked == true && ddl_BookBank.SelectedValue == "Book Bank")
                                    {
                                        selQry += " AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category'";
                                    }
                                    else if (cb_BookBank.Checked == true && ddl_BookBank.SelectedValue == "All")
                                    {
                                        selQry += " AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                                    }
                                }
                                if (library != "All")
                                {
                                    selQry += " AND ISNULL(TransLibCode,'All') ='" + library + "'";
                                }
                                else if (library == "All")
                                {
                                    selQry += " AND ISNULL(TransLibCode,'All') ='All'";
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
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selQry, "Text");
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    Degree_Code = cbl_Department.Items[dept].Value;
                                    //DegreeSplit = Degree_Code.Split('~');
                                    //deg = DegreeSplit[1];
                                    selQry = "SELECT * FROM Registration WHERE Batch_Year ='" + cbl_BatchYear.Items[batchYr].Value + "' AND Degree_Code ='" + Degree_Code + "' AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' ";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selQry, "Text");
                                    int fine = 0;
                                    if (cb_FinePerWeek.Checked)
                                        fine = 1;
                                    else
                                        fine = 0;
                                    int NoofDaysReferal = 0;
                                    int FinePerWeek = 0;

                                    if (txt_NoofDaysReferal.Text == "")
                                        NoofDaysReferal = 0;
                                    else
                                        NoofDaysReferal = Convert.ToInt32(txt_NoofDaysReferal.Text);

                                    if (txt_FinePerWeek.Text == "")
                                        FinePerWeek = 0;
                                    else
                                        FinePerWeek = Convert.ToInt32(txt_FinePerWeek.Text);
                                    int insert = 0;

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine, category, studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES('" + StrDegCode + "','" + StrDegDesc + "'," + cbl_BatchYear.Items[batchYr].Value + "," + txt_NoofCards.Text + "," + txt_NoofDays.Text + "," + FinePerWeek + ",0,0,'" + bookBankCat + "','" + StubookBankCat + "','" + StrBookType + "',0," + NoofDaysReferal + ",'" + StrTransLibCode + "'," + fine + ",'" + StrCardCat + "') ";

                                        insert = d2.update_method_wo_parameter(insertQry, "TEXT");

                                        string MaxCard = string.Empty;
                                        int NoOfCards = Convert.ToInt32(txt_NoofCards.Text);
                                        int StrMaxCard = 0;
                                        string StrTokNo = string.Empty;
                                        string StrCardCount = string.Empty;
                                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                        {
                                            //ProgreeDiv.Visible = true;
                                            string rollno = Convert.ToString(ds.Tables[0].Rows[j]["Roll_No"]);
                                            string stuName = Convert.ToString(ds.Tables[0].Rows[j]["Stud_Name"]);
                                            selQry = "SELECT ISNULL(COUNT(*),0) FROM TokenDetails WHERE Roll_No ='" + rollno + "' AND Is_Staff = 0 ";
                                            MaxCard = d2.GetFunction(selQry);
                                            StrMaxCard = Convert.ToInt32(MaxCard);
                                            for (int k = StrMaxCard + 1; k <= StrMaxCard + NoOfCards; k++)
                                            {

                                                StrTokNo = rollno + "A." + k;
                                                selQry = "Select ISNULL(COUNT(*),0) from tokendetails where Roll_No ='" + rollno + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 0 ";
                                                StrCardCount = d2.GetFunction(selQry);
                                                string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                                string Date = DateTime.Now.ToString("MM/dd/yyy");
                                                if (StrCardCount == "0")
                                                {
                                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time, is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + rollno + "','" + stuName + "','0','" + StrDegDesc + "','" + Date + "', '" + Time + "','0','" + bookBankCat + "','" + StubookBankCat + "',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                                    intGenCout = intGenCout + 1;
                                                    if (intGenCout > 0)
                                                    {

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
                if (intGenCout > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Card(s) Generated Successfully";
                    btn_MainGo_OnClick(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No cards have been Generated";
                }
                #endregion
            }
            if (rbStaff.Checked)
            {
                #region Query
                string degree = getCblSelectedValue(cbl_Department);
                string staffCat = getCblSelectedText(cbl_StaffCatogery);
                string staffType = getCblSelectedValue(cbl_StaffType);
                string strCat = string.Empty;
                string strStafftype = string.Empty;
                string strdept = string.Empty;

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

                if (cb_BookBank.Checked == true)
                {
                    string BookBankCategory = Convert.ToString(ddl_BookBank.SelectedValue);
                    bookBankCat = "Book Bank";
                    if (BookBankCategory == "Book Bank")
                    {
                        StubookBankCat = "SC/ST Category";
                    }
                    else
                        StubookBankCat = "All";
                }
                else
                {
                    bookBankCat = "All";
                    StubookBankCat = "All";
                }
                if (library != "All")
                    StrTransLibCode = library;
                else
                    StrTransLibCode = "All";
                if (CardCategory != "All")
                    StrCardCat = CardCategory;
                else
                    StrCardCat = "All";
                if (degree != "")
                {
                    degree = " AND T.Dept_Code IN ('" + degree + "')";
                }
                if (staffCat != "")
                {
                    strCat = " AND C.CAtegory_Name IN('" + staffCat + "') ";
                }
                if (staffType != "")
                {
                    strStafftype = " AND T.StfType IN ('" + staffType + "')  ";
                }
                selectQry = " SELECT * FROM StaffMaster M,StaffTrans T,HrDept_Master D,StaffCategorizer C WHERE M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Category_Code = C.Category_Code AND C.College_Code = M.College_Code AND M.College_Code = D.College_Code AND Resign = 0 AND Settled = 0 AND T.Latestrec = 1 AND M.College_Code =" + college + " " + degree + " " + strCat + " " + strStafftype + " ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");

                string StrDegDesc = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Staff_Name = Convert.ToString(ds.Tables[0].Rows[i]["Staff_Name"]);
                        StrDegDesc = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["Staff_Code"]);

                        selQry = "SELECT * FROM Lib_Master WHERE Code ='" + staff_Code + "' AND Is_Staff = 1";

                        if (cb_BookBank.Checked == false)
                        {
                            selQry += " AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All'";
                        }
                        else
                        {
                            if (cb_BookBank.Checked == true && ddl_BookBank.SelectedValue == "Book Bank")
                            {
                                selQry += " AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = 'SC/ST Category'";
                            }
                            else if (cb_BookBank.Checked == true && ddl_BookBank.SelectedValue == "All")
                            {
                                selQry += " AND ISNULL(Category,'All') = 'Book Bank' AND ISNULL(StudCategory,'All') = 'All' ";
                            }
                        }
                        if (library != "All")
                        {
                            selQry += " AND ISNULL(TransLibCode,'All') ='" + library + "'";
                        }
                        else if (library == "All")
                        {
                            selQry += " AND ISNULL(TransLibCode,'All') ='All'";
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
                        dsprint.Clear();
                        dsprint = d2.select_method_wo_parameter(selQry, "Text");

                        if (dsprint.Tables[0].Rows.Count == 0)
                        {
                            int fine = 0;
                            if (cb_FinePerWeek.Checked)
                                fine = 1;
                            else
                                fine = 0;
                            int NoofDaysReferal = 0;
                            int FinePerWeek = 0;

                            if (txt_NoofDaysReferal.Text == "")
                                NoofDaysReferal = 0;
                            else
                                NoofDaysReferal = Convert.ToInt32(txt_NoofDaysReferal.Text);

                            if (txt_FinePerWeek.Text == "")
                                FinePerWeek = 0;
                            else
                                FinePerWeek = Convert.ToInt32(txt_FinePerWeek.Text);
                            int insert = 0;

                            insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine, category, studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES('" + staff_Code + "','" + StrDegDesc + "',0," + txt_NoofCards.Text + "," + txt_NoofDays.Text + "," + FinePerWeek + ",1,0,'" + bookBankCat + "','" + StubookBankCat + "','" + StrBookType + "',0," + NoofDaysReferal + ",'" + StrTransLibCode + "'," + fine + ",'" + StrCardCat + "') ";
                            insert = d2.update_method_wo_parameter(insertQry, "TEXT");

                            string MaxCard = string.Empty;
                            int NoOfCards = Convert.ToInt32(txt_NoofCards.Text);
                            int StrMaxCard = 0;
                            string StrTokNo = string.Empty;
                            string StrCardCount = string.Empty;

                            //ProgreeDiv.Visible = true;

                            selQry = "SELECT ISNULL(COUNT(*),0) FROM TokenDetails WHERE Roll_No ='" + staff_Code + "' AND Is_Staff = 1 ";
                            MaxCard = d2.GetFunction(selQry);
                            StrMaxCard = Convert.ToInt32(MaxCard);
                            for (int k = StrMaxCard + 1; k <= StrMaxCard + NoOfCards; k++)
                            {
                                StrTokNo = staff_Code + "A." + k;
                                selQry = "Select ISNULL(COUNT(*),0) from tokendetails where Roll_No ='" + staff_Code + "' AND token_no='" + StrTokNo + "' AND Is_Staff = 1 ";
                                StrCardCount = d2.GetFunction(selQry);
                                string Time = DateTime.Now.ToString("HH:MM:ss tt");
                                string Date = DateTime.Now.ToString("MM/dd/yyy");
                                if (StrCardCount == "0")
                                {
                                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time, is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + staff_Code + "','" + Staff_Name + "','1','" + StrDegDesc + "','" + Date + "', '" + Time + "','0','" + bookBankCat + "','" + StubookBankCat + "',0,'','" + StrTransLibCode + "','" + StrBookType + "','" + StrCardCat + "')";
                                    insert = d2.update_method_wo_parameter(insertQry, "Text");
                                    intGenCout = intGenCout + 1;
                                    if (intGenCout > 0)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }


                if (intGenCout > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Card(s) Generated Successfully";
                    btn_MainGo_OnClick(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No cards have been Generated";
                }
                #endregion
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }


    }

    protected void btnSure_no_Click(object sender, EventArgs e)
    {
        Surediv.Visible = false;
        imgdiv2.Visible = true;
        lbl_alert.Text = "Cards are not generated";
    }

    #endregion

    #region Renewal Content

    protected void lnkbtn_SetRenewal_OnClick(object sender, EventArgs e)
    {
        PSetRenewal.Visible = true;

        txt_BatchYearNEW.Text = "--Select--";
        txt_DepartmentNEW.Text = "--Select--";
        txt_StaffCatogeryNEW.Text = "--Select--";
        DivRenew.Visible = false;
        dtRenewDays.Columns.Add("RenewFrom");
        dtRenewDays.Columns.Add("RenewTo");
        dtRenewDays.Columns.Add("RenewDays");

        DataRow drow = dtRenewDays.NewRow();
        drow["RenewFrom"] = 1;
        drow["RenewTo"] = "";
        drow["RenewDays"] = "";
        dtRenewDays.Rows.Add(drow);
        ViewState["CurrentTable"] = dtRenewDays;
        GrdAddRenew.DataSource = dtRenewDays;
        GrdAddRenew.DataBind();
        GrdAddRenew.Visible = true;
        DivAddRenew.Visible = true;
        btnRenewalAddRow.Visible = true;
        btnRenewalDelRow.Visible = true;
        btnRenewalSave.Visible = true;
        btnRenewalClose.Visible = true;
    }

    protected void btn_RenewalGoClick_OnClick(object sender, EventArgs e)
    {
        try
        {
            string strBatch = "";
            string strDegree = "";
            string StrStfCat = "";
            string selectQry = string.Empty;
            string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            if (rbStudent.Checked)
            {
                strBatch = getCblSelectedValue(cbl_BatchYearNEW);
                strDegree = getCblSelectedValue(cbl_DepartmentNEW);
                if (strBatch != "")
                {
                    strBatch = " AND F.Semester IN ('" + strBatch + "') ";
                }
                if (strDegree != "")
                {
                    strDegree = " AND F.Degree_Code IN ('" + strDegree + "')";
                }
                selectQry = "SELECT Semester,Course_Name+'-'+Dept_Name Degree,CEILING(FromDay) as FromDay,CEILING(ToDay) as ToDay,FineAmount FROM ExamFineMs F,Degree G,Course C,Department D WHERE F.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND G.College_Code = '" + collegeCode + "' AND ExmFine = 3 " + strBatch + " " + strDegree + " ORDER BY Semester,Course_Name,Dept_Name";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                loadRenewalSpread();
            }
            if (rbStaff.Checked)
            {
                StrStfCat = getCblSelectedValue(cbl_StaffCatogeryNEW);
                if (StrStfCat != "")
                {
                    StrStfCat = "AND Category_Name IN('" + StrStfCat + "')";
                }
                selectQry = "SELECT Semester,Category_Name Degree,CEILING(FromDay) as FromDay,CEILING(ToDay) as ToDay,FineAmount FROM ExamFineMs F,StaffCategorizer C WHERE F.Category_Code = C.Category_Code AND C.College_Code = '" + collegecode + "' AND ExmFine = 3 " + StrStfCat + " ORDER BY Category_Name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                loadRenewalSpread();
            }
            btnRenewalSave.Visible = false;
            btnRenewalAddRow.Visible = false;
            btnRenewalDelRow.Visible = false;
            btnRenewalClose.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void loadRenewalSpread()
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtRenew = new DataTable();
                DataRow drow;
                dtRenew.Columns.Add("Batch Year", typeof(string));
                dtRenew.Columns.Add("Department", typeof(string));
                dtRenew.Columns.Add("Renewal From", typeof(string));
                dtRenew.Columns.Add("Renewal To", typeof(string));
                dtRenew.Columns.Add("Renewal Days", typeof(string));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                    drow = dtRenew.NewRow();
                    drow["Batch Year"] = Convert.ToString(ds.Tables[0].Rows[i]["Semester"]);
                    drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                    drow["Renewal From"] = Convert.ToString(ds.Tables[0].Rows[i]["FromDay"]);
                    drow["Renewal To"] = Convert.ToString(ds.Tables[0].Rows[i]["ToDay"]);
                    drow["Renewal Days"] = Convert.ToString(ds.Tables[0].Rows[i]["FineAmount"]);
                    dtRenew.Rows.Add(drow);
                }
                GrdRenew.DataSource = dtRenew;
                GrdRenew.DataBind();
                GrdRenew.Visible = true;
                DivRenew.Visible = true;
                GrdAddRenew.Visible = false;
                DivAddRenew.Visible = false;
                btnRenewalSave.Visible = false;
                btnRenewalClose.Visible = true;
            }
            else
            {
                DivRenew.Visible = false;
                DivAddRenew.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void btnRenewalClose_OnClick(object sender, EventArgs e)
    {
        PSetRenewal.Visible = false;
    }

    protected void btnRenewAddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataRow drow;
            int row_cnt = GrdAddRenew.Rows.Count;
            if (row_cnt != 0)
            {
                dtRenewDays = (DataTable)ViewState["CurrentTable"];
                dtRenewDays.Rows.RemoveAt(dtRenewDays.Rows.Count - 1);
                drow = dtRenewDays.NewRow();
                int rowcnt = GrdAddRenew.Rows.Count - 1;
                double RenewFrom = 0;
                TextBox DayFrom = (TextBox)GrdAddRenew.Rows[rowcnt].FindControl("txt_RenewFrom");
                if (DayFrom.Text.Trim() != "0")
                {
                    RenewFrom = Convert.ToDouble(DayFrom.Text.Trim());
                }
                TextBox DayTo = (TextBox)GrdAddRenew.Rows[rowcnt].FindControl("txt_RenewTo");
                double RenewTo = 0;
                if (DayTo.Text.Trim() != "0")
                {
                    RenewTo = Convert.ToDouble(DayTo.Text.Trim());
                }
                TextBox Fine = (TextBox)GrdAddRenew.Rows[rowcnt].FindControl("txt_RenewDays");
                double FineAmt = 0;
                if (Fine.Text.Trim() != "0")
                {
                    FineAmt = Convert.ToDouble(Fine.Text.Trim());
                }
                if (RenewTo < RenewFrom)
                {
                    RenewTo = RenewFrom;
                }
                drow["RenewFrom"] = RenewFrom;
                drow["RenewTo"] = RenewTo;
                drow["RenewDays"] = FineAmt;
                dtRenewDays.Rows.Add(drow);
                ViewState["CurrentTable"] = dtRenewDays;
            }
            if (ViewState["CurrentTable"] != null)
            {
                dtRenewDays = (DataTable)ViewState["CurrentTable"];
                drCurrentRow = null;
                int TotRowCnt = Convert.ToInt32(dtRenewDays.Rows.Count);
                if (dtRenewDays.Rows.Count > 0)
                {
                    drCurrentRow = dtRenewDays.NewRow();
                    double RenewFrom = 0;
                    double RenewTo = 0;
                    int rowcnt = GrdAddRenew.Rows.Count - 1;
                    TextBox DayFrom = (TextBox)GrdAddRenew.Rows[rowcnt].FindControl("txt_RenewFrom");
                    if (DayFrom.Text.Trim() != "0")
                    {
                        RenewFrom = Convert.ToDouble(DayFrom.Text.Trim());
                    }
                    TextBox DayTo = (TextBox)GrdAddRenew.Rows[rowcnt].FindControl("txt_RenewTo");
                    if (DayTo.Text.Trim() != "0")
                    {
                        RenewTo = Convert.ToDouble(DayTo.Text.Trim());
                    }
                    if (RenewTo < RenewFrom)
                    {
                        RenewFrom = RenewFrom + 1;
                    }
                    else
                    {
                        RenewFrom = RenewTo + 1;
                    }
                    drCurrentRow["RenewFrom"] = RenewFrom;
                    drCurrentRow["RenewTo"] = "";
                    drCurrentRow["RenewDays"] = "";
                    dtRenewDays.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtRenewDays;
                    GrdAddRenew.DataSource = dtRenewDays;
                    GrdAddRenew.DataBind();
                    GrdAddRenew.Visible = true;
                    DivAddRenew.Visible = true;
                }
                else
                {
                    DataRow drow1;
                    dtRenewDays.Columns.Add("RenewFrom");
                    dtRenewDays.Columns.Add("RenewTo");
                    dtRenewDays.Columns.Add("RenewDays");
                    drow1 = dtRenewDays.NewRow();
                    drow1["RenewFrom"] = "";
                    drow1["RenewTo"] = "";
                    drow1["RenewDays"] = "";
                    dtRenewDays.Rows.Add(drow1);
                    ViewState["CurrentTable"] = dtRenewDays;
                    GrdAddRenew.DataSource = dtRenewDays;
                    GrdAddRenew.DataBind();
                    GrdAddRenew.Visible = true;
                    DivAddRenew.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }

    }

    protected void btnRenewDeleteRow_OnClick(object sender, EventArgs e)
    {
        int rowcnt = Convert.ToInt32(GrdAddRenew.Rows.Count);
        if (rowcnt == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Rows to Delete";
        }
        else
        {
            if (ViewState["CurrentTable"] != null)
            {
                dtRenewDays = (DataTable)ViewState["CurrentTable"];
                dtRenewDays.Rows.RemoveAt(dtRenewDays.Rows.Count - 1);
                ViewState["CurrentTable"] = dtRenewDays;
                GrdAddRenew.DataSource = dtRenewDays;
                GrdAddRenew.DataBind();
                GrdAddRenew.Visible = true;
                DivAddRenew.Visible = true;
            }
        }
    }

    protected void btnRenewalSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string frmday = "";
            string today = "";
            string fnAmt = "";
            int SpreadCnt = GrdAddRenew.Rows.Count;
            string insertQry = "";
            int inscount = 0;
            int delcount = 0;
            string DelQry = "";
            if (SpreadCnt > 0)
            {
                if (rbStudent.Checked)
                {
                    string batch = getCblSelectedValue(cbl_BatchYearNEW);
                    string degree = getCblSelectedValue(cbl_DepartmentNEW);
                    string Deg_Code = string.Empty;
                    string Batch_Year = string.Empty;
                    if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree))
                    {
                        for (int batchYr = 0; batchYr < cbl_BatchYearNEW.Items.Count; batchYr++)
                        {
                            if (cbl_BatchYearNEW.Items[batchYr].Selected)
                            {
                                for (int dept = 0; dept < cbl_DepartmentNEW.Items.Count; dept++)
                                {
                                    if (cbl_DepartmentNEW.Items[dept].Selected)
                                    {
                                        Deg_Code = Convert.ToString(cbl_DepartmentNEW.Items[dept].Value);
                                        Batch_Year = Convert.ToString(cbl_BatchYearNEW.Items[batchYr].Text);
                                        DelQry = "DELETE FROM ExamFineMs WHERE Semester ='" + Batch_Year + "' AND Degree_Code ='" + Deg_Code + "' AND ExmFine = 3 ";
                                        delcount = d2.update_method_wo_parameter(DelQry, "Text");
                                        delcount = d2.update_method_wo_parameter(DelQry, "Text");
                                        foreach (GridViewRow gvrow in GrdAddRenew.Rows)
                                        {
                                            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                                            TextBox txtRenewFrom = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewFrom");
                                            if (txtRenewFrom.Text.Trim() != "")
                                            {
                                                frmday = txtRenewFrom.Text.Trim();
                                            }
                                            TextBox txtRenewTo = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewTo");
                                            if (txtRenewTo.Text.Trim() != "")
                                            {
                                                today = txtRenewTo.Text.Trim();
                                            }
                                            TextBox txtRenewDays = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewDays");
                                            if (txtRenewDays.Text.Trim() != "")
                                            {
                                                fnAmt = txtRenewDays.Text.Trim();
                                            }
                                            if (!string.IsNullOrEmpty(frmday) && !string.IsNullOrEmpty(today) && !string.IsNullOrEmpty(fnAmt))
                                            {
                                                double FineFrmDay = 0;
                                                double FineToDay = 0;
                                                double.TryParse(frmday, out FineFrmDay);
                                                double.TryParse(today, out FineToDay);
                                                if (FineToDay > FineFrmDay)
                                                {
                                                    insertQry = "INSERT INTO ExamFineMS(Degree_Code,Semester,FromDay,ToDay,FineAmount,ExamType,exmfine,Category_Code) VALUES('" + Deg_Code + "','" + Batch_Year + "','" + frmday + "','" + today + "','" + fnAmt + "',0,3,'') ";
                                                    inscount = d2.update_method_wo_parameter(insertQry, "Text");
                                                }
                                                else
                                                {
                                                    imgdiv2.Visible = true;
                                                    lbl_alert.Text = "The Ending range of Day should not be Less than the Starting range";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                imgdiv2.Visible = true;
                                                lbl_alert.Text = "Please fill all the values";
                                                return;
                                            }
                                        }
                                        btn_RenewalGoClick_OnClick(sender, e);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select the Batch year and Department";
                    }
                }
                if (rbStaff.Checked)
                {
                    string staffCat = getCblSelectedText(cbl_StaffCatogeryNEW);
                    string staff_Cat = string.Empty;
                    if (!string.IsNullOrEmpty(staffCat))
                    {
                        for (int staffCategory = 0; staffCategory < cbl_StaffCatogeryNEW.Items.Count; staffCategory++)
                        {
                            if (cbl_StaffCatogeryNEW.Items[staffCategory].Selected)
                            {
                                staff_Cat = Convert.ToString(cbl_StaffCatogeryNEW.Items[staffCategory].Value);

                                DelQry = "DELETE FROM ExamFineMs WHERE Category_Code ='" + staff_Cat + "' AND ExmFine = 3 ";
                                delcount = d2.update_method_wo_parameter(DelQry, "Text");
                                foreach (GridViewRow gvrow in GrdAddRenew.Rows)
                                {
                                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                                    TextBox txtRenewFrom = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewFrom");
                                    if (txtRenewFrom.Text.Trim() != "")
                                    {
                                        frmday = txtRenewFrom.Text.Trim();
                                    }
                                    TextBox txtRenewTo = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewTo");
                                    if (txtRenewTo.Text.Trim() != "")
                                    {
                                        today = txtRenewTo.Text.Trim();
                                    }
                                    TextBox txtRenewDays = (TextBox)GrdAddRenew.Rows[RowCnt].FindControl("txt_RenewDays");
                                    if (txtRenewDays.Text.Trim() != "")
                                    {
                                        fnAmt = txtRenewDays.Text.Trim();
                                    }
                                    if (!string.IsNullOrEmpty(frmday) && !string.IsNullOrEmpty(today) && !string.IsNullOrEmpty(fnAmt))
                                    {
                                        double FineFrmDay = 0;
                                        double FineToDay = 0;
                                        double.TryParse(frmday, out FineFrmDay);
                                        double.TryParse(today, out FineToDay);
                                        if (FineToDay > FineFrmDay)
                                        {
                                            insertQry = "INSERT INTO ExamFineMS(Degree_Code,Semester,FromDay,ToDay,FineAmount,ExamType,exmfine,Category_Code) VALUES(0,0,'" + frmday + "','" + today + "','" + fnAmt + "',0,3,'') ";
                                            inscount = d2.update_method_wo_parameter(insertQry, "Text");
                                        }
                                        else
                                        {
                                            imgdiv2.Visible = true;
                                            lbl_alert.Text = "The Ending range of Day should not be Less than the Starting range";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lbl_alert.Text = "Please fill all the values";
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select the Batch year and Department";
                    }
                }
                if (inscount > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Student/Staff Renewal details saved sucessfully";
                    btn_RenewalGoClick_OnClick(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void textboxValidationRenew(object sender, EventArgs e)
    {
        if (HiddenFieldRenew.Value.Trim().Length == 0)
        {
            return;
        }
        double RenewFrom = 0;
        TextBox TxtRenewTo = (TextBox)sender;
        TextBox TxtRenewFrom = (TextBox)GrdAddRenew.Rows[Convert.ToInt32(HiddenFieldRenew.Value)].FindControl("txt_RenewFrom");
        if (TxtRenewFrom.Text.Trim() != "0")
        {
            RenewFrom = Convert.ToDouble(TxtRenewFrom.Text.Trim());
        }
        double RenewTo = 0;
        if (TxtRenewTo.Text.Trim() != "0")
        {
            RenewTo = Convert.ToDouble(TxtRenewTo.Text.Trim());
        }
        if (RenewTo < RenewFrom)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "The Ending Range of Day should Not Be Less than the Starting Range";
            return;
        }
        HiddenFieldRenew.Value = string.Empty;
    }

    protected void GrdAddRenew_RowDataBound(object sender, GridViewRowEventArgs eventArgs)
    {

        if (eventArgs.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox TxtRenew = (TextBox)eventArgs.Row.FindControl("txt_RenewTo");
            TxtRenew.Attributes.Add("onkeydown", "javascript:return DoPostBackWithRowIndex('" + eventArgs.Row.RowIndex + "');");
        }
    }

    #endregion

    #region Fine Content

    protected void imagebtnRenewpopclose_Click(object sender, EventArgs e)
    {
        PSetRenewal.Visible = false;
    }

    protected void lnkbtn_FinePerWeek_OnClick(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        DivFine.Visible = false;
        grdFine.Visible = false;
        txt_BatchYearFine.Text = "--Select--";
        txt_DepartmentFine.Text = "--Select--";
        txt_StaffCatogeryFine.Text = "--Select--";
        BindDepartment();
        BindBatchYear();
        bindStaffcategory();
        dtAddFine.Columns.Add("DayFrom");
        dtAddFine.Columns.Add("DayTo");
        dtAddFine.Columns.Add("FineAmount");

        DataRow drow = dtAddFine.NewRow();
        drow["DayFrom"] = 1;
        drow["DayTo"] = "";
        drow["FineAmount"] = "";
        dtAddFine.Rows.Add(drow);
        ViewState["CurrentTable"] = dtAddFine;
        grdAddFine.DataSource = dtAddFine;
        grdAddFine.DataBind();
        grdAddFine.Visible = true;
        DivAddFine.Visible = true;
        btnFineSave.Visible = true;
        btnFineClose.Visible = true;
        btnAddRow.Visible = true;
        btnDeleteRow.Visible = true;
    }

    protected void btnAddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable newdt = new DataTable();
            DataRow drow;
            newdt.Columns.Add("DayFrom");
            newdt.Columns.Add("DayTo");
            newdt.Columns.Add("FineAmount");
            int row_cnt = grdAddFine.Rows.Count;
            if (row_cnt != 0)
            {
                dtAddFine = (DataTable)ViewState["CurrentTable"];
                dtAddFine.Rows.RemoveAt(dtAddFine.Rows.Count - 1);
                drow = dtAddFine.NewRow();
                int rowcnt = grdAddFine.Rows.Count - 1;
                double FromDay = 0;
                TextBox DayFrom = (TextBox)grdAddFine.Rows[rowcnt].FindControl("txt_DayFrom");
                if (DayFrom.Text.Trim() != "0")
                {
                    FromDay = Convert.ToDouble(DayFrom.Text.Trim());
                }
                TextBox DayTo = (TextBox)grdAddFine.Rows[rowcnt].FindControl("txt_DayTo");
                double ToDay = 0;
                if (DayTo.Text.Trim() != "0")
                {
                    ToDay = Convert.ToDouble(DayTo.Text.Trim());
                }
                TextBox Fine = (TextBox)grdAddFine.Rows[rowcnt].FindControl("txt_FineAmount");
                double FineAmt = 0;
                if (Fine.Text.Trim() != "0")
                {
                    FineAmt = Convert.ToDouble(Fine.Text.Trim());
                }
                if (ToDay < FromDay)
                {
                    ToDay = FromDay;
                }
                drow["DayFrom"] = FromDay;
                drow["DayTo"] = ToDay;
                drow["FineAmount"] = FineAmt;
                dtAddFine.Rows.Add(drow);
                ViewState["CurrentTable"] = dtAddFine;
            }
            if (ViewState["CurrentTable"] != null)
            {
                dtAddFine = (DataTable)ViewState["CurrentTable"];
                drCurrentRow = null;
                int TotRowCnt = Convert.ToInt32(dtAddFine.Rows.Count);
                if (dtAddFine.Rows.Count > 0)
                {
                    drCurrentRow = dtAddFine.NewRow();
                    double FromDay = 0;
                    double to_day = 0;
                    int rowcnt = grdAddFine.Rows.Count - 1;
                    TextBox DayFrom = (TextBox)grdAddFine.Rows[rowcnt].FindControl("txt_DayFrom");
                    if (DayFrom.Text.Trim() != "0")
                    {
                        FromDay = Convert.ToDouble(DayFrom.Text.Trim());
                    }
                    TextBox DayTo = (TextBox)grdAddFine.Rows[rowcnt].FindControl("txt_DayTo");
                    if (DayTo.Text.Trim() != "0")
                    {
                        to_day = Convert.ToDouble(DayTo.Text.Trim());
                    }
                    if (to_day < FromDay)
                    {
                        FromDay = FromDay + 1;
                    }
                    else
                    {
                        FromDay = to_day + 1;
                    }
                    drCurrentRow["DayFrom"] = FromDay;
                    drCurrentRow["DayTo"] = "";
                    drCurrentRow["FineAmount"] = "";
                    dtAddFine.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtAddFine;
                    grdAddFine.DataSource = dtAddFine;
                    grdAddFine.DataBind();
                    grdAddFine.Visible = true;
                    DivAddFine.Visible = true;
                }
                else
                {
                    DataRow drow1;
                    dtAddFine.Columns.Add("DayFrom");
                    dtAddFine.Columns.Add("DayTo");
                    dtAddFine.Columns.Add("FineAmount");
                    drow1 = dtAddFine.NewRow();
                    drow1["DayFrom"] = "";
                    drow1["DayTo"] = "";
                    drow1["FineAmount"] = "";
                    dtAddFine.Rows.Add(drow1);

                    ViewState["CurrentTable"] = dtAddFine;
                    grdAddFine.DataSource = dtAddFine;
                    grdAddFine.DataBind();
                    grdAddFine.Visible = true;
                    DivAddFine.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }

    }

    protected void btnDeleteRow_OnClick(object sender, EventArgs e)
    {
        int rowcnt = Convert.ToInt32(grdAddFine.Rows.Count);
        if (rowcnt == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Rows to Delete";
        }
        else
        {
            if (ViewState["CurrentTable"] != null)
            {
                dtAddFine = (DataTable)ViewState["CurrentTable"];
                dtAddFine.Rows.RemoveAt(dtAddFine.Rows.Count - 1);
                ViewState["CurrentTable"] = dtAddFine;
                grdAddFine.DataSource = dtAddFine;
                grdAddFine.DataBind();
                grdAddFine.Visible = true;
                DivAddFine.Visible = true;
            }
        }
    }

    protected void textboxValidation(object sender, EventArgs e)
    {
        if (HdnSelectedRowIndex.Value.Trim().Length == 0)
        {
            return;
        }
        double FromDay = 0;
        TextBox TxtDayTo = (TextBox)sender;
        TextBox TxtFrom = (TextBox)grdAddFine.Rows[Convert.ToInt32(HdnSelectedRowIndex.Value)].FindControl("txt_DayFrom");
        if (TxtFrom.Text.Trim() != "0")
        {
            FromDay = Convert.ToDouble(TxtFrom.Text.Trim());
        }
        double ToDay = 0;
        if (TxtDayTo.Text.Trim() != "0")
        {
            ToDay = Convert.ToDouble(TxtDayTo.Text.Trim());
        }
        if (ToDay < FromDay)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "The Ending Range of Day should Not Be Less than the Starting Range";
            return;
        }
        HdnSelectedRowIndex.Value = string.Empty;
    }

    protected void GvUsersRowDataBound(object sender, GridViewRowEventArgs eventArgs)
    {

        if (eventArgs.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox TxtDay = (TextBox)eventArgs.Row.FindControl("txt_DayTo");
            TxtDay.Attributes.Add("onkeydown", "javascript:return DoPostBackWithRowIndex('" + eventArgs.Row.RowIndex + "');");
        }
    }

    protected void btn_FineGoClick_OnClick(object sender, EventArgs e)
    {
        try
        {
            DivFine.Visible = false;
            grdAddFine.Visible = false;
            btnAddRow.Visible = false;
            btnDeleteRow.Visible = false;
            string strBatch = "";
            string strDegree = "";
            string StrStfCat = "";
            string selectQry = string.Empty;
            string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            if (rbStudent.Checked)
            {
                strBatch = getCblSelectedValue(cbl_BatchYearFine);
                strDegree = getCblSelectedValue(cbl_DepartmentFine);
                if (strBatch != "")
                {
                    strBatch = " AND F.Semester IN ('" + strBatch + "') ";
                }
                if (strDegree != "")
                {
                    strDegree = " AND F.Degree_Code IN ('" + strDegree + "')";
                }
                selectQry = "SELECT Semester,Course_Name+'-'+Dept_Name Degree,CEILING(FromDay) as FromDay,CEILING(ToDay) as ToDay,FineAmount FROM ExamFineMs F,Degree G,Course C,Department D WHERE F.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND G.College_Code = '" + collegeCode + "' AND ExmFine = 2 " + strBatch + " " + strDegree + " ORDER BY Semester,Course_Name,Dept_Name";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                loadFineSpread();
            }
            if (rbStaff.Checked)
            {
                StrStfCat = getCblSelectedValue(cbl_StaffCatogeryFine);
                if (StrStfCat != "")
                {
                    StrStfCat = "AND Category_Name IN('" + StrStfCat + "')";
                }
                selectQry = "SELECT Semester,Category_Name Degree,CEILING(FromDay) as FromDay,CEILING(ToDay) as ToDay,FineAmount FROM ExamFineMs F,StaffCategorizer C WHERE F.Category_Code = C.Category_Code AND C.College_Code = '" + collegecode + "' AND ExmFine = 2 " + StrStfCat + " ORDER BY Category_Name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQry, "Text");
                loadFineSpread();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void loadFineSpread()
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtfine = new DataTable();
                DataRow drow;
                dtfine.Columns.Add("Batch Year", typeof(string));
                dtfine.Columns.Add("Department", typeof(string));
                dtfine.Columns.Add("Day From", typeof(string));
                dtfine.Columns.Add("Day To", typeof(string));
                dtfine.Columns.Add("Fine Amount", typeof(string));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                    drow = dtfine.NewRow();
                    drow["Batch Year"] = Convert.ToString(ds.Tables[0].Rows[i]["Semester"]);
                    drow["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                    drow["Day From"] = Convert.ToString(ds.Tables[0].Rows[i]["FromDay"]);
                    drow["Day To"] = Convert.ToString(ds.Tables[0].Rows[i]["ToDay"]);
                    drow["Fine Amount"] = Convert.ToString(ds.Tables[0].Rows[i]["FineAmount"]);
                    dtfine.Rows.Add(drow);
                }
                grdFine.DataSource = dtfine;
                grdFine.DataBind();
                grdFine.Visible = true;
                DivFine.Visible = true;
                btnFineSave.Visible = false;
                btnFineClose.Visible = true;
            }
            else
            {
                DivFine.Visible = false;
                grdFine.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    protected void btnFineClose_OnClick(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        btnFineClose.Visible = false;
    }

    protected void btnFineSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string frmday = "";
            string today = "";
            string fnAmt = "";
            int SpreadCnt = Convert.ToInt32(grdAddFine.Rows.Count);
            string insertQry = "";
            string DelQry = "";
            int inscount = 0;
            int delcount = 0;
            if (SpreadCnt > 0)
            {
                if (rbStudent.Checked)
                {
                    string batch = getCblSelectedValue(cbl_BatchYearFine);
                    string degree = getCblSelectedValue(cbl_DepartmentFine);
                    string Deg_Code = string.Empty;
                    string Batch_Year = string.Empty;
                    if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree))
                    {
                        for (int batchYr = 0; batchYr < cbl_BatchYearFine.Items.Count; batchYr++)
                        {
                            if (cbl_BatchYearFine.Items[batchYr].Selected)
                            {
                                for (int dept = 0; dept < cbl_DepartmentFine.Items.Count; dept++)
                                {
                                    if (cbl_DepartmentFine.Items[dept].Selected)
                                    {
                                        Deg_Code = Convert.ToString(cbl_DepartmentFine.Items[dept].Value);
                                        Batch_Year = Convert.ToString(cbl_BatchYearFine.Items[batchYr].Text);
                                        DelQry = "DELETE FROM ExamFineMs WHERE Semester ='" + Batch_Year + "' AND Degree_Code ='" + Deg_Code + "' AND ExmFine = 2 ";
                                        delcount = d2.update_method_wo_parameter(DelQry, "Text");
                                        foreach (GridViewRow gvrow in grdAddFine.Rows)
                                        {
                                            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                                            TextBox txtFromDay = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_DayFrom");
                                            if (txtFromDay.Text.Trim() != "")
                                            {
                                                frmday = txtFromDay.Text.Trim();
                                            }
                                            TextBox txtToDay = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_DayTo");
                                            if (txtToDay.Text.Trim() != "")
                                            {
                                                today = txtToDay.Text.Trim();
                                            }
                                            TextBox txtFineAmt = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_FineAmount");
                                            if (txtFineAmt.Text.Trim() != "")
                                            {
                                                fnAmt = txtFineAmt.Text.Trim();
                                            }
                                            if (!string.IsNullOrEmpty(frmday) && !string.IsNullOrEmpty(today) && !string.IsNullOrEmpty(fnAmt))
                                            {
                                                double FineFrmDay = 0;
                                                double FineToDay = 0;
                                                FineFrmDay = Convert.ToDouble(frmday);
                                                FineToDay = Convert.ToDouble(today);
                                                if (FineToDay > FineFrmDay)
                                                {
                                                    insertQry = "INSERT INTO ExamFineMS(Degree_Code,Semester,FromDay,ToDay,FineAmount,ExamType,exmfine,Category_Code) VALUES('" + Deg_Code + "','" + Batch_Year + "','" + FineFrmDay + "','" + FineToDay + "','" + fnAmt + "',0,2,'') ";
                                                    inscount = d2.update_method_wo_parameter(insertQry, "Text");
                                                }
                                                else
                                                {
                                                    imgdiv2.Visible = true;
                                                    lbl_alert.Text = "The Ending range of Day should not be Less than the Starting range";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                imgdiv2.Visible = true;
                                                lbl_alert.Text = "Please fill all the values";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select the Batch year and Department";
                    }
                }
                if (rbStaff.Checked)
                {
                    string staffCat = getCblSelectedText(cbl_StaffCatogeryFine);
                    string staff_Cat = string.Empty;
                    if (!string.IsNullOrEmpty(staffCat))
                    {
                        for (int staffCategory = 0; staffCategory < cbl_StaffCatogeryFine.Items.Count; staffCategory++)
                        {
                            if (cbl_StaffCatogeryFine.Items[staffCategory].Selected)
                            {
                                staff_Cat = Convert.ToString(cbl_StaffCatogeryFine.Items[staffCategory].Value);
                                DelQry = "DELETE FROM ExamFineMs WHERE Category_Code ='" + staff_Cat + "' AND ExmFine = 2 ";
                                delcount = d2.update_method_wo_parameter(DelQry, "Text");
                                foreach (GridViewRow gvrow in grdAddFine.Rows)
                                {
                                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                                    TextBox txtFromDay = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_DayFrom");
                                    if (txtFromDay.Text.Trim() != "")
                                    {
                                        frmday = txtFromDay.Text.Trim();
                                    }
                                    TextBox txtToDay = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_DayTo");
                                    if (txtToDay.Text.Trim() != "")
                                    {
                                        today = txtToDay.Text.Trim();
                                    }
                                    TextBox txtFineAmt = (TextBox)grdAddFine.Rows[RowCnt].FindControl("txt_FineAmount");
                                    if (txtFineAmt.Text.Trim() != "")
                                    {
                                        fnAmt = txtFineAmt.Text.Trim();
                                    }
                                    if (!string.IsNullOrEmpty(frmday) && !string.IsNullOrEmpty(today) && !string.IsNullOrEmpty(fnAmt))
                                    {
                                        double FineFrmDay = 0;
                                        double FineToDay = 0;
                                        FineFrmDay = Convert.ToDouble(frmday);
                                        FineToDay = Convert.ToDouble(today);
                                        if (FineToDay > FineFrmDay)
                                        {
                                            insertQry = "INSERT INTO ExamFineMS(Degree_Code,Semester,FromDay,ToDay,FineAmount,ExamType,exmfine,Category_Code) VALUES(0,0,'" + FineFrmDay + "','" + FineToDay + "','" + fnAmt + "',0,2,'') ";
                                            inscount = d2.update_method_wo_parameter(insertQry, "Text");
                                        }
                                        else
                                        {
                                            imgdiv2.Visible = true;
                                            lbl_alert.Text = "The Ending range of Day should not be Less than the Starting range";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lbl_alert.Text = "Please fill all the values";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select the Batch year and Department";
                    }
                }
                if (inscount > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Student/Staff fine details saved sucessfully";
                    btn_FineGoClick_OnClick(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Card_Master");
        }
    }

    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

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
                d2.printexcelreportgrid(grdLibCardMas, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Library Card Master " + '@';
            pagename = "Library_Card_Master.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(grdLibCardMas, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    #endregion

}