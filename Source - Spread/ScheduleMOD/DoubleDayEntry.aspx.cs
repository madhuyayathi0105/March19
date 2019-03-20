using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Globalization;
using System.Configuration;

public partial class ScheduleMOD_DoubleDayEntry : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string build = "", buildvalue = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

                //bindbranch();
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            
            if (!IsPostBack)
            {
                btnSave.Visible = false;
                Bindcollege();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                string stDate;
                stDate = DateTime.Today.ToShortDateString();
                string[] dsplit_from = stDate.Split(new Char[] { '/' });
                txtFromDate.Text = dsplit_from[1].ToString().PadLeft(2, '0') + "/" + dsplit_from[0].ToString().PadLeft(2, '0') + "/" + dsplit_from[2].ToString();
            }
        }
        catch (Exception ex) { }
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

        }
    }

    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollege = " and r.college_code in(" + collegeCode + ")";
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();

                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
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
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
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
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }

    //public void bindsem()
    //{
    //    //--------------------semester load
    //    try
    //    {
    //        ds.Clear();
    //        string sqlcurrentsem = "select distinct current_semester from Registration where Batch_Year='" + ddlBatch.Text.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'";
    //        ds = da.select_method_wo_parameter(sqlcurrentsem, "Text");
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlSemester.DataSource = ds;
    //            ddlSemester.DataTextField = "current_semester";
    //            ddlSemester.DataValueField = "current_semester";
    //            ddlSemester.DataBind();
    //        }

    //    }
    //    catch
    //    {

    //    }

    //}

    //public void bindsec()
    //{
    //    //----------load section
    //    try
    //    {
    //        ds.Clear();
    //        ddlSec.Items.Clear();
    //        if (ddlBranch.SelectedValue.ToString() != "" && ddlBranch.SelectedValue.ToString() != null)
    //        {
    //            string SelectQ = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
    //            ds = da.select_method_wo_parameter(SelectQ,"Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ddlSec.DataSource = ds;
    //                ddlSec.DataTextField = "sections";
    //                ddlSemester.DataValueField = "sections";
    //                ddlSec.DataBind();
    //            }
    //        }

    //    }
    //    catch
    //    {
    //    }
    //}

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree();
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();


        }
        catch (Exception ex)
        {
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        string valDegree = string.Empty;
        string valBatch = string.Empty;
        DataTable dtCourceInfo = new DataTable();
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        if (cblBranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
        {
            string SelectQ = "select distinct (c.Course_Name+'-'+de.Dept_Name) as courceName,r.Batch_Year,r.degree_code,COUNT(r.app_no) as totalStudent from Registration r,Degree d,course c,Department de where r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code in('" + valDegree + "') and r.Batch_Year in('" + valBatch + "') and r.college_code=c.college_code and d.college_code=de.college_code and d.college_code=c.college_code and r.college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' and  CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and delflag=0 group by (c.Course_Name+'-'+de.Dept_Name),r.Batch_Year,r.degree_code order by (c.Course_Name+'-'+de.Dept_Name),r.Batch_Year,r.degree_code,totalStudent ";
            dtCourceInfo = dirAcc.selectDataTable(SelectQ);
        }
        if (dtCourceInfo.Rows.Count > 0)
        {
            int sno = 0;
            FpSpread1.Visible = true;
            //lblexportxl.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].Columns[0].Width = 70;
            FpSpread1.Sheets[0].Columns[1].Width = 100;
            FpSpread1.Sheets[0].Columns[2].Width = 250;
            FpSpread1.Sheets[0].Columns[3].Width = 100;
            FpSpread1.Sheets[0].Columns[4].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
            chkcell1.AutoPostBack = true;
            FpSpread1.Sheets[0].FrozenRowCount = 1;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            foreach (DataRow dr in dtCourceInfo.Rows)
            {
                string BatchYear = Convert.ToString(dr["batch_year"]).Trim();
                string DegreeCode = Convert.ToString(dr["degree_code"]).Trim();
                string CourceName = Convert.ToString(dr["courceName"]).Trim();
                string torstudent = Convert.ToString(dr["totalStudent"]).Trim();
                sno++;
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = BatchYear;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = CourceName;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = DegreeCode;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = torstudent;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            }
            //FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        }
        else
        {
        }
        btnSave.Visible = true;
        FpSpread1.Visible = true;
        Btndelete.Visible = false;
        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();
        FpSpread1.Width = 640;
        FpSpread1.Height = 400;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int count = 0;
        //string datefrom = string.Empty;
        Hashtable hat = new Hashtable();
        string date1 = string.Empty;
        date1 = txtFromDate.Text.ToString();
        DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
        bool isValidDate = DateTime.TryParseExact(date1, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1);
        int isval = 0;
        FpSpread1.SaveChanges();
        for (int s = 1; s < FpSpread1.Sheets[0].RowCount; s++)
        {

            //Convert.ToInt32(FpSpread1.Sheets[0].Cells[s, 4].Value);
            int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[s, 4].Value), out isval);
            if (isval == 1)
            {
                string batchYear = FpSpread1.Sheets[0].Cells[s, 1].Text;
                string degCode = Convert.ToString(FpSpread1.Sheets[0].Cells[s, 2].Note);
                hat.Clear();
                hat.Add("@batchyear", batchYear);
                hat.Add("@degreecode", degCode);
                hat.Add("@doubleDate", dt1.ToString("MM/dd/yyyy"));
                hat.Add("@college_code", Convert.ToString(ddlCollege.SelectedValue));
                count = da.update_method_with_parameter("Insert_Doubleday_schdl", hat, "sp");
            }

        }
        if (count > 0)
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Saved Successfully";
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        string date1 = string.Empty;
        date1 = txtFromDate.Text.ToString();
        DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
        bool isValidDate = DateTime.TryParseExact(date1, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1);
        FpSpread1.Visible = false;
        string SelectQ = "select distinct (c.Course_Name+'-'+de.Dept_Name) as courceName,r.Batch_Year,r.degree_code,COUNT(r.app_no) as totalStudent from Registration r,Degree d,course c,Department de,doubledayorder do where r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code=do.degreeCode and r.Batch_Year=do.batchYear and do.doubledate='" + dt1.ToString("MM/dd/yyyy") + "' and r.college_code=c.college_code and d.college_code=de.college_code and d.college_code=c.college_code and r.college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and delflag=0 group by (c.Course_Name+'-'+de.Dept_Name),r.Batch_Year,r.degree_code order by (c.Course_Name+'-'+de.Dept_Name),r.Batch_Year,r.degree_code,totalStudent";

        DataTable dtsaveDate = dirAcc.selectDataTable(SelectQ);
        if (dtsaveDate.Rows.Count > 0)
        {
            int sno = 0;
            FpSpread1.Visible = true;
            //lblexportxl.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].Columns[0].Width = 70;
            FpSpread1.Sheets[0].Columns[1].Width = 100;
            FpSpread1.Sheets[0].Columns[2].Width = 250;
            FpSpread1.Sheets[0].Columns[3].Width = 100;
            //FpSpread1.Sheets[0].Columns[4].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
            chkcell1.AutoPostBack = true;
            FpSpread1.Sheets[0].FrozenRowCount = 1;
            FpSpread1.Sheets[0].AutoPostBack = false;

            foreach (DataRow dr in dtsaveDate.Rows)
            {
                string BatchYear = Convert.ToString(dr["batch_year"]).Trim();
                string DegreeCode = Convert.ToString(dr["degree_code"]).Trim();
                string CourceName = Convert.ToString(dr["courceName"]).Trim();
                string torstudent = Convert.ToString(dr["totalStudent"]).Trim();
                sno++;
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = BatchYear;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = CourceName;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = DegreeCode;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = torstudent;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            }
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Width = 540;
            FpSpread1.Height = 400;
            Btndelete.Visible = true;
            Div1.Visible = true;
        }
        else
        {
            Div1.Visible = false;
            Btndelete.Visible = false;
        }
        btnSave.Visible = false;



    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Convert.ToString(e.SheetView.ActiveRow).Trim();
            if (actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); j++)
                {
                    string actcol = Convert.ToString(e.SheetView.ActiveColumn).Trim();
                    string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
                    if (seltext != "System.Object")
                        FpSpread1.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();

                }
            }
            //string ctrlname = Page.Request.Params["__EVENTTARGET"];
            //if (ctrlname != null && ctrlname != String.Empty)
            //{
            //    string[] spiltspreadname = ctrlname.Split('$');
            //    if (spiltspreadname.GetUpperBound(0) > 1)
            //    {
            //        string getrowxol = spiltspreadname[3].ToString().Trim();
            //        string[] spr = getrowxol.Split(',');
            //        if (spr.GetUpperBound(0) == 1)
            //        {
            //            int arow = Convert.ToInt32(spr[0]);
            //            int acol = Convert.ToInt32(spr[1]);
            //            if (arow == 0 && acol > 4)
            //            {
            //                string setval = e.EditValues[acol].ToString();
            //                int setvalcel = 0;
            //                if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
            //                {
            //                    setvalcel = 1;
            //                }
            //                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            //                {
            //                    FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
            //                }
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {

        }
    }

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
        catch { }
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
        catch { }
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
        catch { }
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
        catch { }
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
        catch
        {
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
        catch
        {
            return false;
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
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopAlert.Visible = false;
        lblAlertMsg.Visible = false;
    }
    //protected void Fpspread2_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.Fpspread1, "Select$" + e.Row.RowIndex);
    //        }
    //    }
    //    catch { }
    //}
    //protected void Fpspread2_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName == "Select")
    //        {

    //        }

    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void Fpspread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    string actrow = Convert.ToString(e.SheetView.ActiveRow).Trim();
    //    if (flag_true == false && actrow == "0")
    //    {
    //        for (int j = 1; j < Convert.ToInt16(Fpspread2.Sheets[0].RowCount); j++)
    //        {
    //            string actcol = Convert.ToString(e.SheetView.ActiveColumn).Trim();
    //            string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
    //            if (seltext != "System.Object")
    //                Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
    //        }
    //        flag_true = true;
    //    }
    //}
    protected void Btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            //string datefrom = string.Empty;
            Hashtable hat = new Hashtable();
            string date1 = string.Empty;
            date1 = txtFromDate.Text.ToString();
            DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
            bool isValidDate = DateTime.TryParseExact(date1, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1);
            int isval = 0;
            FpSpread1.SaveChanges();
            for (int s = 1; s < FpSpread1.Sheets[0].RowCount; s++)
            {

                //Convert.ToInt32(FpSpread1.Sheets[0].Cells[s, 4].Value);
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[s, 4].Value), out isval);
                if (isval == 1)
                {
                    string batchYear = FpSpread1.Sheets[0].Cells[s, 1].Text;
                    string degCode = Convert.ToString(FpSpread1.Sheets[0].Cells[s, 2].Note);
                    hat.Clear();
                    hat.Add("@batchyear", batchYear);
                    hat.Add("@degreecode", degCode);
                    hat.Add("@doubleDate", dt1.ToString("MM/dd/yyyy"));
                    hat.Add("@college_code", Convert.ToString(ddlCollege.SelectedValue));
                    count = da.update_method_with_parameter("Delete_Doubleday_schdl", hat, "sp");
                }

            }
            if (count > 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Delete Successfully";
            }
        }
        catch
        {
        }
    }



    protected void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {

            string Hostel = "Late Attendance Report ";
            string pagename = "Late Attendance Report.aspx";

            FpSpread1.Columns[4].Visible = false;
            if (FpSpread1.Visible == true)
            {
                Printmaster1.loadspreaddetails(FpSpread1, pagename, Hostel);
            }
            Printmaster1.Visible = true;
            Label3.Visible = false;
            FpSpread1.Columns[4].Visible = true;
        }

        catch
        {
        }
    }
    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        Label3.Visible = false;
        try
        {
            string reportname = TextBox3.Text;
            FpSpread1.Columns[4].Visible = false;
            if (reportname.ToString().Trim() != "")
            {

                if (FpSpread1.Visible == true)
                {
                    da.printexcelreport(FpSpread1, reportname);
                }
                Label3.Visible = false;
            }
            else
            {
                Label3.Text = "Please Enter Your Report Name";
                Label3.Visible = true;
                TextBox3.Focus();
            }
            FpSpread1.Columns[4].Visible = true;
        }
        catch
        {
        }
    }
}


