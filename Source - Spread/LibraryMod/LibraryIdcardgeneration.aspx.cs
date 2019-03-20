using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_LibraryIdcardgeneration : System.Web.UI.Page
{
    # region fielddeclaration
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable has = new Hashtable();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    DataSet dscode = new DataSet();
    string selroll = string.Empty;
    string selsex = string.Empty;
    string seladm = string.Empty;
    string selstud = string.Empty;
    string selq = string.Empty;
    string degree = string.Empty;
    string admo = "";
    string name = "";
    string gender = "";
    string libid = "";
    string sec = string.Empty;
    string batch = string.Empty;
    string roll = string.Empty;
    string qrydegfilter = string.Empty;
    string qrybranchfilter = string.Empty;
    string qrysemfilter = string.Empty;
    string qrysecfilter = string.Empty;
    string qrybatchfilter = string.Empty;
    string staffcode = "";
    string staffname = "";
    string desgin = "";
    string libid1 = "";
    DataRow dr;
    DataRow dr1;
    DataSet dscode1 = new DataSet();
    string selstaff = string.Empty;
    string dep = string.Empty;
    DataTable studid = new DataTable();
    DataTable staffid = new DataTable();
    static int searchby = 0;
    static string searchclgcode = string.Empty;
    # endregion

    #region pageload
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
                ViewState["NoOfStudents"] = null;
                lib();
                Bindcollege();
                binddeg();
                BindBatchYear();
                branch();
                sem();
                section();
                student();
                studentpop.Visible = true;

            }
        }
        catch
        { }

    }
    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
        {
            query = "SELECT DISTINCT  TOP  100 Roll_No FROM Registration where Roll_No Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by Roll_No";
        }
        else if (searchby == 2)
        {
            query = "SELECT DISTINCT  TOP  100 Stud_Name FROM Registration where Stud_Name Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by Stud_Name";
        }
        else if (searchby == 3)
        {
            query = "SELECT DISTINCT  TOP  100 Roll_Admit FROM Registration where Roll_Admit Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by Roll_Admit";
        }
        else if (searchby == 5)
        {
            query = "SELECT DISTINCT  TOP  100 lib_id FROM Registration where lib_id Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by lib_id";
        }
        values = ws.Getname(query);
        return values;
    }

    #region bind
    public void lib()
    {
        try
        {
            rbllib.Items.Add("ID for Students");
            rbllib.Items.Add("ID for Staffs");
            rbllib.Items.FindByText("ID for Students").Selected = true;

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
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


                searchclgcode = Convert.ToString(ddlCollege.SelectedValue);
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    public void binddept()
    {
        try
        {
            collegeCode = ddlCollege.SelectedItem.Value.ToString();
            ds.Clear();
            //string strquery = "select distinct degree.degree_code,hrdept_master.dept_name,hrdept_master.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,hrdept_master,course,deptprivilages where course.course_id=degree.course_id  and course.college_code = degree.college_code  and degree.college_code='" + collegeCode + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='30' order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc  ";

            string strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code='" + userCode + "' and hr.dept_code=hp.dept_code order by dept_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "dept_code";
                ddldept.DataBind();
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
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

            //cbl_BatchYearFine.DataSource = dtbatchyr;
            //cbl_BatchYearFine.DataTextField = "Batch_Year";
            //cbl_BatchYearFine.DataValueField = "Batch_Year";
            //cbl_BatchYearFine.DataBind();
        }
    }

    public void binddeg()
    {
        try
        {
            ddldegree.Items.Clear();
            string collegecode = ddlCollege.SelectedItem.Value.ToString();
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

    public void branch()
    {
        try
        {
            ddlbranch.Items.Clear();
            string batch2 = "";
            string degree = "";
            string course_id = ddldegree.SelectedItem.Value;
            string collcode = ddlCollege.SelectedItem.Value.ToString();
            string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code='" + collcode + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "' order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            // string strquery = " SELECT Course_Name+'-'+Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + collcode + "' and c.course_id in(" + course_id + ")  ORDER BY Course_Name,Dept_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");

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

    protected void sem()
    {
        try
        {

            ddlsem.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlBatch.SelectedValue.ToString());
            has.Add("college_code", userCollegeCode);
            ds = da.select_method("bind_sem", has, "sp");
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "libraryidcardgeneration"); }

    }

    protected void section()
    {
        string qry = " select distinct Sections from registration order by Sections desc";
        DataTable secdt = dirAcc.selectDataTable(qry);
        ddlsection.Items.Clear();
        if (secdt.Rows.Count > 0)
        {
            ddlsection.DataSource = secdt;
            ddlsection.DataTextField = "Sections";
            ddlsection.DataValueField = "Sections";
            ddlsection.DataBind();
            ddlsection.Items.Insert(0, "All");
        }
        else
        {

        }
    }

    public void student()
    {
        try
        {
            //rblstudent.Items.Add("All").;
            //rblstudent.Items.Add("Transfer");
            //rblstudent.Items.Add("Regular");
            //rblstudent.Items.Add("Lateral");
            rblstudent.Items.FindByText("All").Selected = true;

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    public void Bindcollege1()
    {
        try
        {
            ddlcollege1.Items.Clear();
            dtCommon.Clear();
            ddlcollege1.Enabled = false;
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
                ddlcollege1.DataSource = dtCommon;
                ddlcollege1.DataTextField = "collname";
                ddlcollege1.DataValueField = "college_code";
                ddlcollege1.DataBind();
                ddlcollege1.SelectedIndex = 0;
                ddlcollege1.Enabled = true;
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }
    #endregion

    #region Index Changed Events
    protected void rbllib_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rbllib.SelectedIndex == 0)
            {
                studentpop.Visible = true;
                staffpop.Visible = false;
                grdStaff.Visible = false;
                div2.Visible = false;
                div3.Visible = false;
                print2.Visible = false;
                print3.Visible = false;
            }
            if (rbllib.SelectedIndex == 1)
            {
                studentpop.Visible = false;
                staffpop.Visible = true;
                Bindcollege1();
                binddept();
                divtable.Visible = false;
                div2.Visible = false;
                div3.Visible = false;
                print2.Visible = false;
                print3.Visible = false;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddeg();
            branch();
            searchclgcode = Convert.ToString(ddlCollege.SelectedValue);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            branch();

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            sem();
            section();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }


    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            section();

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    protected void ddlrollno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlrollno.SelectedIndex == 0)
            {
                txtsearchcontent.Visible = true;
                txtsearchcontent.Enabled = false;
                ddlsearchcontnet.Visible = false;

                searchby = 0;
            }
            if (ddlrollno.SelectedIndex == 1)
            {
                txtsearchcontent.Visible = true;
                txtsearchcontent.Enabled = true;
                ddlsearchcontnet.Visible = false;

                searchby = 1;
            }
            if (ddlrollno.SelectedIndex == 2)
            {
                txtsearchcontent.Visible = true;
                txtsearchcontent.Enabled = true;
                ddlsearchcontnet.Visible = false;

                searchby = 2;
            }
            if (ddlrollno.SelectedIndex == 3)
            {
                txtsearchcontent.Visible = true;
                txtsearchcontent.Enabled = true;
                ddlsearchcontnet.Visible = false;

                searchby = 3;
            }
            if (ddlrollno.SelectedIndex == 4)
            {

                txtsearchcontent.Visible = false;
                ddlsearchcontnet.Visible = true;


            }
            if (ddlrollno.SelectedIndex == 5)
            {
                txtsearchcontent.Visible = true;
                txtsearchcontent.Enabled = true;
                ddlsearchcontnet.Visible = false;

                searchby = 5;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void txtsearchcontent_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void rblstudent_Selected(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    protected void ddlCollege1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtacr_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void cbacr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbacr.Checked)
            {
                txtacr.Enabled = true;
            }
            else
            {
                txtacr.Enabled = false;
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    protected void txtstart_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtsize_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void btngenClick(object sender, EventArgs e)
    {
        try
        {
            SubCmdRegenerateClick();
            btngen.Enabled = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration");
        }
    }

    protected void btmnogenClick(object sender, EventArgs e)
    {
        try
        {
            SubCmdRegenerateClick();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtnoofstud_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void btndefaultClick(object sender, EventArgs e)
    {
        try
        {
            cmddefaultClick();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtstaffacr_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtstaffstart_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void txtstaffsize_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void btnstaffgenClick(object sender, EventArgs e)
    {
        try
        {
            cmdGenClick();

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }
    #endregion

    #region strudent

    protected void grdStudent_onselectedindexchanged(object sender, EventArgs e)
    {
        studentgoClick(sender, e);
    }

    protected void studentgoClick(object sender, EventArgs e)
    {
        try
        {
            DataSet idgenertionstud = new DataSet();

            idgenertionstud = forstudent();
            if (idgenertionstud.Tables.Count > 0 && idgenertionstud.Tables[0].Rows.Count > 0)
            {
                loadspreadstud(ds);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
                grdStudent.Visible = false;
                print2.Visible = false;
            }

            //div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    private DataSet forstudent()
    {
        string branch = string.Empty;
        string sem = string.Empty;
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddldegree.Items.Count > 0)
                degree = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                branch = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlsection.Items.Count > 0)
                sec = Convert.ToString(ddlsection.SelectedValue);
            if (ddlBatch.Items.Count > 0)
                batch = Convert.ToString(ddlBatch.SelectedValue);
            if (ddlrollno.Items.Count > 0)
                roll = Convert.ToString(ddlrollno.SelectedValue);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(batch))
            {
                if (degree != "All" && degree != "")
                {
                    qrydegfilter = " and Degree.Course_Id ='" + degree + "'";
                }
                if (branch != "All" && branch != "")
                {
                    qrybranchfilter = " and Degree.Degree_Code ='" + branch + "'";
                }
                if (sem != "All" && sem != "")
                {
                    qrysemfilter = " and registration.current_semester ='" + sem + "'";
                }
                if (sec != "All" && sec != "")
                {
                    qrysecfilter = " and registration.Sections ='" + sec + "'";
                }
                if (batch != "All" && batch != "")
                {
                    qrybatchfilter = " and registration.batch_year  ='" + batch + "'";
                }


                if (ddlrollno.SelectedIndex == 0)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        selq = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " order by registration.stud_name,Roll_No";
                    }
                    else
                    {
                        selq = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and mode='0' order by registration.stud_name,Roll_No";
                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(selq, "Text");
                }
                else if (ddlrollno.SelectedIndex == 1)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        selroll = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.roll_no like '%" + Convert.ToString(txtsearchcontent.Text) + "' order by registration.stud_name,Roll_No";
                    }
                    else
                    {

                        selroll = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.roll_no like '%" + Convert.ToString(txtsearchcontent.Text) + "' order by registration.stud_name,Roll_No ";
                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(selroll, "Text");
                }
                else if (ddlrollno.SelectedIndex == 2)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        selstud = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.stud_name like '" + Convert.ToString(txtsearchcontent.Text) + "%' order by registration.stud_name,Roll_No";


                        selstud = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year,serialno from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.stud_name like '" + Convert.ToString(txtsearchcontent.Text) + "%'order by registration.stud_name,Roll_No";

                    }
                    else
                    {
                        selstud = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.stud_name like '" + Convert.ToString(txtsearchcontent.Text) + "%' and mode='2' order by registration.stud_name,Roll_No";

                        selstud = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year,serialno from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.stud_name like '" + Convert.ToString(txtsearchcontent.Text) + "%' and  mode='2' order by registration.stud_name,Roll_No";

                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(selstud, "Text");
                }
                else if (ddlrollno.SelectedIndex == 3)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        seladm = "select distinct registration.reg_no,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.reg_no like '" + Convert.ToString(txtsearchcontent.Text) + "%' order by registration.stud_name,Roll_No";


                        seladm = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.reg_no like '" + Convert.ToString(txtsearchcontent.Text) + "%' order by registration.stud_name,Roll_No";

                        seladm = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.roll_admit like '" + Convert.ToString(txtsearchcontent.Text) + "%' order by registration.stud_name,Roll_No";

                    }
                    else
                    {
                        seladm = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno,registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and  applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and  registration.roll_admit like '" + Convert.ToString(txtsearchcontent.Text) + "%' and mode='3' order by registration.stud_name,Roll_No";

                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(seladm, "Text");
                }
                else if (ddlrollno.SelectedIndex == 4)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        if (ddlsearchcontnet.SelectedIndex == -1)
                        {
                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " order by registration.stud_name,Roll_No";
                        }

                        else if (ddlsearchcontnet.SelectedIndex == 0)
                        {

                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and sex=0  order by registration.stud_name,Roll_No ";
                        }
                        else if (ddlsearchcontnet.SelectedIndex == 1)
                        {
                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code   " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and sex=1  order by registration.stud_name,Roll_No ";
                        }
                    }
                    else
                    {
                        if (ddlsearchcontnet.SelectedIndex == -1)
                        {
                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and mode='4' order by registration.stud_name,Roll_No";
                        }

                        else if (ddlsearchcontnet.SelectedIndex == 0)
                        {

                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and sex=0  and mode='4' order by registration.stud_name,Roll_No";
                        }
                        else if (ddlsearchcontnet.SelectedIndex == 1)
                        {
                            selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and sex=1 and mode='4' order by registration.stud_name,Roll_No ";
                        }

                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(selsex, "Text");
                }
                else if (ddlrollno.SelectedIndex == 5)
                {
                    if (rblstudent.SelectedIndex == 0)
                    {
                        selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end as Gender,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.lib_id like '" + Convert.ToString(txtsearchcontent.Text) + "%' order by registration.stud_name,Roll_No";

                    }
                    else
                    {
                        selsex = "select distinct registration.Roll_No,registration.stud_name,case when sex=0 then 'Male' else 'Female' end,registration.lib_id,isnull(registration.lib_id,'') as rlno, registration.batch_year from registration,applyn,Degree,Department where registration.mode=applyn.mode and applyn.app_no=registration.app_no and registration.delflag=0 and Department.Dept_Code=registration.degree_code  and Department.Dept_Code=degree.Degree_Code " + qrydegfilter + qrysemfilter + qrybatchfilter + qrybranchfilter + " and registration.lib_id like '" + Convert.ToString(txtsearchcontent.Text) + "%' and mode='5' order by registration.stud_name,Roll_No ";

                    }
                    dscode.Clear();
                    dscode = d2.select_method_wo_parameter(selsex, "Text");
                }
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
        return dscode;

    }

    private void loadspreadstud(DataSet ds)
    {
        DataSet dscostm = new DataSet();
        try
        {
            int sno = 0;

            grdStudent.Visible = true;
            divtable.Visible = true;
            studid.Columns.Add("SNo", typeof(string));
            studid.Columns.Add("Roll No", typeof(string));
            studid.Columns.Add("Student Name", typeof(string));
            studid.Columns.Add("Gender", typeof(string));
            studid.Columns.Add("Library ID", typeof(string));


          

            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dscode.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    dr = studid.NewRow();
                    admo = Convert.ToString(dscode.Tables[0].Rows[row]["Roll_No"]).Trim();
                    name = Convert.ToString(dscode.Tables[0].Rows[row]["stud_name"]).Trim();
                    gender = Convert.ToString(dscode.Tables[0].Rows[row]["Gender"]).Trim();
                    libid = Convert.ToString(dscode.Tables[0].Rows[row]["lib_id"]).Trim();

                    dr["SNo"] = Convert.ToString(sno);
                    dr["Roll No"] = admo;
                    dr["Student Name"] = name;
                    dr["Gender"] = gender;
                    dr["Library ID"] = libid;
                    studid.Rows.Add(dr);
                }
                grdStudent.DataSource = studid;
                grdStudent.DataBind();
                grdStudent.Visible = true;
              

                for (int l = 0; l < grdStudent.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdStudent.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdStudent.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdStudent.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdStudent.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }

            }
            div2.Visible = true;
            txtnoofstud.Text = Convert.ToString(sno);
            print2.Visible = true;
            print3.Visible = false;
         
        }
        catch
        {
        }
    }

    private void SubCmdRegenerateClick()
    {
        try
        {
            string selx = "";
            string acr = "";
            string strStart = "";
            string strSize = "";
            string gener = "";
            string cond = "";
            string branch = string.Empty;
            string sem = string.Empty;
            string acrochk = "";

            if (ddldegree.Items.Count > 0)
                degree = Convert.ToString(ddldegree.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlBatch.Items.Count > 0)
                batch = Convert.ToString(ddlBatch.SelectedValue);

            selx = "select roll_no,registration.reg_no,roll_admit from registration,Degree where Course_Id='" + degree + "' and current_semester='" + sem + "' and delflag = 0 and batch_year ='" + batch + "' order by roll_no";
            ds.Clear();
            ds = da.select_method_wo_parameter(selx, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (TextBox1.Text != "" && txtstart.Text != "" && txtsize.Text != "")
                {
                    cmddefaultClick();
                    acrochk = Convert.ToString(txtacr.Text);
                    acr = Convert.ToString(TextBox1.Text);
                    strStart = Convert.ToString(txtstart.Text);
                    strSize = Convert.ToString(txtsize.Text);
                    int size = Convert.ToInt32(txtsize.Text);
                    int start = Convert.ToInt32(txtstart.Text);
                    string temp4 = Convert.ToString(start);
                    string temp2 = "";

                    for (int k = 0; k < grdStudent.Rows.Count; k++)
                    {
                        int temp = size - temp4.Length;
                        temp2 = "";
                        for (int l = 0; l <= temp; l++)
                        {
                            temp2 = "0" + temp2;
                        }
                        string finalvalue = acrochk + acr + temp2 + start;
                        grdStudent.Rows[k].Cells[4].Text = finalvalue;
                        start++;
                        temp4 = Convert.ToString(start);
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btnsaveClick(object sender, EventArgs e)
    {
        try
        {
            int qury = 0;
            if (ddldegree.Items.Count > 0)
                degree = Convert.ToString(ddlbranch.SelectedValue);
            for (int t = 1; t < grdStudent.Rows.Count; t++)
            {
                string adno = Convert.ToString(grdStudent.Rows[t].Cells[1].Text);
                string lib = Convert.ToString(grdStudent.Rows[t].Cells[4].Text);
                string name = Convert.ToString(grdStudent.Rows[t].Cells[2].Text);
                string selqry = "update registration set lib_id='" + lib + "' where registration.roll_no='" + adno + "' and degree_code='" + degree + "'";
                qury = d2.update_method_wo_parameter(selqry, "TEXT");
                if (qury != 0)
                {
                    Label3.Text = "Saved Successfully";
                    Label3.Visible = true;
                    Div4.Visible = true;
                    Div1.Visible = true;
                }
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    private void cmddefaultClick()
    {
        btndefault.Enabled = true;
        int m = 0;
        int yr = 0;

        int.TryParse(ddlsem.SelectedValue, out m);
        if (m % 2 == 0)
        {
            yr = (m / 2);
        }
        else
        {
            yr = (m / 2) + 1;
        }
        TextBox1.Text = TextBox1.Text;
    }



    #endregion

    #region Staff

    protected void grdStaff_onselectedindexchanged(object sender, EventArgs e)
    {
        staffgoClick1(sender, e);
    }

    protected void staffgoClick1(object sender, EventArgs e)
    {

        try
        {
            DataSet idgenertionstaff = new DataSet();

            idgenertionstaff = staff();
            if (idgenertionstaff.Tables.Count > 0 && idgenertionstaff.Tables[0].Rows.Count > 0)
            {
                loadspreadstaff(ds);

            }

            else
            {
                //Div1.Visible = true;
                // Div4.Visible = true;
                Label3.Text = "No Record Found!";
                grdStaff.Visible = false;
                print3.Visible = false;
            }



        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    private DataSet staff()
    {

        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddldept.Items.Count > 0)
                dep = Convert.ToString(ddldept.SelectedItem);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dep))
            {
                if (collegeCode == "" && dep == "")
                {
                    selstaff = " select distinct staffmaster.staff_code,staffmaster.staff_name,desig_master.desig_name,staffmaster.lib_id from staffmaster,stafftrans,hrdept_master,desig_master where staffmaster.staff_code=stafftrans.staff_code and stafftrans.latestrec=1 and stafftrans.desig_code=desig_master.desig_code and stafftrans.dept_code = hrdept_master.dept_code and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code = desig_master.collegecode and staffmaster.resign = 0 and settled = 0";
                }
                else
                {
                    selstaff = "select staffmaster.staff_code,staffmaster.staff_name,desig_master.desig_name,staffmaster.lib_id from staffmaster,stafftrans,hrdept_master,desig_master where staffmaster.staff_code=stafftrans.staff_code  and stafftrans.latestrec=1 and stafftrans.desig_code=desig_master.desig_code and stafftrans.dept_code=hrdept_master.dept_code and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code = desig_master.collegecode and staffmaster.resign = 0 and settled = 0  and hrdept_master.dept_name='" + dep + "'";
                }

                dscode1.Clear();
                dscode1 = da.select_method_wo_parameter(selstaff, "Text");
            }

        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
        return dscode1;

    }

    private void loadspreadstaff(DataSet ds)
    {

        try
        {

            grdStaff.Visible = true;
            staffid.Columns.Add("SNo", typeof(string));
            staffid.Columns.Add("StaffCode", typeof(string));
            staffid.Columns.Add("StaffName", typeof(string));
            staffid.Columns.Add("Designation", typeof(string));
            staffid.Columns.Add("Library ID", typeof(string));
            int sno = 0;

          
            if (dscode1.Tables.Count > 0 && dscode1.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dscode1.Tables[0].Rows.Count; row++)
                {

                    sno++;
                    dr1 = staffid.NewRow();
                    staffcode = Convert.ToString(dscode1.Tables[0].Rows[row]["staff_code"]).Trim();
                    staffname = Convert.ToString(dscode1.Tables[0].Rows[row]["staff_name"]).Trim();
                    desgin = Convert.ToString(dscode1.Tables[0].Rows[row]["desig_name"]).Trim();
                    libid1 = Convert.ToString(dscode1.Tables[0].Rows[row]["lib_id"]).Trim();
                    dr1["SNo"] = Convert.ToString(sno);
                    dr1["StaffCode"] = staffcode;
                    dr1["StaffName"] = staffname;
                    dr1["Designation"] = desgin;
                    dr1["Library ID"] = libid1;
                    staffid.Rows.Add(dr1);

                    Session["staff_code"] = staffcode;
                    grdStaff.DataSource = staffid;
                    grdStaff.DataBind();
                    grdStaff.Visible = true;
                    

                }


                for (int l = 0; l < grdStaff.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdStaff.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdStaff.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdStaff.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdStaff.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }

            }
            grdStaff.Visible = true;
            div3.Visible = true;
          
            print2.Visible = false;
            print3.Visible = true;
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

  
    private void cmdGenClick()
    {
        string strCod = "";
        string strnumber = "";

        string strSiz = "";
        if (txtstaffacr.Text != "" && txtstaffstart.Text != "" && txtstaffsize.Text != "")
        {

            for (int s = 0; s < grdStaff.Rows.Count; s++)
            {

                strCod = Convert.ToString(txtstaffacr.Text);
                strnumber = Convert.ToString(txtstaffstart.Text);
                strSiz = Convert.ToString(txtstaffsize.Text);
                int size1 = Convert.ToInt32(txtstaffsize.Text);
                int start1 = Convert.ToInt32(txtstaffstart.Text);
                string temp = Convert.ToString(start1);
                string temp3 = "";

                for (int r = 0; r < grdStaff.Rows.Count; r++)
                {
                    int temp1 = size1 - temp.Length;
                    temp3 = "";
                    for (int l = 0; l <= temp1; l++)
                    {
                        temp3 = "0" + temp3;
                    }
                    string finalvalue1 = strCod + temp3 + start1;
                    grdStaff.Rows[r].Cells[4].Text = finalvalue1;
                    start1++;
                    temp = Convert.ToString(start1);
                    Session["lib_id"] = finalvalue1;
                }
            }

        }
        else
        {
            string errMsg = "";
            if (txtstaffacr.Text == "")
                errMsg = "Please enter Acronym";
            if (txtstaffstart.Text == "")
            {
                if (errMsg == "")
                    errMsg = "Please enter Start value";
                else
                    errMsg = errMsg + " and Start value";
            }
            if (txtstaffsize.Text == "")
            {
                if (errMsg == "")
                    errMsg = "Please enter Size";
                else
                    errMsg = errMsg + " and size";
            }
            lblalerterr.Text = errMsg;
            lblalerterr.Visible = true;
            alertpopwindow.Visible = true;
            btnerrclose.Visible = true;

        }
    }

    protected void btnsave1Click(object sender, EventArgs e)
    {
        int query = 0;
        try
        {
            for (int m = 0; m < grdStaff.Rows.Count; m++)
            {
                string code = Convert.ToString(grdStaff.Rows[m].Cells[1].Text);
                string lib = Convert.ToString(grdStaff.Rows[m].Cells[4].Text);
                string sqry = "update staffmaster set lib_id='" + lib + "' where staff_code='" + code + "'";
                query = d2.update_method_wo_parameter(sqry, "TEXT");
                if (query != 0)
                {
                    Label3.Text = "Updated Successfully";
                    Label3.Visible = true;
                    Div4.Visible = true;
                    Div1.Visible = true;
                }
            }




        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }


    }

    #endregion

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdStudent, reportname);
                lblvalidation3.Visible = false;
            }
            else
            {
                lblvalidation3.Text = "Please Enter Your  Report Name";
                lblvalidation3.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Library ID Generation";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "LibraryIdcardgeneration.aspx";
            string ss = Session["usercode"].ToString();

            Printcontrolhed2.loadspreaddetails(grdStudent, pagename, degreedetails, 0, ss,"1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void getPrintSettings2()
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
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion

    #region Print
    protected void btnExcel2_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdStaff, reportname);
                Label1.Visible = false;
            }
            else
            {
                Label1.Text = "Please Enter Your  Report Name";
                Label1.Visible = true;
                TextBox2.Focus();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }

    }

    public void btnprintmaster2_Click2(object sender, EventArgs e)
    {
        try
        {
            Label1.Text = "";
            TextBox2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Library ID Generation";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "LibraryIdcardgeneration.aspx";
            string ss = Session["usercode"].ToString();
            NEWPrintMater1.loadspreaddetails(grdStaff, pagename, degreedetails, 0, ss, "1");
            NEWPrintMater1.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void getPrintSettings3()
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
                    Label2.Visible = true;
                    TextBox2.Visible = true;
                    ImageButton1.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    ImageButton2.Visible = true;
                }
                if (printset == "0")
                {
                    Label2.Visible = true;
                    TextBox2.Visible = true;
                    ImageButton1.Visible = true;
                    ImageButton2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

  

    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
            Div4.Visible = false;
            Div1.Visible = false;
            Label3.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        Div1.Visible = false;
        Div4.Visible = false;
    }
    #endregion
}