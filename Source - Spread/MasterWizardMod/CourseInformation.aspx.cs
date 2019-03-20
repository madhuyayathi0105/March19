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
using System.Data.SqlClient;

public partial class MasterWizardMod_CourseInformation : System.Web.UI.Page
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
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    static string searchclgcode = string.Empty;
    DataSet dscode1 = new DataSet();
    DataTable dtcor = new DataTable();
    DataRow drinfo;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
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
                Education();
                type();
                if (ddleducation.SelectedItem.Text.ToLower().Trim() == "others")
                {
                    txtedu.Visible = true;
                }
                else
                {
                    txtedu.Visible = false;
                }
            }
        }
        catch
        {
        }

    }

    #region bind

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

    public void Education()
    {
        try
        {
            ddleducation.Items.Clear();
            ds.Clear();


            string strquery = "select distinct Edu_Level from course";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddleducation.DataSource = ds;
                ddleducation.DataTextField = "Edu_Level";
                ddleducation.DataValueField = "Edu_Level";
                ddleducation.DataBind();
                ddleducation.Items.Insert(0, " ");
                ddleducation.Items.Insert(1, "Others");

            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    public void type()
    {
        try
        {
            ddltype.Items.Clear();
            ds.Clear();


            string strquery1 = "select distinct type from course";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
                ddltype.Items.Insert(0, " ");

            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #endregion

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddleducation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddleducation.SelectedItem.Text.ToLower().Trim() == "others")
        {
            txtedu.Visible = true;
        }
        else
        {
            txtedu.Visible = false;
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        btnupdate.Visible = false;
        btn_save.Visible = true;
        Education();
        type();
        if (ddleducation.SelectedItem.Text.ToLower().Trim() == "others")
        {
            txtedu.Visible = true;
        }
        else
        {
            txtedu.Visible = false;
        }
        popwindow1.Visible = true;

        ddltype.Attributes.Add("onfocus", "frelig5()");
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        DataSet idgenertionstaff = new DataSet();

        idgenertionstaff = cours();
        if (idgenertionstaff.Tables.Count > 0 && idgenertionstaff.Tables[0].Rows.Count > 0)
        {
            loadspreadstaff(ds);

        }

        else
        {
            //Div1.Visible = true;
            grdcourse.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "No Record Found!";
        }
    }

    private DataSet cours()
    {

        try
        {

            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string course = txtsearch1.Text;
            string sel = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(course))
            {

                sel = "select Course_Name,Edu_Level,type from course where college_code='" + collegeCode + "' and Course_Name='" + course + "'";



                dscode1.Clear();
                dscode1 = da.select_method_wo_parameter(sel, "Text");
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

            string coursee = string.Empty;
            string educat = string.Empty;
            string typpp = string.Empty;
            dtcor.Columns.Add("SNo", typeof(string));
            dtcor.Columns.Add("Course Name", typeof(string));
            dtcor.Columns.Add("Education Level", typeof(string));
            dtcor.Columns.Add("Type", typeof(string));
            int sno = 0;

            drinfo = dtcor.NewRow();
            drinfo["SNo"] = "SNo";
            drinfo["Course Name"] = "Course Name";
            drinfo["Education Level"] = "Education Level";
            drinfo["Type"] = "Type";
            dtcor.Rows.Add(drinfo);
            if (dscode1.Tables.Count > 0 && dscode1.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dscode1.Tables[0].Rows.Count; row++)
                {

                    sno++;
                    drinfo = dtcor.NewRow();

                    coursee = Convert.ToString(dscode1.Tables[0].Rows[row]["Course_Name"]).Trim();
                    educat = Convert.ToString(dscode1.Tables[0].Rows[row]["Edu_Level"]).Trim();
                    typpp = Convert.ToString(dscode1.Tables[0].Rows[row]["type"]).Trim();
                    drinfo["SNo"] = Convert.ToString(sno);
                    drinfo["Course Name"] = coursee;
                    drinfo["Education Level"] = educat;
                    drinfo["Type"] = typpp;

                    dtcor.Rows.Add(drinfo);
                }
                divtable.Visible = true;
                grdcourse.DataSource = dtcor;
                grdcourse.DataBind();
                RowHead(grdcourse);
                grdcourse.Visible = true;



                for (int l = 0; l < grdcourse.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdcourse.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdcourse.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }

            }

        }

        catch (Exception ex)
        { }
    }

    protected void RowHead(GridView grdcourse)
    {
        for (int head = 0; head < 1; head++)
        {
            grdcourse.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdcourse.Rows[head].Font.Bold = true;
            grdcourse.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }
    protected void grdcourse_OnRowCreated(object sender, GridViewRowEventArgs e)
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
    protected void grdcourse_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            popwindow1.Visible = true;
            btnupdate.Visible = true;
            btn_delete.Visible = true;
            btn_save.Visible = false;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            if (Convert.ToString(rowIndex) != "-1")
            {
                txt_course.Text = Convert.ToString(grdcourse.Rows[rowIndex].Cells[1].Text);
                ddleducation.SelectedItem.Text = Convert.ToString(grdcourse.Rows[rowIndex].Cells[2].Text);
                ddltype.SelectedItem.Text = Convert.ToString(grdcourse.Rows[rowIndex].Cells[3].Text);
            }


        }

        catch
        {
        }
    }


    #region plusminustype
    protected void btn_pls_mat_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Type";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch
        {
        }

    }
    protected void btn_min_mat_Click(object sender, EventArgs e)
    {
        try
        {
            string educa = Convert.ToString(ddleducation.Text);
            string get = d2.GetFunction("select distinct type  from course where Course_Name='" + txt_course.Text + "' and Edu_Level='" + educa + "'");
            int getcnt = Convert.ToInt32(get);
            if (getcnt > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Alredy Books Available in this Attachment.So it Cannot be deleted.";
                return;

            }

            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Attachment";
                return;
            }

        }
        catch
        {
        }

    }
    #endregion

    #region Add_And_Exit
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);

            if (group != "")
            {
                if (lbl_addgroup.Text.Trim() == "Type")
                {

                    int j = ddltype.Items.Count;
                    ddltype.Items.Insert(j, group);

                }
                plusdiv.Visible = false;
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Please Enter the " + lbl_addgroup.Text + "";
            }
        }

        catch
        {
        }
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex)
        { }
    }
    #endregion

    protected void btnpopsave_Click(object sender, EventArgs e)
    {

        int query = 0;
        string sql = string.Empty;
        string name = string.Empty;
        string edu = string.Empty;
        string ty = string.Empty;
        try
        {

            name = Convert.ToString(txt_course.Text);
            edu = Convert.ToString(ddleducation.Text);
            ty = Convert.ToString(ddltype.Text);

            sql = "if exists (select * from course where Course_Name='" + name + "' and Edu_Level='" + edu + "' and type='" + ty + "' and college_code='" + ddlCollege.SelectedValue.ToString() + "') update course set Course_Name='" + name + "',Edu_Level='" + edu + "',type='" + ty + "' where Course_Name='" + name + "' and Edu_Level='" + edu + "' and college_code='" + ddlCollege.SelectedValue.ToString() + "' else insert into course(Course_Name,Edu_Level,type,college_code) values ('" + name + "','" + edu + "','" + ty + "','" + ddlCollege.SelectedValue.ToString() + "')";
            query = d2.update_method_wo_parameter(sql, "TEXT");
            if (query != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully";

            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter The Gym Details!";
            }
        }
        catch
        {
        }

    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        int query = 0;
        string sqlu = string.Empty;
        string name1 = string.Empty;
        string edu1 = string.Empty;
        string ty1 = string.Empty;
        try
        {

            name1 = Convert.ToString(txt_course.Text);
            edu1 = Convert.ToString(ddleducation.Text);
            ty1 = Convert.ToString(ddltype.SelectedItem.Text);

            sqlu = "update course set Course_Name='" + name1 + "', Edu_Level='" + edu1 + "',type='" + ty1 + "' where Course_Name='" + name1 + "'";
            query = d2.update_method_wo_parameter(sqlu, "TEXT");
            if (query != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully";

            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch
        {
        }
    }
    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        txt_course.Text = string.Empty;
        ddleducation.Items[0].Selected = true;
        txtedu.Text = string.Empty;

        ddltype.Items[0].Selected = true;
        popwindow1.Visible = false;
    }
    #region delete
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                alertpopwindow.Visible = true;
                lbl_sure.Text = "Do you want to delete this record?";


            }
        }
        catch
        {
        }

    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        popwindow1.Visible = true;


    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        popwindow1.Visible = true;
    }

    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string sqld = "";
            string course = "";
            string eduact = "";
            string typ = "";
            int query = 0;
            course = Convert.ToString(txt_course.Text);
            eduact = Convert.ToString(ddleducation.SelectedItem.Text);
            typ = Convert.ToString(ddltype.SelectedItem.Text);
            string qry1 = "select d.course_id from degree d,course c where c.course_id=d.course_id and course_Name='" + course.Trim() + "'";
            DataSet dss = da.select_method_wo_parameter(qry1, "text");
            if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {

                string delqry = "delete from degree where course_id='" + Convert.ToString(dss.Tables[0].Rows[0]["course_id"]) + "'";
                int del = d2.update_method_wo_parameter(delqry, "text");
            }

            sqld = "delete from course where Course_Name='" + course + "' and Edu_Level='" + eduact + "' and type='" + typ + "'";
            query = d2.update_method_wo_parameter(sqld, "TEXT");
            if (query != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                pnl2.Visible = true;
                lblalerterr.Text = "Deleted Successfully";
                Div3.Visible = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                pnl2.Visible = true;
                lblalerterr.Text = "Not Deleted";
                Div3.Visible = false;
            }



        }
        catch (Exception ex) { }
    }
    #endregion

    # region Getrno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();

        try
        {
            string query = "";
            WebService ws = new WebService();
            {
                string txtval = string.Empty;
                query = "select distinct Course_Name from course  where Course_Name like '" + prefixText + "%'  order by Course_Name";
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    # endregion
}