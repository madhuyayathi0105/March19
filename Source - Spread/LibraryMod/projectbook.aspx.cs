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


public partial class LibraryMod_projectbook : System.Web.UI.Page
{
    static string SearchField = string.Empty;
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
   
    bool flag_true = false;
    Boolean Cellclick = false;
    Boolean Cellclick1 = false;
    Boolean Cellclick4 = false;
    static string Cellclick2 = string.Empty;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable roll = new Hashtable();

    static string rolls = string.Empty;
    static string names = string.Empty;
    static string depts = string.Empty;
    static string cellclick3 = string.Empty;
    static int searchby = 0;
    static string searchclgcode = string.Empty;
    static string searchlibcode = string.Empty;

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
                Binddeopt();
                getLibPrivil();
                dues();
                bindbatch();
                binddegree();
                bindbranch();
                Bindsubject();
                Bindlanguage();
                dept();
                bindsec();
                bindsem();
                Autoaccno();
                // rptprint.Visible = false;
            }
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();


        if (searchby == 1)
        {

            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 ProBook_Accno FROM Project_Book where ProBook_Accno Like '" + prefixText + "%' AND Lib_code ='" + searchlibcode + "' order by ProBook_Accno";
            else
                query = "SELECT DISTINCT  TOP  100 ProBook_Accno FROM Project_Book where ProBook_Accno Like '" + prefixText + "%'  order by ProBook_Accno";
        }
        else if (searchby == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Roll_No FROM Project_Book where Roll_No Like '" + prefixText + "%' AND Lib_code ='" + searchlibcode + "' order by Roll_No";
            else
                query = "SELECT DISTINCT  TOP  100 Roll_No FROM Project_Book where Roll_No Like '" + prefixText + "%'  order by Roll_No";
        }
        else if (searchby == 3)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Title FROM Project_Book where Title Like '" + prefixText + "%' AND Lib_code ='" + searchlibcode + "' order by Title";
            else
                query = "SELECT DISTINCT  TOP  100 Title FROM Project_Book where Title Like '" + prefixText + "%'  order by Title";
        }
        else if (searchby == 4)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Name FROM Project_Book where Name Like '" + prefixText + "%' AND Lib_code ='" + searchlibcode + "' order by Name";
            else
                query = "SELECT DISTINCT  TOP  100 Name FROM Project_Book where Name Like '" + prefixText + "%'  order by Name";
        }
        values = ws.Getname(query);
        return values;
    }

    #region BindMethod

    public void Bindcollege()
    {
        try
        {
            ddlcollege.Items.Clear();
            dtCommon.Clear();
            ddlcollege.Enabled = false;
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
            // d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); 
        }
        {
        }
    }

    public void Bindlib(string Libcollection)
    {
        try
        {
            collegeCode = ddlcollege.SelectedItem.Value.ToString();
            string hed = " Select Lib_Code,Lib_Name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) FROM Library   " + Libcollection + " AND college_code='" + collegeCode + " ' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds2;
                ddllibrary.DataTextField = "Lib_Name";
                ddllibrary.DataValueField = "Lib_Code";
                ddllibrary.DataBind();
                ddlprojectlibrary.DataSource = ds2;
                ddlprojectlibrary.DataTextField = "Lib_Name";
                ddlprojectlibrary.DataValueField = "Lib_Code";
                ddlprojectlibrary.DataBind();

                searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
            }

        }
        catch
        {
        }
    }

    public void Binddeopt()
    {
        try
        {
            //string typ = string.Empty;
            //if (ddlcollege.Items.Count > 0)
            //{
            //    for (int i = 0; i < ddlcollege.Items.Count - 1; i++)
            //    {
            //        if (Convert.ToString(ddlcollege.SelectedItem) == "All")
            //        {
            //            if (typ == "")
            //            {
            //                typ = "" + ddlcollege.Items[i + 1].Value + "";
            //            }
            //            else
            //            {
            //                typ = typ + "'" + "," + "'" + ddlcollege.Items[i + 1].Value + "";
            //            }
            //        }
            //        else
            //            typ = ddlcollege.SelectedValue;
            //    }
            //}
            collegeCode = ddlcollege.SelectedItem.Value.ToString();
            string hed = " SELECT (C.Course_Name +'-'+ D.Dept_Name)as Dept_Name  FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code  AND G.College_Code='" + collegeCode + "'  ORDER BY Dept_Name ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds2;
                ddldept.DataTextField = "Dept_Name";
                ddldept.DataValueField = "Dept_Name";
                ddldept.DataBind();
                //ddlbudgetdept.DataSource = ds2;
                //ddlbudgetdept.DataTextField = "Dept_Name";
                //ddlbudgetdept.DataValueField = "Dept_Name";
                //ddlbudgetdept.DataBind();
                //ddlbudgetdept.Items.Insert(0, "");
            }

        }
        catch
        {
        }
    }

    public void dues()
    {
        try
        {
            ddlstatus.Items.Add("Issued");
            ddlstatus.Items.Add("Binding");
            ddlstatus.Items.Add("Lost");
            ddlstatus.Items.Add("Available");
            ddlstatus.Items.Add("condemn");
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatchyear.Items.Clear();

            ds = dirAcc.selectDataSet("select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>'' and college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "' order by batch_year desc");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlbatchyear.DataSource = ds;
                    ddlbatchyear.DataTextField = "batch_year";
                    ddlbatchyear.DataValueField = "batch_year";
                    ddlbatchyear.DataBind();
                    //ddlbatch.SelectedIndex = 0;


                }
            }
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
            collegecode = ddlcollege.SelectedItem.Value.ToString();
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
        catch (Exception ex)
        {

        }
    }

    public void bindbranch()
    {
        try
        {
            has.Clear();
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            //  ds = da.select_method("bind_branch", has, "sp");

            string bat = "SELECT  D.Dept_Name as Dept_Name, D.Dept_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code  AND G.College_Code=" + collegecode + " and c.course_id='" + Convert.ToString(ddldegree.SelectedValue) + "' ORDER BY Dept_Name ";
            ds = da.select_method_wo_parameter(bat, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlbranch.DataSource = ds;
                    ddlbranch.DataTextField = "dept_name";
                    ddlbranch.DataValueField = "Dept_Code";
                    ddlbranch.DataBind();
                    ddlbranch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void Bindsubject()
    {
        try
        {

            string hed = "SELECT DISTINCT ISNULL(Subject,'') Subject  FROM Project_Book WHERE Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "' ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlsubj.DataSource = ds2;
                ddlsubj.DataTextField = "Subject";
                ddlsubj.DataValueField = "Subject";
                ddlsubj.DataBind();
            }

        }
        catch
        {
        }
    }

    public void Bindlanguage()
    {
        try
        {

            string hed = "SELECT DISTINCT ISNULL(Language,'') Language  FROM Project_Book WHERE Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "'";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlmedium.DataSource = ds2;
                ddlmedium.DataTextField = "Language";
                ddlmedium.DataValueField = "Language";
                ddlmedium.DataBind();
            }

        }
        catch
        {
        }
    }

    public void dept()
    {
        try
        {

            string hed = "SELECT (C.Course_Name +'-'+ D.Dept_Name) as Dept_Name FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code = '" + Convert.ToString(ddlcollege.SelectedValue) + "' ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds2;
                ddldepartment.DataTextField = "Dept_Name";
                ddldepartment.DataValueField = "Dept_Name";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, "");
            }

        }
        catch
        {
        }
    }

    public void bindsem()
    {
        try
        {

            string BatchYear = ddlbatchyear.SelectedItem.Value.ToString();
            string qry = "select distinct Current_Semester,Batch_Year from registration where Batch_Year='" + BatchYear + "' order by Current_Semester desc";
            DataTable semdt = dirAcc.selectDataTable(qry);
            ddlsemester.Items.Clear();
            if (semdt.Rows.Count > 0)
            {
                ddlsemester.DataSource = semdt;
                ddlsemester.DataTextField = "Current_Semester";
                ddlsemester.DataValueField = "Current_Semester";
                ddlsemester.DataBind();


            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsec()
    {
        try
        {

            ddlsection.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlbatchyear.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue);
            ds = da.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlsection.DataSource = ds;
                ddlsection.DataTextField = "sections";
                ddlsection.DataValueField = "sections";
                ddlsection.DataBind();
                ddlsection.Enabled = true;
            }
            else
            {
                ddlsection.Enabled = false;
            }
            ddlsection.Items.Add("All");
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

        getLibPrivil();
        Binddeopt();
    }

    #region ButtonStaffCode

    public void Bindcolle()
    {
        try
        {
            ddlcolle.Items.Clear();
            dtCommon.Clear();
            ddlcolle.Enabled = false;
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
                ddlcolle.DataSource = dtCommon;
                ddlcolle.DataTextField = "collname";
                ddlcolle.DataValueField = "college_code";
                ddlcolle.DataBind();
                ddlcolle.SelectedIndex = 0;
                ddlcolle.Enabled = true;
                ddlcolle.Items.Insert(0, "All");



            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); 
        }
        {
        }
    }

    public void binddept()
    {
        try
        {
            collegeCode = ddlcolle.SelectedItem.Value.ToString();
            ds.Clear();


            string strquery = "SELECT DISTINCT hr.dept_code,dept_name from hrdept_master hr  where college_code='" + collegeCode + "' order by dept_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldep.DataSource = ds;
                ddldep.DataTextField = "dept_name";
                ddldep.DataValueField = "dept_code";
                ddldep.DataBind();

            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "LibraryIdcardgeneration"); }
    }

    protected void ddlcolle_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void ddldep_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void btnstaff_Click(object sender, EventArgs e)
    {

        divstafflist.Visible = true;
        divstafflist1.Visible = true;
        divPopAlertContent.Visible = false;
        divPopAlertprojectbook.Visible = false;
        Bindcolle();
        binddept();
        grdStaff.Visible = true;
        divpoprollnumber.Visible = false;
    }

    private void staffsearchgo()
    {
        try
        {
            DataTable prostaff = new DataTable();
            DataRow drstaff;
            collegeCode = Convert.ToString(Session["collegecode"]);
            string dep = Convert.ToString(ddldep.SelectedItem);
            string stafqry = string.Empty;
            if (ddlsearstaff.SelectedIndex == 0)
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + collegeCode + "' and hrdept_master.dept_name='" + dep + "'";
            }
            else if (ddlsearstaff.SelectedIndex == 1)
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + collegeCode + "' and staffmaster.staff_name='" + txtsearstaff.Text + "' and hrdept_master.dept_name='" + dep + "' ";
            }
            else
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + collegeCode + "' and staffmaster.staff_code='" + txtsearstaff.Text + "' and hrdept_master.dept_name='" + dep + "'";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(stafqry, "text");
            int sno = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                prostaff.Columns.Add("Staff No", typeof(string));
                prostaff.Columns.Add("Staff Name", typeof(string));
                prostaff.Columns.Add("Department", typeof(string));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drstaff = prostaff.NewRow();
                    string staffno = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    string staffname = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                    string deptname = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                    drstaff["Staff No"] = staffno;
                    drstaff["Staff Name"] = staffname;
                    drstaff["Department"] = deptname;
                    prostaff.Rows.Add(drstaff);
                }
                grdStaff.DataSource = prostaff;
                grdStaff.DataBind();
                grdStaff.Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                div2.Visible = true;
                grdStaff.Visible = true;
                btnex.Visible = true;
            }
            else
            {
                div2.Visible = false;
                grdStaff.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void btnreqstaff_click(object sender, EventArgs e)
    {
        try
        {
            divstafflist.Visible = true;
            divstafflist1.Visible = true;
            staffsearchgo();

        }
        catch
        {
        }
    }

    protected void ddlsearstaff_selectedindex_changed(object sender, EventArgs e)
    {
        if (ddlsearstaff.SelectedIndex == 0)
        {
            txtsearstaff.Visible = false;
        }
        else
        {
            txtsearstaff.Visible = true;
        }

    }

    protected void btnseargo_click(object sender, EventArgs e)
    {
        try
        {
            string staffqry = string.Empty;
            staffsearchgo();
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
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStaff_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            // if (Cellclick1 == true)
            {
                //btnsavebud.Enabled = false;


                string sql = string.Empty;
                var grid = (GridView)sender;
                GridViewRow selectedRow = grid.SelectedRow;
                int rowIndex = grid.SelectedIndex;
                int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

                string staffcod = grdStaff.Rows[rowIndex].Cells[1].Text;
                string guide = grdStaff.Rows[rowIndex].Cells[2].Text;
                // txtreqBystaff.Text = staffcod;
                divstafflist.Visible = false;
                divstafflist1.Visible = false;
                divPopAlertContent.Visible = true;
                divPopAlertprojectbook.Visible = true;

                Txtguidename.Text = guide;



            }
        }
        catch
        {
        }

    }

    protected void btn_ex_Click(object sender, EventArgs e)
    {
        divstafflist.Visible = false;
        divstafflist1.Visible = false;
        divPopAlertContent.Visible = true;
        divPopAlertprojectbook.Visible = true;
    }

    #endregion

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        Binddeopt();

        searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ddlsearchby.SelectedItem) == "Department")
            {
                Binddeopt();
                SearchField = "D.Degree";
                ddldept.Visible = true;
                txt_searchby.Visible = false;
            }
            if (Convert.ToString(ddlsearchby.SelectedItem) == "Name")
            {
                SearchField = "D.Stud_Name";
                ddldept.Visible = false;
                txt_searchby.Visible = true;

                searchby = 4;
            }
            if (Convert.ToString(ddlsearchby.SelectedItem) == "All")
            {
                SearchField = "";
                ddldept.Visible = false;
                txt_searchby.Visible = false;
            }
            if (Convert.ToString(ddlsearchby.SelectedValue) == "1")
            {
                SearchField = "M.ProBook_AccNo";
                ddldept.Visible = false;
                txt_searchby.Visible = true;
                searchby = 1;
            }
            if (Convert.ToString(ddlsearchby.SelectedItem) == "Title")
            {
                SearchField = "Title";
                ddldept.Visible = false;
                txt_searchby.Visible = true;

                searchby = 3;
            }
            if (Convert.ToString(ddlsearchby.SelectedItem) == "Roll No")
            {
                SearchField = "D.Roll_No";
                ddldept.Visible = false;
                txt_searchby.Visible = true;

                searchby = 2;
            }
            if (Convert.ToString(ddlsearchby.SelectedItem) == "Guide Name")
            {
                SearchField = "Guide_Name";
                ddldept.Visible = true;
                txt_searchby.Visible = false;
            }

        }
        catch
        {
        }
    }

    #region GoEvents

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            sql = " SELECT DISTINCT  M.ProBook_AccNo,Title,Guide_Name,ISNULL(SubmitDate,'') SubmitDate,DATENAME(month,SubmitDate) as month,DATEPART(year,SubmitDate)as Year,Issue_Flag,LEN(M.ProBook_AccNo),name as Stud_Name,m.roll_no ";
            sql = sql + " FROM Project_BookDetails D, Project_Book M ";
            sql = sql + " INNER JOIN Library L ON L.Lib_Code = M.Lib_Code ";
            sql = sql + " Where 1 = 1 and M.ProBook_AccNo = D.ProBook_AccNo ";
            sql = sql + " AND M.Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "'";
            if (Convert.ToString(ddlsearchby.SelectedItem) != "All")
            {
                if (Convert.ToString(ddlsearchby.SelectedItem) != "Department" && Convert.ToString(ddlsearchby.SelectedItem) != "Guide Name")
                {
                    sql = sql + " AND " + SearchField + " Like '%" + txt_searchby.Text + "%'";
                }
                else
                {
                    sql = sql + " AND " + SearchField + " Like '%" + Convert.ToString(ddldept.SelectedItem) + "%'";
                }
            }
            sql = sql + "ORDER BY LEN(M.ProBook_AccNo),M.ProBook_AccNo ";
            DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
            int sno = 0;
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                DataTable dtProBook = new DataTable();
                DataRow drow;
                dtProBook.Columns.Add("S.No", typeof(string));
                dtProBook.Columns.Add("Access No", typeof(string));
                dtProBook.Columns.Add("Title", typeof(string));
                dtProBook.Columns.Add("Roll No", typeof(string));
                dtProBook.Columns.Add("Name", typeof(string));
                dtProBook.Columns.Add("Department", typeof(string));
                dtProBook.Columns.Add("Month&Year", typeof(string));
                dtProBook.Columns.Add("Status", typeof(string));
                dtProBook.Columns.Add("Guide Name", typeof(string));

              
                for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    string proAccNo = Convert.ToString(bookallo.Tables[0].Rows[i]["ProBook_AccNo"]);
                    string submitMonth = Convert.ToString(bookallo.Tables[0].Rows[i]["month"]);
                    string submitYear = Convert.ToString(bookallo.Tables[0].Rows[i]["year"]);
                    sql = " SELECT D.Roll_No,Stud_Name,Degree FROM Project_BookDetails D,Project_Book M WHERE M.ProBook_AccNo = D.ProBook_AccNo  AND D.ProBook_AccNo ='" + proAccNo + "' AND M.Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "'";

                    DataSet bookallo2 = d2.select_method_wo_parameter(sql, "Text");
                    if (bookallo2.Tables.Count > 0 && bookallo2.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < bookallo2.Tables[0].Rows.Count; j++)
                        {
                            drow = dtProBook.NewRow();
                            drow["S.No"] = sno;
                            drow["Access No"] = proAccNo;
                            drow["Title"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Title"]);
                            drow["Roll No"] = Convert.ToString(bookallo2.Tables[0].Rows[j]["Roll_No"]);
                            drow["Name"] = Convert.ToString(bookallo2.Tables[0].Rows[j]["Stud_Name"]);
                            drow["Department"] = Convert.ToString(bookallo2.Tables[0].Rows[j]["Degree"]);
                            drow["Month&Year"] = Convert.ToString(submitMonth) + '/' + submitYear;
                            drow["Status"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Issue_Flag"]);
                            drow["Guide Name"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Guide_Name"]);
                            dtProBook.Rows.Add(drow);
                        }
                    }
                }
                grdProBook.DataSource = dtProBook;
                grdProBook.DataBind();
               
                grdProBook.Visible = true;
                btn_Excel.Visible = true;
                btn_printmaster.Visible = true;
            }
            else
            {
                grdProBook.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No record found";
            }
            MergeRows(grdProBook);

        }
        catch
        {
        }
    }

   

    public static void MergeRows(GridView grdProBook)
    {
        string sNo = grdProBook.HeaderRow.Cells[0].Text;
        string AccNo = grdProBook.HeaderRow.Cells[1].Text;
        string title = grdProBook.HeaderRow.Cells[2].Text;
        string dept = grdProBook.HeaderRow.Cells[5].Text;
        string month = grdProBook.HeaderRow.Cells[6].Text;
        string status = grdProBook.HeaderRow.Cells[7].Text;
        string guidename = grdProBook.HeaderRow.Cells[8].Text;

        for (int rowIndex = grdProBook.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = grdProBook.Rows[rowIndex];
            GridViewRow previousRow = grdProBook.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (grdProBook.HeaderRow.Cells[i].Text.ToLower() == sNo.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == AccNo.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == title.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == dept.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == month.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == status.ToLower() || grdProBook.HeaderRow.Cells[i].Text.ToLower() == guidename.ToLower())
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

    protected void grdProBook_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdProBook_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string sql = string.Empty;
            Btnup.Visible = true;
            Btndele.Visible = true;
            Btnsave.Visible = false;
            if (Convert.ToString(rowIndex) != "-1" && Convert.ToString(rowIndex) != "")
            {
                divPopAlertprojectbook.Visible = true;
                divPopAlertContent.Visible = true;
                string var_AccNo = grdProBook.Rows[rowIndex].Cells[1].Text; //Convert.ToString(spreadprojectbook.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                sql = " SELECT M.ProBook_AccNo,ISNULL(Access_Date,'')  Access_Date,Title,ISNULL(D.Roll_No,'') Roll_No,ISNULL(D.Stud_Name,'') Stud_Name,Guide_Name,Issue_Flag,ISNULL(Remarks,'') Remarks,M.Lib_Code,Tot_Stud,ISNULL(SubmitDate,'') SubmitDate,ISNULL(Subject,'') Subject,ISNULL(Language,'') Language,D.Degree FROM Project_Book M,Library L,Project_BookDetails D where  L.Lib_Code = M.Lib_Code and  M.ProBook_AccNo = D.ProBook_AccNo AND M.Lib_Code = D.Lib_Code and 1 = 1 AND M.ProBook_AccNo ='" + var_AccNo + "'";
                DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    DataTable dtViewStu = new DataTable();
                    DataRow drow;
                    dtViewStu.Columns.Add("Roll No", typeof(string));
                    dtViewStu.Columns.Add("Name", typeof(string));
                    dtViewStu.Columns.Add("Department", typeof(string));
                    for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                    {
                        txtaccno.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["ProBook_AccNo"]);
                        ddlprojectlibrary.Items.Insert(0, Convert.ToString(ddllibrary.SelectedItem));
                        ddlprojectlibrary.SelectedValue.Insert(0, Convert.ToString(ddllibrary.SelectedValue));
                        string accDate = Convert.ToString(bookallo.Tables[0].Rows[0]["Access_Date"]);
                        string[] split = accDate.Split('/');
                        accDate = split[1] + "/" + split[0] + "/" + split[2];
                        txt_accessdate2.Text = accDate.Split(' ')[0];
                        Txttltle.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Title"]);
                        Txtguidename.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Guide_Name"]);
                        ddlstatus.Items.Insert(0, Convert.ToString(bookallo.Tables[0].Rows[0]["Remarks"]));
                        Txtrollnumber.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Roll_No"]);
                        txtname.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Stud_Name"]);
                        ddldepartment.SelectedItem.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Degree"]);

                        drow = dtViewStu.NewRow();
                        drow["Roll No"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Roll_No"]);
                        drow["Name"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Stud_Name"]);
                        drow["Department"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Degree"]);
                        dtViewStu.Rows.Add(drow);
                    }
                    GrdViewStu.DataSource = dtViewStu;
                    GrdViewStu.DataBind();
                    GrdViewStu.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    public void addclear()
    {
        try
        {
            txtaccno.Text = "";
            ddlprojectlibrary.Items.Insert(0, "");
            ddlprojectlibrary.SelectedValue.Insert(0, "");
            getLibPrivil();
            txt_accessdate2.Text = "";
            Txttltle.Text = "";
            Txtguidename.Text = "";
            ddlstatus.Items.Insert(0, ""); ;
            Txtrollnumber.Text = "";
            txtname.Text = "";
            ddldepartment.SelectedValue.Insert(0, "");
            dept();

        }
        catch
        {
        }
    }

    public void Autoaccno()
    {
        try
        {
            string sql = string.Empty;
            sql = "SELECT ISNULL(ProjAutoNo,0) Project_AutoNo,ISNULL(Proj_Acr,'') Proj_Acr,ISNULL(Proj_StNo,1) Proj_StNo FROM Library Where Lib_Code ='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
            DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
            string codeno = "";
            string codeno1 = "";
            DataSet dsbook = new DataSet();
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                string book = Convert.ToString(bookallo.Tables[0].Rows[0]["Project_AutoNo"]);
                if (book.ToLower() == "true")
                {
                    for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                    {
                        string itemacronym = Convert.ToString(bookallo.Tables[0].Rows[0]["Proj_Acr"]);
                        sql = "SELECT distinct top (1) probook_accno FROM Project_Book WHERE Lib_Code ='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "' and probook_accno like '" + Convert.ToString(itemacronym) + "[0-9]%' ORDER BY (probook_accno) desc";
                        string newitemcode = "";
                        dsbook = d2.select_method_wo_parameter(sql, "Text");
                        if (dsbook.Tables.Count > 0 && dsbook.Tables[0].Rows.Count > 0)
                        {
                            codeno = Convert.ToString(dsbook.Tables[0].Rows[i]["probook_accno"]);

                            string str = "";
                            for (int k = 0; k < codeno.Length; k++)
                            {
                                string a = Convert.ToString(codeno.ElementAt<char>(k));
                                if (a.All(char.IsNumber))
                                {
                                    str = str + a;
                                }
                            }
                            int jj = Convert.ToInt32(str) + 1;
                            //codeno1 = Convert.ToString(bookallo.Tables[0].Rows[0]["Proj_Acr"]) + jj;
                            txtaccno.Text = Convert.ToString(jj);
                            txtaccno.Enabled = false;
                        }
                        else
                        {
                            string sql1 = d2.GetFunction("SELECT ISNULL(Proj_Acr,'')  +''+ISNULL(Proj_StNo,1) Proj_StNo FROM Library Where Lib_Code ='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'");
                            newitemcode = sql1;
                            txtaccno.Text = newitemcode;
                            txtaccno.Enabled = false;
                        }
                    }
                }
                else
                {
                    txtaccno.Enabled = true;
                }
            }
            else
            {
                txtaccno.Enabled = true;
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        divPopAlertprojectbook.Visible = true;
        divPopAlertContent.Visible = true;
        addclear();
        Autoaccno();
        divPopAlertContent.Visible = true;
        Cellclick2 = "true";
        btnadd.Visible = true;
        Btnup.Visible = false;
        Btndele.Visible = false;
        Bindsubject();
        Bindlanguage();

    }

    protected void ddlprojectlibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnrollno_Click(object sender, EventArgs e)
    {
        divpoprollnumber.Visible = true;
        bindbatch();
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Text = string.Empty;
            lblAlertMsg.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlertContent.Visible = true;
            divPopAlertprojectbook.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnaddproject_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlertContent.Visible = false;
            divPopAlertprojectbook.Visible = false;
            Cellclick4 = true;
            int sno = 0;
            DataTable dtViewStu = new DataTable();
            DataRow drow;
            dtViewStu.Columns.Add("Roll No", typeof(string));
            dtViewStu.Columns.Add("Name", typeof(string));
            dtViewStu.Columns.Add("Department", typeof(string));
            if (Txtrollnumber.Text != "")
            {
                if (Label6.Text != "")
                {
                    if (Cellclick2 == "false")
                    {
                        Cellclick2 = "true";
                        lnkviwestudent_Click(sender, e);
                    }
                    else if (Cellclick2 == "true" && cellclick3 == "true")
                    {
                        cellclick3 = "false";
                    }
                    else
                    {
                        Label6.Text = Label6.Text + ',' + Txtrollnumber.Text;
                        Label7.Text = Label7.Text + ',' + txtname.Text;
                        Label8.Text = Label8.Text + ',' + ddldepartment.SelectedItem.Text;
                    }
                    // Cellclick4 = false;  
                    rolls = Label6.Text;
                    names = Label7.Text;
                    depts = Label8.Text;
                    string[] rollmess_attn = rolls.Split(',');
                    string[] appmess_attn = depts.Split(',');
                    string[] name = names.Split(',');
                    if (rolls != "")
                    {
                        for (int i = 0; i < rollmess_attn.Length; i++)
                        {
                            drow = dtViewStu.NewRow();
                            drow["Roll No"] = rollmess_attn[i];
                            drow["Name"] = name[i];
                            drow["Department"] = appmess_attn[i];
                            dtViewStu.Rows.Add(drow);
                            divPoplinlkprojectbook.Visible = true;
                            div1.Visible = true;
                            sno++;
                        }
                        GrdViewStu.DataSource = dtViewStu;
                        GrdViewStu.DataBind();
                        GrdViewStu.Visible = true;
                    }
                }
                else
                {
                    divPoplinlkprojectbook.Visible = true;
                    div1.Visible = true;
                    lnkviwestudent_Click(sender, e);
                    if (Label6.Text == "")
                    {
                        Label6.Text = Txtrollnumber.Text;
                        Label7.Text = txtname.Text;
                        Label8.Text = ddldepartment.SelectedItem.Text;
                    }
                    else
                    {
                        Label6.Text = Label6.Text + ',' + Txtrollnumber.Text;
                        Label7.Text = Label7.Text + ',' + txtname.Text;
                        Label8.Text = Label8.Text + ',' + ddldepartment.SelectedItem.Text;
                    }
                    rolls = Label6.Text;
                    names = Label7.Text;
                    depts = Label8.Text;

                    string[] rollmess_attn = rolls.Split(',');
                    string[] appmess_attn = depts.Split(',');
                    string[] name = names.Split(',');
                    if (rolls != "")
                    {
                        for (int i = 0; i < rollmess_attn.Length; i++)
                        {
                            drow = dtViewStu.NewRow();
                            drow["Roll No"] = rollmess_attn[i];
                            drow["Name"] = name[i];
                            drow["Department"] = appmess_attn[i];
                            dtViewStu.Rows.Add(drow);
                            divPoplinlkprojectbook.Visible = true;
                            div1.Visible = true;
                            sno++;
                        }
                        GrdViewStu.DataSource = dtViewStu;
                        GrdViewStu.DataBind();
                        GrdViewStu.Visible = true;

                        Label6.Text = Txtrollnumber.Text;
                        Label7.Text = txtname.Text;
                        Label8.Text = ddldepartment.SelectedItem.Text;
                    }
                }
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Text = "Enter Roll No.";
            }
        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            int IntTotStud = 0;
            string[] rollmess_attn = Label6.Text.Split(',');
            string[] appmess_attn = Label8.Text.Split(',');
            string[] name = Label7.Text.Split(',');
            int lenth = rollmess_attn.Length;
            IntTotStud = lenth;
            string AccessDate = txt_accessdate2.Text;
            string[] dtAccessDate = AccessDate.Split('/');
            if (dtAccessDate.Length == 3)
                AccessDate = dtAccessDate[1].ToString() + "/" + dtAccessDate[0].ToString() + "/" + dtAccessDate[2].ToString();

            string submitDate = TextBox2.Text;
            string[] dtsubmitDate = submitDate.Split('/');
            if (dtsubmitDate.Length == 3)
                submitDate = dtsubmitDate[1].ToString() + "/" + dtsubmitDate[0].ToString() + "/" + dtsubmitDate[2].ToString();

            if (txtaccno.Text != "" && Txttltle.Text != "" && rollmess_attn.Length > 0)
            {
                sql = "SELECT * FROM Project_Book WHERE ProBook_Accno='" + txtaccno.Text + "' AND Lib_Code = '" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
                DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Access No Already Exists";
                    divPopAlertContent.Visible = false;
                    divPopAlertprojectbook.Visible = false;
                    //divPopAlertrollnumber.Visible = false;
                    divpoprollnumber.Visible = false;
                    alertpopwindow.Visible = false;
                }
                else
                {
                    if (Chk_MultipleCopies.Checked == false)
                    {
                        sql = "INSERT INTO Project_Book(ProBook_Accno,Title,Roll_No,Name,Degree_code,Guide_name,Remarks,Lib_code,Issue_Flag,Access_Date,Tot_Stud,SubmitDate,Subject,Language) ";
                        sql = sql + " values ('" + txtaccno.Text + "','" + Txttltle.Text + "','" + rollmess_attn[0] + "','" + name[0] + "','" + appmess_attn[0] + "','" + Txtguidename.Text + "','" + txtremark.Text + "','" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "','" + Convert.ToString(ddlstatus.SelectedItem) + "','" + AccessDate + "'," + IntTotStud + ",'" + submitDate + "','" + Convert.ToString(ddlsubj.SelectedValue) + "','" + Convert.ToString(ddlmedium.SelectedValue) + "')";
                        int ups = d2.update_method_wo_parameter(sql, "Text");
                        //string[] rollmess_attn = Label6.Text.Split(',');
                        //string[] appmess_attn = Label8.Text.Split(',');
                        //string[] name = Label7.Text.Split(',');
                        int ups1 = 0;

                        for (int i = 0; i < rollmess_attn.Length; i++)
                        {
                            sql = "INSERT INTO Project_BookDetails(Probook_Accno,Roll_No,Stud_Name,Degree,Lib_Code) ";
                            sql = sql + " VALUES ('" + txtaccno.Text + "','" + rollmess_attn[i] + "','" + name[i] + "','" + appmess_attn[0] + "','" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "')";
                            ups1 = d2.update_method_wo_parameter(sql, "Text");
                        }
                        if (ups1 != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                            divPopAlertContent.Visible = false;
                            divPopAlertprojectbook.Visible = false;
                            divpoprollnumber.Visible = false;
                            //alertpopwindow.Visible = false;
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            divPopAlertContent.Visible = false;
                            divPopAlertprojectbook.Visible = false;
                            divpoprollnumber.Visible = false;
                            //alertpopwindow.Visible = false;
                        }
                    }
                    else
                    {
                        if (Txt_MultipleCopies.Text != "")
                        {
                            sql = "INSERT INTO Project_Book(ProBook_Accno,Title,Roll_No,Name,Degree_code,Guide_name,Remarks,Lib_code,Issue_Flag,Access_Date,Tot_Stud,SubmitDate,Subject,Language) ";
                            sql = sql + " values ('" + txtaccno.Text + "','" + Txttltle.Text + "','" + rollmess_attn[0] + "','" + name[0] + "','" + appmess_attn[0] + "','" + Txtguidename.Text + "','" + txtremark.Text + "','" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "','" + Convert.ToString(ddlstatus.SelectedItem) + "','" + AccessDate + "'," + IntTotStud + ",'" + submitDate + "','" + Convert.ToString(ddlsubj.SelectedValue) + "','" + Convert.ToString(ddlmedium.SelectedValue) + "')";
                            int up = d2.update_method_wo_parameter(sql, "Text");
                            //string[] rollmess_attn = Label6.Text.Split(',');
                            //string[] appmess_attn = Label8.Text.Split(',');
                            //string[] name = Label7.Text.Split(',');
                            int ups12 = 0;
                            for (int i = 0; i < rollmess_attn.Length; i++)
                            {
                                sql = "INSERT INTO Project_BookDetails(Probook_Accno,Roll_No,Stud_Name,Degree,Lib_Code) ";
                                sql = sql + " VALUES ('" + txtaccno.Text + "','" + rollmess_attn[i] + "','" + name[i] + "','" + appmess_attn[0] + "','" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "')";
                                ups12 = d2.update_method_wo_parameter(sql, "Text");
                            }
                            if (ups12 != 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                                divPopAlertContent.Visible = false;
                                divPopAlertprojectbook.Visible = false;
                                divpoprollnumber.Visible = false;
                                alertpopwindow.Visible = false;
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "No Record Found";
                                divPopAlertContent.Visible = false;
                                divPopAlertprojectbook.Visible = false;
                                divpoprollnumber.Visible = false;
                                alertpopwindow.Visible = false;
                            }
                        }
                    }
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter Mandatory Field";
                divPopAlertContent.Visible = false;
                divPopAlertprojectbook.Visible = false;
                divpoprollnumber.Visible = false;
                alertpopwindow.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void Btnup_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            string date = DateTime.Now.ToString("yyy-MM-dd");
            int IntTotStud = 0;
            string[] rollmess_attn = Label6.Text.Split(',');
            string[] appmess_attn = Label8.Text.Split(',');
            string[] name = Label7.Text.Split(',');
            int lenth = rollmess_attn.Length;
            IntTotStud = lenth;
            // sql = "DELETE FROM Project_BookDetails WHERE ProBook_AccNo ='" + txtaccno.Text + "' AND Lib_Code ='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
            //int up = d2.update_method_wo_parameter(sql, "Text");
            if (txtaccno.Text != "" && Txttltle.Text != "" && rollmess_attn.Length > 0)
            {
                sql = "UPDATE Project_Book SET Title ='" + Txttltle.Text + "',Guide_name='" + Txtguidename.Text + "',Remarks='" + txtremark.Text + "',Lib_code='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "',Issue_Flag='" + Convert.ToString(ddlstatus.SelectedItem) + "',Access_Date='" + date + "',Tot_Stud =" + IntTotStud + ",SubmitDate ='" + TextBox2.Text + "',Subject ='" + Convert.ToString(ddlsubj.SelectedValue) + "',Language ='" + Convert.ToString(ddlmedium.SelectedValue) + "' WHERE ProBook_AccNo ='" + txtaccno.Text + "' AND Lib_Code ='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
                int up1 = d2.update_method_wo_parameter(sql, "Text");
                int ups1 = 0;

                for (int i = 0; i < rollmess_attn.Length; i++)
                {
                    sql = "INSERT INTO Project_BookDetails(Probook_Accno,Roll_No,Stud_Name,Degree,Lib_Code) ";
                    sql = sql + " VALUES ('" + txtaccno.Text + "','" + rollmess_attn[i] + "','" + name[i] + "','" + appmess_attn[0] + "','" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "')";
                    ups1 = d2.update_method_wo_parameter(sql, "Text");
                }
                if (ups1 != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Updated Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter Mandatory Field";
            }
        }
        catch
        {
        }
    }

    protected void Btndele_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            sql = "DELETE FROM Project_Book WHERE ProBook_Accno='" + txtaccno.Text + "' AND Lib_Code='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
            int up1 = d2.update_method_wo_parameter(sql, "Text");
            sql = "DELETE FROM Project_BookDetails WHERE ProBook_Accno='" + txtaccno.Text + "' AND Lib_Code='" + Convert.ToString(ddlprojectlibrary.SelectedValue) + "'";
            int up = d2.update_method_wo_parameter(sql, "Text");

            if (up != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Project Book Details Deleted Successfully";
            }

        }
        catch
        {
        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {

        divPopAlertprojectbook.Visible = false;
        divPopAlertContent.Visible = false;
        Label6.Text = "";
        Label7.Text = "";
        Label8.Text = "";
        Cellclick2 = "false";
        divpoprollnumber.Visible = false;
    }

    protected void lnkviwestudent_Click(object sender, EventArgs e)
    {
        try
        {
            if (GrdViewStu.Rows.Count == 0)
            {
                DataTable dtViewStu = new DataTable();
                DataRow drow;
                dtViewStu.Columns.Add("Roll No", typeof(string));
                dtViewStu.Columns.Add("Name", typeof(string));
                dtViewStu.Columns.Add("Department", typeof(string));

                string rolls = Label6.Text;
                string names = Label7.Text;
                string depts = Label8.Text;
                string[] rollmess_attn = rolls.Split(',');
                string[] appmess_attn = depts.Split(',');
                string[] name = names.Split(',');
                int sno = 0;
                for (int i = 0; i < rollmess_attn.Length; i++)
                {
                    drow = dtViewStu.NewRow();
                    drow["Roll No"] = rollmess_attn[i];
                    drow["Name"] = name[i];
                    drow["Department"] = appmess_attn[i];
                    dtViewStu.Rows.Add(drow);
                    divPoplinlkprojectbook.Visible = true;
                    div1.Visible = true;
                    sno++;
                }
                GrdViewStu.DataSource = dtViewStu;
                GrdViewStu.DataBind();
                GrdViewStu.Visible = true;
            }
            divPoplinlkprojectbook.Visible = true;
            div1.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "projectbook");
        }
    }

    protected void Chk_MultipleCopies_CheckedChange(object sender, EventArgs e)
    {
        if (Chk_MultipleCopies.Checked == true)
            Txt_MultipleCopies.Visible = true;
        else
            Txt_MultipleCopies.Visible = false;
    }

    #region Student

    protected void ddlbatchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Btnselectrollnogo_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable prorollno = new DataTable();
            DataRow drpro;
          
            grdStudent.Visible = true;
            prorollno.Columns.Add("Roll No", typeof(string));
            prorollno.Columns.Add("Name", typeof(string));
            prorollno.Columns.Add("Department", typeof(string));
            prorollno.Columns.Add("Reg No", typeof(string));

            string sql = string.Empty;
            string collcode = "";
            string batch = "";
            string courseid = "";
            string bran = "";
            string sem = "";
            string sec = "";
            string Section = "";            
            if (ddlcollege.Items.Count > 0)
                collcode = Convert.ToString(ddlcollege.SelectedValue);
            //collegeCode = ddlcolle.SelectedItem.Value.ToString();
            if (ddlbatchyear.Items.Count > 0)
                batch = Convert.ToString(ddlbatchyear.SelectedValue);
            if (ddldegree.Items.Count > 0)
                courseid = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                bran = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsemester.Items.Count > 0)
                sem = Convert.ToString(ddlsemester.SelectedValue);
            if (ddlsection.Items.Count > 0)
                sec = Convert.ToString(ddlsection.SelectedValue).Trim();
            if (sec == "" || sec == "All")
                Section = "";
            else
                Section = "and R.sections='" + sec + "'";
            if (txtrollno.Text == "" && TextBox1.Text == "" && txtreg.Text == "")
                sql = "SELECT distinct R.roll_no, R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester,R.Reg_No FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  and R.batch_year='" + batch + "' and  D.Dept_Code='" + bran + "' and R.Current_Semester='" + sem + "' " + Section + " order by R.roll_no";
            //AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'
            if (txtrollno.Text != "")
                sql = "SELECT distinct R.roll_no, R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester,R.Reg_No FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  and    C.college_code='" + collcode + "'  and R.roll_no='" + txtrollno.Text + "' order by R.roll_no";
            //AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'
            if (TextBox1.Text != "")
                sql = "SELECT distinct R.roll_no, R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester,R.Reg_No FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  and  C.college_code='" + collcode + "'  and R.Stud_Name='" + TextBox1.Text + "' order by R.roll_no";
            //AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'
            if (txtreg.Text != "")
                sql = "SELECT distinct R.roll_no, R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester,R.Reg_No FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  and  C.college_code='" + collcode + "'  and R.Stud_Name='" + TextBox1.Text + "' order by R.roll_no";
            //AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'
            DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
            int sno = 0;
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < bookallo.Tables[0].Rows.Count; j++)
                {
                    sno++;
                    drpro = prorollno.NewRow();

                    drpro["Name"] = Convert.ToString(bookallo.Tables[0].Rows[j]["Stud_Name"]);
                    drpro["Department"] = Convert.ToString(bookallo.Tables[0].Rows[j]["Degree"]);
                    drpro["Roll No"] = Convert.ToString(bookallo.Tables[0].Rows[j]["Roll_No"]);
                    drpro["Reg No"] = Convert.ToString(bookallo.Tables[0].Rows[j]["Reg_No"]);

                    prorollno.Rows.Add(drpro);
                }
                grdStudent.DataSource = prorollno;
                grdStudent.DataBind();
                grdStudent.Visible = true;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No record found";
            }
            btnstaffexit.Visible = true;
            // btnstaffok.Visible = true;
        }
        catch
        {

        }
    }

    protected void Btnselectrollnoclose_Click(object sender, EventArgs e)
    {

        //divpoprollnumber.Visible = false;
        divpoprollnumber.Visible = false;
        divPopAlertContent.Visible = true;
        divPopAlertprojectbook.Visible = true;
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
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStudent_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            Txtrollnumber.Text = grdStudent.Rows[rowIndex].Cells[1].Text;
            txtname.Text = grdStudent.Rows[rowIndex].Cells[2].Text;
            string deg = grdStudent.Rows[rowIndex].Cells[3].Text;
            ddldepartment.SelectedItem.Text = deg;
            //divPopAlertrollnumber.Visible = true;
            divpoprollnumber.Visible = false;
            divPopAlertContent.Visible = false;
            divPopAlertContent.Visible = true;
            divPopAlertprojectbook.Visible = true;
        }
        catch
        {
        }

    }

    #endregion

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void Btnclose1_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        divPoplinlkprojectbook.Visible = false;
        divPopAlertContent.Visible = true;
        divPopAlertprojectbook.Visible = true;
    }

    protected void btnstaffexit_Click(object sender, EventArgs e)
    {
        divstafflist.Visible = false;
        divstafflist1.Visible = false;
        divPopAlertContent.Visible = true;
        divPopAlertprojectbook.Visible = true;
        divpoprollnumber.Visible = false;
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlcollege.SelectedValue);
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
            Bindlib(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    protected void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataSet txttable = new DataSet();
            DataSet txttable1 = new DataSet();
            string colcode = Convert.ToString(ddlcollege.SelectedValue);
            Sql = "SELECT App_No,Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,Current_Semester,G.Course_ID,G.Dept_Code,DelFlag,Exam_Flag ";
            Sql = Sql + "FROM Registration R,Degree G,Course C,Department D ";
            Sql = Sql + "WHERE R.Degree_Code = G.Degree_Code ";
            Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
            Sql = Sql + "AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
            Sql = Sql + "AND Roll_No ='" + Txtrollnumber.Text + "' ";
            txttable = d2.select_method_wo_parameter(Sql, "text");
            txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Stud_Name"]);
            ddldepartment.SelectedItem.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
        }
        catch (Exception ex)
        {

        }
    }

    protected void Btnstafflistclose_Click(object sender, EventArgs e)
    { divstafflist.Visible = false; }
  
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Non Member Entry";
            string pagename = "NonMemberEntry.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(grdProBook, pagename, attendance,0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdProBook, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();

        }
        catch
        {

        }

    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch { }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

}



