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


public partial class PresentnAbsentCountDetails : System.Web.UI.Page
{

    DAccess2 da = new DAccess2();
    DAccess2 dt = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable ht = new Hashtable();

    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
    static string loadval = string.Empty;
    static string colval = string.Empty;
    static string printval = string.Empty;
    bool Cellclick = false;
    static byte roll = 0;
    static string columnname = string.Empty;
    static string columnname1 = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("OffM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("OfficeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                    return;
                }
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            }
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyy");
                Bindcollege();
                // BindRightsBaseBatch();
                binddegree();
                bindbranch();
                binddept();
                Radioformat1.Checked = true;
                columnordertype();
                Radioformat1.Checked = true;
                lbldep.Visible = false;
                UpdatePanel11.Visible = false;
                txt_dept.Visible = false;



            }
        }
        catch
        {
        }
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

    //public void BindRightsBaseBatch()
    //{
    //    try
    //    {
    //        DataSet dsBatch = new DataSet();
    //        userCode = string.Empty;
    //        groupUserCode = string.Empty;
    //        qryUserOrGroupCode = string.Empty;
    //        collegeCode = string.Empty;
    //        ds.Clear();
    //        chkBatch.Checked = false;
    //        cblBatch.Items.Clear();
    //        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
    //        {
    //            string group = Convert.ToString(Session["group_code"]).Trim();
    //            if (group.Contains(";"))
    //            {
    //                string[] group_semi = group.Split(';');
    //                groupUserCode = Convert.ToString(group_semi[0]);
    //            }
    //            if (!string.IsNullOrEmpty(groupUserCode))
    //            {
    //                qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
    //            }
    //        }
    //        else if (Session["usercode"] != null)
    //        {
    //            userCode = Convert.ToString(Session["usercode"]).Trim();
    //            if (!string.IsNullOrEmpty(userCode))
    //            {
    //                qryUserOrGroupCode = " and user_id='" + userCode + "'";
    //            }
    //        }
    //        if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
    //        {
    //            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
    //            if (!string.IsNullOrEmpty(collegeCode))
    //            {
    //                qryCollege = " and r.college_code in(" + collegeCode + ")";
    //            }
    //        }
    //        if (!string.IsNullOrEmpty(collegeCode))
    //        {
    //            qryCollege = " and r.college_code in(" + collegeCode + ")";
    //        }

    //        dsBatch.Clear();
    //        if (!string.IsNullOrEmpty(qryUserOrGroupCode))
    //        {
    //            string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
    //            dsBatch = da.select_method_wo_parameter(qry, "Text");
    //        }
    //        qryBatch = string.Empty;
    //        if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
    //        {
    //            List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
    //            if (lstBatch.Count > 0)
    //                qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
    //        }
    //        string batchquery = string.Empty;
    //        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
    //        {
    //            batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
    //            //ds.Clear();
    //            ds = da.select_method_wo_parameter(batchquery, "Text");
    //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //            {
    //                cblBatch.DataSource = ds;
    //                cblBatch.DataTextField = "Batch_Year";
    //                cblBatch.DataValueField = "Batch_Year";
    //                cblBatch.DataBind();

    //                checkBoxListselectOrDeselect(cblBatch, true);
    //                CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
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
            //if (cblBatch.Items.Count > 0)
            //    valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "')  " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//AND r.Batch_Year in('" + valBatch + "')
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
            //if (cblBatch.Items.Count > 0)
            //    valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "')  AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//AND r.Batch_Year in('" + valBatch + "')
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

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //BindRightsBaseBatch();
            binddegree();
            bindbranch();
            columnordertype();

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

    protected void Radioformat1_CheckedChanged(object sender, EventArgs e)
    {
        lbldep.Visible = false;
        txt_dept.Visible = false;
        UpdatePanel11.Visible = false;
        lblDegree.Visible = true;
        upnlDegree.Visible = true;
        txtDegree.Visible = true;
        lblBranch.Visible = true;
        upnlBranch.Visible = true;
        txtBranch.Visible = true;
        Fpspread1.Visible = false;
        Fpspread2.Visible = false;
        div_report.Visible = false;
        columnordertype();


    }

    protected void Radioformat2_CheckedChanged(object sender, EventArgs e)
    {


        lbldep.Visible = true;
        txt_dept.Visible = true;
        UpdatePanel11.Visible = true;
        lblDegree.Visible = false;
        upnlDegree.Visible = false;
        txtDegree.Visible = false;
        lblBranch.Visible = false;
        upnlBranch.Visible = false;
        txtBranch.Visible = false;
        Fpspread1.Visible = false;
        Fpspread2.Visible = false;
        div_report.Visible = false;
        columnordertype();

    }

    protected void rdbguest_CheckedChanged(object sender, EventArgs e)
    {


        lbldep.Visible = false;
        txt_dept.Visible = false;
        UpdatePanel11.Visible = false;
        lblDegree.Visible = false;
        upnlDegree.Visible = false;
        txtDegree.Visible = false;
        lblBranch.Visible = false;
        upnlBranch.Visible = false;
        txtBranch.Visible = false;
        Fpspread1.Visible = false;
        Fpspread2.Visible = false;
        div_report.Visible = false;
        columnordertype();

    }


    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

        }

        catch (Exception ex)
        {

        }
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        {

        }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                if (Radioformat1.Checked == true)
                {

                    string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReport' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddl_colord.SelectedItem.Text != "Select")
                        {
                            fpspread1go1();
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            lblAlertMsg.Text = "Kindly Select Report Type";
                        }
                    }
                    else
                    {
                        imgbtn_all_Click(sender, e);
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Kindly Set Report Type";
                    }

                }
            }
            if (Cellclick == true)
            {
                if (Radioformat2.Checked == true)//delsis
                {
                    string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReportStaff' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");

                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddl_colord.SelectedItem.Text != "Select")
                        {
                            fpspread1go1staff();
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            lblAlertMsg.Text = "Kindly Select Report Type";
                        }
                    }
                    else
                    {
                        imgbtn_all_Click(sender, e);
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Kindly Set Report Type";
                    }

                }
            }
            if (Cellclick == true)
            {
                if (rdbguest.Checked == true)//delsis
                {
                    string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReportGuest' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");

                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddl_colord.SelectedItem.Text != "Select")
                        {
                            fpspread1go1guest();
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            lblAlertMsg.Text = "Kindly Select Report Type";
                        }
                    }
                    else
                    {
                        imgbtn_all_Click(sender, e);
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Kindly Set Report Type";
                    }

                }
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Radioformat1.Checked == true)
            {
                Hashtable totalmode = new Hashtable();
                totalmode.Clear();
                string fdate = txtDate.Text;
                string[] f_split = fdate.Split(new Char[] { '/' });
                int MonthYear = Convert.ToInt16(f_split[1].ToString()) + (Convert.ToInt16(f_split[2].ToString()) * 12);
                DateTime dt = Convert.ToDateTime(f_split[1].ToString() + "/" + f_split[0].ToString() + "/" + f_split[2].ToString());
                string ColName = "d" + dt.Day.ToString() + "d1";
                string valBatch = string.Empty;
                string valDegree = string.Empty;
                string valBranch = string.Empty;
                int tcount = 0;
                int dcount = 0;
                int overalltot = 0;

                if (ddlCollege.Items.Count > 0)
                {
                    collegeCode = ddlCollege.SelectedValue.ToString().Trim();
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lblCollege.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }

                if (cblDegree.Items.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lblDegree.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }

                else
                {
                    valDegree = rs.GetSelectedItemsValueAsString(cblDegree);
                    if (string.IsNullOrEmpty(valDegree))
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Select Atleast One " + lblDegree.Text + "";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                if (cblBranch.Items.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lblBranch.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    valBranch = rs.GetSelectedItemsValueAsString(cblBranch);
                    if (string.IsNullOrEmpty(valBranch))
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Select Atleast One " + lblBranch.Text + "";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                string messType = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegeCode + "'";
                DataTable dtStu = dirAcc.selectDataTable(messType);
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
                Fpspread1.Sheets[0].ColumnCount = 5;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;//delsi 0709
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Branch";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Strength";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostler";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Tag = "Hostler";
                if (dtStu.Rows.Count > 0)
                {
                    int column = Fpspread1.Sheets[0].ColumnCount;
                    int colcount = 0;
                    for (int i = 0; i < dtStu.Rows.Count; i++)
                    {
                        colcount++;
                        string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                        string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = messName;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = messId;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Hostler" + "," + messId + "," + "P";


                        colcount++;
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Hostler" + "," + messId + "," + "P";

                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 2, 1, dtStu.Rows.Count);
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                    }
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, column - 1, 1, colcount);
                }
               
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Day Scholor";
                dcount = Fpspread1.Sheets[0].ColumnCount - 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Day Scholor";

                int count1 = Fpspread1.Sheets[0].ColumnCount;
                if (dtStu.Rows.Count > 0)
                {
                    int countCol = 0;
                    int column = Fpspread1.Sheets[0].ColumnCount;
                    for (int i = 0; i < dtStu.Rows.Count; i++)
                    {
                        countCol++;
                        string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                        string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = messName;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = messId;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Day Scholor" + "," + messId + "," + "P";
                        countCol++;
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Day Scholor" + "," + messId + "," + "A";

                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 2, 1, dtStu.Rows.Count);

                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                    }
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count1 - 1, 1, countCol);
                }
                //Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount;
                tcount = Fpspread1.Sheets[0].ColumnCount - 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Transport";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, 2, 2);
                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";




                Fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 200;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 14;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Black;
                style2.BackColor = Color.AliceBlue;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.SaveChanges();

                string SelectQ = "select distinct r.degree_code,de.Dept_Name,c.Course_Name,r.college_code from Registration r,Degree d,Department de,course c where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + valBranch + "') and r.college_code in('" + collegeCode + "') order by c.Course_Name,de.Dept_Name,r.degree_code";
                DataTable dtDegInfo = dirAcc.selectDataTable(SelectQ);
                if (dtDegInfo.Rows.Count > 0)
                {
                    int row = 0;
                    int columnCount = 0;
                    int rowcount = Fpspread1.Sheets[0].RowCount;
                    foreach (DataRow dr in dtDegInfo.Rows)
                    {
                        row++;
                        string degCode = Convert.ToString(dr["degree_code"]);
                        string colCode = Convert.ToString(dr["college_code"]);
                        string deptName = Convert.ToString(dr["Dept_Name"]);
                        string course = Convert.ToString(dr["Course_Name"]);
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = row.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = course;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = deptName;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = degCode;
                        DataSet studDetDS = new DataSet();
                        string query = " select * from Registration r,Degree d,Department de,course c where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)";

                        query += " select * from Registration r,Degree d,Department de,course c where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and stud_Type='Hostler' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)";

                        query += " select * from Registration r,Degree d,Department de,course c where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and stud_Type='Day Scholar' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)";


                        query += " select distinct r.Roll_No ,r.app_no, StudMessType from Registration r,Degree d,Department de,course c,HT_HostelRegistration hr where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and stud_Type='Hostler' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) and r.App_No=hr.APP_No and MemType='1' union all select distinct m.Roll_No,m.app_no, messtype as StudMessType  from stud_messtype m,registration r ,Degree d,Department de,course c where CONVERT(nvarchar, date,103) ='" + txtDate.Text + "' and stu_type='2' and m.roll_no=r.roll_no and m.app_no=r.app_no and  r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "'  and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) ";//


                        //   query += " select StudMessType,* from Registration r,Degree d,Department de,course c,DayScholourStaffAdd ds  where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and r.Roll_No=ds.Roll_No and   DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) AND r.App_No not in (select APP_No from HT_HostelRegistration)";

                        query += "  select  distinct r.Roll_No ,r.app_no ,StudMessType,date    from Registration r,Degree d,Department de,course c,DayScholourStaffAdd ds  where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and r.Roll_No=ds.Roll_No and   DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) AND r.App_No not in (select APP_No from HT_HostelRegistration) union all select distinct m.Roll_No,m.app_no, messtype as StudMessType,date  from stud_messtype m,registration r ,Degree d,Department de,course c where CONVERT(nvarchar, date,103) ='" + txtDate.Text + "' and stu_type='1' and m.roll_no=r.roll_no and m.app_no=r.app_no and  r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "'  and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)  order by DATE desc";//union all select distinct Roll_No,app_no, messtype as StudMessType,date from stud_messtype where CONVERT(nvarchar, date,103) ='" + txtDate.Text + "' and stu_type='1' order by DATE desc

                        query += " select Roll_No,roll_admit,App_no,Stud_Name,Bus_RouteID,VehID,Boarding,de.dept_acronym,Seat_No,r.college_code,r.Current_Semester from Registration r,Degree d,Department de where  r.degree_code=d.Degree_Code and d.Dept_Code=de.dept_code  and isnull(IsCanceledStage,0)<>'1' and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and VehID<>'' and Boarding is not null and Boarding<>'' and r.degree_code in('" + degCode + "') and r.college_code in('" + ddlCollege.SelectedItem.Value + "')  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'  order by len(Seat_No),Seat_No";
                        query += "  select distinct  CONVERT(nvarchar(max), app_no) as App_no,roll_no,messtype as StudMessType,date,stu_type from stud_messtype where CONVERT(nvarchar, date,103) ='" + txtDate.Text + "'";

                        studDetDS = da.select_method_wo_parameter(query, "text");


                        string overallStudentCount = da.GetFunction("select Count(Roll_No) from Registration r,Degree d,Department de,course c where r.degree_code=d.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code in('" + degCode + "') and r.college_code='" + ddlCollege.SelectedItem.Value + "' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)");
                        string getroll = string.Empty;

                        for (int rollNum = 0; rollNum < studDetDS.Tables[0].Rows.Count; rollNum++)
                        {
                            string roll = Convert.ToString(studDetDS.Tables[0].Rows[rollNum]["Roll_No"]);
                            if (getroll == "")
                            {
                                getroll = roll;
                            }
                            else
                            {
                                getroll = getroll + "','" + roll;
                            }

                        }
                        overalltot = overalltot + Convert.ToInt32(overallStudentCount);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = overallStudentCount;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(getroll);


                        if (studDetDS.Tables[3].Rows.Count > 0)
                        {
                            Hashtable hs = new Hashtable();
                            int colcount = 3;
                            for (int i = 0; i < dtStu.Rows.Count; i++)
                            {
                                int prsentCount = 0;
                                int absentCount = 0;
                                string prestrollnotag = string.Empty;
                                string absrolnotag = string.Empty;
                                DataTable stumesstbl = new DataTable();
                                int val = 0;
                                colcount++;
                                string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                                val = Convert.ToInt32(messId) - 1;
                                string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                             
                                studDetDS.Tables[3].DefaultView.RowFilter = "StudMessType='" + val + "'";
                                DataTable dtStuappfilters = studDetDS.Tables[3].DefaultView.ToTable();

                                if (dtStuappfilters.Rows.Count > 0)
                                {
                                    for (int stttype = 0; stttype < dtStuappfilters.Rows.Count; stttype++)
                                    {
                                        studDetDS.Tables[6].DefaultView.RowFilter = "StudMessType='" + val + "' and Roll_No='" + Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"]) + "' and stu_type='2'";
                                        stumesstbl = studDetDS.Tables[6].DefaultView.ToTable();
                                        if (!hs.ContainsKey(Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"])))
                                        {

                                            studDetDS.Tables[6].DefaultView.RowFilter = "Roll_No='" + Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"]) + "' and stu_type='2'";
                                            DataTable vegstumesstbl = studDetDS.Tables[6].DefaultView.ToTable();
                                            if (vegstumesstbl.Rows.Count > 0)
                                            {
                                                if (stumesstbl.Rows.Count > 0)
                                                {

                                                    string rollno = Convert.ToString(dtStuappfilters.Rows[stttype]["roll_no"]);
                                                    hs.Add(rollno, val);
                                                    string getpresentAbsent = da.GetFunction("select " + ColName + " from attendance where roll_no='" + rollno + "' and month_year='" + MonthYear + "'");


                                                    if (getpresentAbsent != "" && getpresentAbsent != "0")// magesh 16.10.18 add getpresentAbsent != "0"
                                                    {
                                                        if (getpresentAbsent == "1")
                                                        {
                                                            prsentCount = prsentCount + 1;

                                                            if (prestrollnotag == "")
                                                            {
                                                                prestrollnotag = rollno;
                                                            }
                                                            else
                                                            {
                                                                prestrollnotag = prestrollnotag + "','" + rollno;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            absentCount = absentCount + 1;
                                                            if (absrolnotag == "")
                                                            {
                                                                absrolnotag = rollno;

                                                            }
                                                            else
                                                            {
                                                                absrolnotag = absrolnotag + "','" + rollno;
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string rollno = Convert.ToString(dtStuappfilters.Rows[stttype]["roll_no"]);
                                                hs.Add(rollno, val);
                                                string getpresentAbsent = da.GetFunction("select " + ColName + " from attendance where roll_no='" + rollno + "' and month_year='" + MonthYear + "'");

                                                if (getpresentAbsent != "" && getpresentAbsent != "0")
                                                {
                                                    if (getpresentAbsent == "1")
                                                    {
                                                        prsentCount = prsentCount + 1;
                                                        if (prestrollnotag == "")
                                                        {
                                                            prestrollnotag = rollno;
                                                        }
                                                        else
                                                        {
                                                            prestrollnotag = prestrollnotag + "','" + rollno;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        absentCount = absentCount + 1;
                                                        if (absrolnotag == "")
                                                        {
                                                            absrolnotag = rollno;

                                                        }
                                                        else
                                                        {
                                                            absrolnotag = absrolnotag + "','" + rollno;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(prsentCount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(prestrollnotag);

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;

                                    if (!totalmode.Contains(Convert.ToString(colcount)))
                                    {
                                        totalmode.Add(Convert.ToString(colcount), Convert.ToString(prsentCount));
                                    }
                                    else
                                    {
                                        int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcount)]);

                                        getvalue = getvalue + prsentCount;
                                        totalmode.Remove(Convert.ToString(colcount));

                                        totalmode.Add(Convert.ToString(colcount), Convert.ToString(getvalue));


                                    }

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;
                                    colcount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(absentCount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(absrolnotag);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;


                                    if (!totalmode.Contains(Convert.ToString(colcount)))
                                    {
                                        totalmode.Add(Convert.ToString(colcount), Convert.ToString(absentCount));
                                    }
                                    else
                                    {
                                        int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcount)]);

                                        getvalue = getvalue + absentCount;
                                        totalmode.Remove(Convert.ToString(colcount));

                                        totalmode.Add(Convert.ToString(colcount), Convert.ToString(getvalue));


                                    }
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(prsentCount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                    colcount++;

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(absentCount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;

                                }

                                columnCount = colcount;

                            }


                        }


                        if (studDetDS.Tables[4].Rows.Count > 0)
                        {
                            Hashtable hs = new Hashtable();
                            int colcounts = dcount - 1;//Fpspread1.Sheets[0].ColumnCount - 1
                            for (int i = 0; i < dtStu.Rows.Count; i++)
                            {
                                int prsentCount = 0;
                                int absentCount = 0;
                                string prestrollnotag = string.Empty;
                                string absrolnotag = string.Empty;
                                int val = 0;
                                colcounts++;
                                string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                                val = Convert.ToInt32(messId) - 1;
                                string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                                DataTable stumesstbl = new DataTable();
                               
                                studDetDS.Tables[4].DefaultView.RowFilter = "StudMessType='" + val + "'";
                                //studDetDS.Tables[6].DefaultView.RowFilter = "StudMessType='" + val + "'";
                                //stumesstbl = studDetDS.Tables[6].DefaultView.ToTable();
                                DataTable dtStuappfilters = studDetDS.Tables[4].DefaultView.ToTable();
                                if (dtStuappfilters.Rows.Count > 0)
                                {
                                    for (int stttype = 0; stttype < dtStuappfilters.Rows.Count; stttype++)
                                    {
                                       
                                        studDetDS.Tables[6].DefaultView.RowFilter = "StudMessType='" + val + "' and Roll_No='" + Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"]) + "' and stu_type='1'";
                                        stumesstbl = studDetDS.Tables[6].DefaultView.ToTable();
                                        if (!hs.ContainsKey(Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"])))
                                        {
                                             studDetDS.Tables[6].DefaultView.RowFilter = "Roll_No='" + Convert.ToString(dtStuappfilters.Rows[stttype]["Roll_No"]) + "' and stu_type='1'";
                                      DataTable vegstumesstbl = studDetDS.Tables[6].DefaultView.ToTable();
                                      if (vegstumesstbl.Rows.Count > 0)
                                      {
                                          if (stumesstbl.Rows.Count > 0)
                                          {

                                              string rollno = Convert.ToString(dtStuappfilters.Rows[stttype]["roll_no"]);
                                              hs.Add(rollno, val);
                                              string getpresentAbsent = da.GetFunction("select " + ColName + " from attendance where roll_no='" + rollno + "' and month_year='" + MonthYear + "'");

                                              if (getpresentAbsent != "" && getpresentAbsent != "0")
                                              {
                                                  if (getpresentAbsent == "1")
                                                  {
                                                      prsentCount = prsentCount + 1;
                                                      if (prestrollnotag == "")
                                                      {
                                                          prestrollnotag = rollno;
                                                      }
                                                      else
                                                      {
                                                          prestrollnotag = prestrollnotag + "','" + rollno;
                                                      }
                                                  }
                                                  else
                                                  {
                                                      absentCount = absentCount + 1;
                                                      if (absrolnotag == "")
                                                      {
                                                          absrolnotag = rollno;

                                                      }
                                                      else
                                                      {
                                                          absrolnotag = absrolnotag + "','" + rollno;
                                                      }
                                                  }

                                              }
                                          }
                                      }
                                      else
                                      {
                                          string rollno = Convert.ToString(dtStuappfilters.Rows[stttype]["roll_no"]);
                                          hs.Add(rollno, val);
                                          string getpresentAbsent = da.GetFunction("select " + ColName + " from attendance where roll_no='" + rollno + "' and month_year='" + MonthYear + "'");

                                          if (getpresentAbsent != "" && getpresentAbsent != "0")
                                          {
                                              if (getpresentAbsent == "1")
                                              {
                                                  prsentCount = prsentCount + 1;
                                                  if (prestrollnotag == "")
                                                  {
                                                      prestrollnotag = rollno;
                                                  }
                                                  else
                                                  {
                                                      prestrollnotag = prestrollnotag + "','" + rollno;
                                                  }
                                              }
                                              else
                                              {
                                                  absentCount = absentCount + 1;
                                                  if (absrolnotag == "")
                                                  {
                                                      absrolnotag = rollno;

                                                  }
                                                  else
                                                  {
                                                      absrolnotag = absrolnotag + "','" + rollno;
                                                  }
                                              }

                                          }
                                      }

                                    }

                                    }

                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(prsentCount);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(prestrollnotag);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                    if (!totalmode.Contains(Convert.ToString(colcounts)))
                                    {
                                        totalmode.Add(Convert.ToString(colcounts), Convert.ToString(prsentCount));
                                    }
                                    else
                                    {
                                        int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcounts)]);

                                        getvalue = getvalue + prsentCount;
                                        totalmode.Remove(Convert.ToString(colcounts));

                                        totalmode.Add(Convert.ToString(colcounts), Convert.ToString(getvalue));


                                    }


                                    colcounts++;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(absentCount);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(absrolnotag);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;
                                    if (!totalmode.Contains(Convert.ToString(colcounts)))
                                    {
                                        totalmode.Add(Convert.ToString(colcounts), Convert.ToString(absentCount));
                                    }
                                    else
                                    {
                                        int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcounts)]);

                                        getvalue = getvalue + absentCount;
                                        totalmode.Remove(Convert.ToString(colcounts));

                                        totalmode.Add(Convert.ToString(colcounts), Convert.ToString(getvalue));


                                    }

                                }

                                else
                                {
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(prsentCount);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                    colcounts++;

                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(absentCount);
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                }
                            }

                        }

                        if (studDetDS.Tables[5].Rows.Count > 0)
                        {

                            int prsentCount = 0;
                            int absentCount = 0;
                            string prestrollnotag = string.Empty;
                            string absrolnotag = string.Empty;
                            int colCountval = 0;

                            for (int trans = 0; trans < studDetDS.Tables[5].Rows.Count; trans++)
                            {

                                string rollno = Convert.ToString(studDetDS.Tables[5].Rows[trans]["roll_no"]);
                                string getpresentAbsent = da.GetFunction("select " + ColName + " from attendance where roll_no='" + rollno + "' and month_year='" + MonthYear + "'");

                                if (getpresentAbsent != "" && getpresentAbsent != "0")
                                {
                                    if (getpresentAbsent == "1")
                                    {
                                        prsentCount = prsentCount + 1;
                                        if (prestrollnotag == "")
                                        {
                                            prestrollnotag = rollno;
                                        }
                                        else
                                        {
                                            prestrollnotag = prestrollnotag + "','" + rollno;
                                        }
                                    }
                                    else
                                    {
                                        absentCount = absentCount + 1;
                                        if (absrolnotag == "")
                                        {
                                            absrolnotag = rollno;

                                        }
                                        else
                                        {
                                            absrolnotag = absrolnotag + "','" + rollno;
                                        }
                                    }

                                }


                            }

                            colCountval = tcount;
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Text = Convert.ToString(prsentCount);
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Tag = Convert.ToString(prestrollnotag);
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].ForeColor = Color.Black;
                            if (!totalmode.Contains(Convert.ToString(colCountval)))
                            {
                                totalmode.Add(Convert.ToString(colCountval), Convert.ToString(prsentCount));
                            }
                            else
                            {
                                int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colCountval)]);

                                getvalue = getvalue + prsentCount;
                                totalmode.Remove(Convert.ToString(colCountval));

                                totalmode.Add(Convert.ToString(colCountval), Convert.ToString(getvalue));


                            }

                            colCountval++;

                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Text = Convert.ToString(absentCount);
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Tag = Convert.ToString(absrolnotag);
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[rowcount, colCountval].ForeColor = Color.Black;
                            if (!totalmode.Contains(Convert.ToString(colCountval)))
                            {
                                totalmode.Add(Convert.ToString(colCountval), Convert.ToString(absentCount));
                            }
                            else
                            {
                                int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colCountval)]);

                                getvalue = getvalue + absentCount;
                                totalmode.Remove(Convert.ToString(colCountval));

                                totalmode.Add(Convert.ToString(colCountval), Convert.ToString(getvalue));


                            }

                        }

                        rowcount++;

                    }

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 0].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Text = "Total";
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Tag = "Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Font.Bold = true;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Text = Convert.ToString(overalltot);
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Tag = "Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Font.Bold = true;

                    foreach (DictionaryEntry entry in totalmode)
                    {
                        int col = Convert.ToInt32(entry.Key);
                        string getval = Convert.ToString(entry.Value);

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Text = Convert.ToString(getval);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].ForeColor = ColorTranslator.FromHtml("#107532");

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Font.Bold = true;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].BackColor = ColorTranslator.FromHtml("#80EDED");
                    }

                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Degree were found";
                    divPopAlert.Visible = true;
                    return;
                }

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 900;
                Fpspread1.Height = 420;
                Fpspread1.Visible = true;
                //  rptprint.Visible = true;
                //  Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                // Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }

            if (Radioformat2.Checked == true)
            {
                Hashtable totalmode = new Hashtable();
                up_spd1.Visible = false;
                Fpspread2.Visible = false;
                div_report.Visible = false;
                string dept = string.Empty;
                dept = GetSelectedItemsValueAsString(cbl_dept);
                string monyear = string.Empty;
                string fdate = txtDate.Text;
                string[] f_split = fdate.Split(new Char[] { '/' });
                string dateval = f_split[0];
                string monthval = f_split[1];
                string yearval = f_split[2];
                dateval = dateval.TrimStart('0');
                monthval = monthval.TrimStart('0');
                yearval = yearval.TrimStart('0');
                dateval = "[" + dateval + "]";
                monyear = monthval + "/" + yearval;
                int tcount = 0;
                int dcount = 0;
                int overalltot = 0;


                string valDept = string.Empty;
                if (ddlCollege.Items.Count > 0)
                {
                    collegeCode = ddlCollege.SelectedValue.ToString().Trim();
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lblCollege.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }

                if (cbl_dept.Items.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lbldep.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }

                else
                {
                    dept = rs.GetSelectedItemsValueAsString(cbl_dept);
                    if (string.IsNullOrEmpty(dept))
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Select Atleast One " + lbldep.Text + "";
                        divPopAlert.Visible = true;
                        return;
                    }
                }

                string messType = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegeCode + "'";
                DataTable dtStu = dirAcc.selectDataTable(messType);//delsi 0709

                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
                Fpspread1.Sheets[0].ColumnCount = 4;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strength";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Tag = "Total Strength";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostler";


                if (dtStu.Rows.Count > 0)
                {
                    int column = Fpspread1.Sheets[0].ColumnCount;
                    int colcount = 0;
                    for (int i = 0; i < dtStu.Rows.Count; i++)
                    {
                        colcount++;
                        string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                        string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = messName;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = messId;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Hostler" + "," + messId + "," + "P";
                        colcount++;
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Hostler" + "," + messId + "," + "A";

                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 2, 1, dtStu.Rows.Count);
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                    }
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, column - 1, 1, colcount);
                }


                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Day Scholor";
                dcount = Fpspread1.Sheets[0].ColumnCount - 1;
                int count1 = Fpspread1.Sheets[0].ColumnCount;
                if (dtStu.Rows.Count > 0)
                {
                    int countCol = 0;
                    int column = Fpspread1.Sheets[0].ColumnCount;
                    for (int i = 0; i < dtStu.Rows.Count; i++)
                    {
                        countCol++;
                        string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                        string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = messName;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";

                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Day Scholor" + "," + messId + "," + "P";
                        countCol++;
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";

                        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "Day Scholor" + "," + messId + "," + "A";

                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 2, 1, dtStu.Rows.Count);

                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                    }
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count1 - 1, 1, countCol);
                }
                //   Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount - 1;


                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Transport";
                tcount = Fpspread1.Sheets[0].ColumnCount - 1;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, 2, 2);
                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";

                Fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 200;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 14;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Black;
                style2.BackColor = Color.AliceBlue;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.SaveChanges();

                if (cbl_dept.Items.Count > 0)//delsi 0709
                {
                    int row = 0;
                    int columnCount = 0;
                    int rowcount = Fpspread1.Sheets[0].RowCount;
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        if (cbl_dept.Items[i].Selected == true)
                        {
                            row++;
                            string deptCode = Convert.ToString(cbl_dept.Items[i].Value);
                            string deptname = Convert.ToString(cbl_dept.Items[i].Text);

                            Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = row.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Black;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = deptname;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = deptCode;

                            DataSet staffcout = new DataSet();
                            string allstaffcode = string.Empty;
                            string qury = "select m.staff_code from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null)) and m.college_code in('" + ddlCollege.SelectedItem.Value + "') and h.dept_code='" + deptCode + "'";
                            staffcout = da.select_method_wo_parameter(qury, "text");
                            if (staffcout.Tables[0].Rows.Count > 0)
                            {
                                for (int count = 0; count < staffcout.Tables[0].Rows.Count; count++)
                                {
                                    string getstaffCode = Convert.ToString(staffcout.Tables[0].Rows[count]["staff_code"]);
                                    if (allstaffcode == "")
                                    {
                                        allstaffcode = getstaffCode;
                                    }
                                    else
                                    {
                                        allstaffcode = allstaffcode + "','" + getstaffCode;
                                    }

                                }
                            }


                            string overallStudentCount = da.GetFunction("select COUNT(m.staff_code) as totalcount from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null)) and m.college_code in('" + ddlCollege.SelectedItem.Value + "') and h.dept_code='" + deptCode + "'");

                            overalltot = overalltot + (Convert.ToInt32(overallStudentCount));


                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Black;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = overallStudentCount;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(allstaffcode);


                            DataSet studDetDS = new DataSet();

                            string query = "select StudMessType, m.staff_code,m.staff_name,sm.appl_no,sm.appl_id,t.dept_code from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s,HT_HostelRegistration hr,staff_appl_master sm where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null)) and m.college_code in('" + ddlCollege.SelectedItem.Value + "') and t.dept_code in('" + deptCode + "')  and MemType='2' and m.appl_no=sm.appl_no and sm.appl_id=hr.app_No";

                            query += " select distinct StudMessType, m.staff_code,m.staff_name,t.dept_code from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s,DayScholourStaffAdd ds where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null)) and m.college_code in('" + ddlCollege.SelectedItem.Value + "') and t.dept_code in('" + deptCode + "') and ds.Staff_code=m.staff_code";


                            query += " select s.staff_code,s.staff_name,s.Bus_RouteID,s.VehID,s.Boarding,hm.dept_acronym,Seat_No,s.appl_no,s.college_code from staffmaster s,stafftrans st,hrdept_master hm where s.staff_code=st.staff_code and st.dept_code=hm.dept_code and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' and s.college_code=hm.college_code  and isnull(IsCanceledStage,0)<>'1' and s.college_code in('" + ddlCollege.SelectedItem.Value + "') and s.settled <>1 and s.resign <>1 and  st.latestrec<>0  and st.dept_code in('" + deptCode + "')    order by len(Seat_No),Seat_No";

                            studDetDS = da.select_method_wo_parameter(query, "text");


                            if (studDetDS.Tables[0].Rows.Count > 0)
                            {
                                int colcount = 2;
                                for (int vals = 0; vals < dtStu.Rows.Count; vals++)
                                {
                                    int prsentCount = 0;
                                    int absentCount = 0;
                                    string prestrollnotag = string.Empty;
                                    string absrolnotag = string.Empty;
                                    int val = 0;
                                    colcount++;
                                    string messId = Convert.ToString(dtStu.Rows[vals]["StudentType"]);
                                    val = Convert.ToInt32(messId) - 1;
                                    string messName = Convert.ToString(dtStu.Rows[vals]["StudentTypeName"]);
                                    studDetDS.Tables[0].DefaultView.RowFilter = "StudMessType='" + val + "'";
                                    DataTable dtStuappfilters = studDetDS.Tables[0].DefaultView.ToTable();

                                    if (dtStuappfilters.Rows.Count > 0)
                                    {
                                        for (int stttype = 0; stttype < dtStuappfilters.Rows.Count; stttype++)
                                        {
                                            string staffCode = Convert.ToString(dtStuappfilters.Rows[stttype]["staff_code"]);
                                            string getpresentAbsent = da.GetFunction("select " + dateval + " from staff_attnd where staff_code='" + staffCode + "' and mon_year='" + monyear + "'");

                                            if (getpresentAbsent != "" && getpresentAbsent != "0")
                                            {
                                                string[] splitarray = getpresentAbsent.Split('-');
                                                if (splitarray[0].ToString() != "")
                                                {

                                                    if (splitarray[0].ToString().Trim().ToUpper() == "P")
                                                    {
                                                        prsentCount = prsentCount + 1;
                                                        if (prestrollnotag == "")
                                                        {
                                                            prestrollnotag = staffCode;
                                                        }
                                                        else
                                                        {
                                                            prestrollnotag = prestrollnotag + "','" + staffCode;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        absentCount = absentCount + 1;

                                                        if (absrolnotag == "")
                                                        {
                                                            absrolnotag = staffCode;

                                                        }
                                                        else
                                                        {
                                                            absrolnotag = absrolnotag + "','" + staffCode;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(prsentCount);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(prestrollnotag);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;
                                        if (!totalmode.Contains(Convert.ToString(colcount)))
                                        {
                                            totalmode.Add(Convert.ToString(colcount), Convert.ToString(prsentCount));
                                        }
                                        else
                                        {
                                            int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcount)]);

                                            getvalue = getvalue + prsentCount;
                                            totalmode.Remove(Convert.ToString(colcount));

                                            totalmode.Add(Convert.ToString(colcount), Convert.ToString(getvalue));


                                        }


                                        colcount++;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(absentCount);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(absrolnotag);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;

                                        if (!totalmode.Contains(Convert.ToString(colcount)))
                                        {
                                            totalmode.Add(Convert.ToString(colcount), Convert.ToString(absentCount));
                                        }
                                        else
                                        {
                                            int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcount)]);

                                            getvalue = getvalue + absentCount;
                                            totalmode.Remove(Convert.ToString(colcount));

                                            totalmode.Add(Convert.ToString(colcount), Convert.ToString(getvalue));


                                        }


                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(prsentCount);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(prestrollnotag);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                        colcount++;

                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(absentCount);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = Convert.ToString(absrolnotag);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = Color.Black;

                                    }

                                    columnCount = colcount;

                                }


                            }


                            if (studDetDS.Tables[1].Rows.Count > 0)
                            {

                                int colcounts = dcount - 1;
                                for (int vals = 0; vals < dtStu.Rows.Count; vals++)
                                {
                                    int prsentCount = 0;
                                    int absentCount = 0;
                                    string prestrollnotag = string.Empty;
                                    string absrolnotag = string.Empty;
                                    int val = 0;
                                    colcounts++;
                                    string messId = Convert.ToString(dtStu.Rows[vals]["StudentType"]);
                                    val = Convert.ToInt32(messId) - 1;
                                    string messName = Convert.ToString(dtStu.Rows[vals]["StudentTypeName"]);
                                    studDetDS.Tables[1].DefaultView.RowFilter = "StudMessType='" + val + "'";
                                    DataTable dtStuappfilters = studDetDS.Tables[1].DefaultView.ToTable();
                                    if (dtStuappfilters.Rows.Count > 0)
                                    {
                                        for (int stttype = 0; stttype < dtStuappfilters.Rows.Count; stttype++)
                                        {

                                            string staff_code = Convert.ToString(dtStuappfilters.Rows[stttype]["staff_code"]);
                                            string appl_no = da.GetFunction("select select appl_no from staffmaster ");
                                            string getpresentAbsent = da.GetFunction("select " + dateval + " from staff_attnd where staff_code='" + staff_code + "' and mon_year='" + monyear + "'");

                                            if (getpresentAbsent != "" && getpresentAbsent != "0")
                                            {
                                                string[] splitarray = getpresentAbsent.Split('-');
                                                if (splitarray[0].ToString() != "")
                                                {

                                                    if (splitarray[0].ToString().Trim().ToUpper() == "P")
                                                    {
                                                        prsentCount = prsentCount + 1;

                                                        if (prestrollnotag == "")
                                                        {
                                                            prestrollnotag = staff_code;
                                                        }
                                                        else
                                                        {
                                                            prestrollnotag = prestrollnotag + "','" + staff_code;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        absentCount = absentCount + 1;

                                                        if (absrolnotag == "")
                                                        {
                                                            absrolnotag = staff_code;

                                                        }
                                                        else
                                                        {
                                                            absrolnotag = absrolnotag + "','" + staff_code;
                                                        }
                                                    }
                                                }

                                            }

                                        }

                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(prsentCount);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(prestrollnotag);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;
                                        if (!totalmode.Contains(Convert.ToString(colcounts)))
                                        {
                                            totalmode.Add(Convert.ToString(colcounts), Convert.ToString(prsentCount));
                                        }
                                        else
                                        {
                                            int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcounts)]);

                                            getvalue = getvalue + prsentCount;
                                            totalmode.Remove(Convert.ToString(colcounts));

                                            totalmode.Add(Convert.ToString(colcounts), Convert.ToString(getvalue));


                                        }
                                        colcounts++;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(absentCount);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(absrolnotag);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                        if (!totalmode.Contains(Convert.ToString(colcounts)))
                                        {
                                            totalmode.Add(Convert.ToString(colcounts), Convert.ToString(absentCount));
                                        }
                                        else
                                        {
                                            int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colcounts)]);

                                            getvalue = getvalue + absentCount;
                                            totalmode.Remove(Convert.ToString(colcounts));

                                            totalmode.Add(Convert.ToString(colcounts), Convert.ToString(getvalue));


                                        }

                                    }

                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(prsentCount);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(prestrollnotag);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                        colcounts++;

                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Text = Convert.ToString(absentCount);

                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Tag = Convert.ToString(absrolnotag);
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[rowcount, colcounts].ForeColor = Color.Black;

                                    }
                                }

                            }
                            if (studDetDS.Tables[2].Rows.Count > 0)
                            {
                                int prsentCount = 0;
                                int absentCount = 0;
                                string prestrollnotag = string.Empty;
                                string absrolnotag = string.Empty;
                                int colCountval = 0;
                                colCountval = tcount;
                                for (int tstaf = 0; tstaf < studDetDS.Tables[2].Rows.Count; tstaf++)
                                {

                                    string staff_code = Convert.ToString(studDetDS.Tables[2].Rows[tstaf]["staff_code"]);
                                    string appl_no = da.GetFunction("select select appl_no from staffmaster ");
                                    string getpresentAbsent = da.GetFunction("select " + dateval + " from staff_attnd where staff_code='" + staff_code + "' and mon_year='" + monyear + "'");

                                    if (getpresentAbsent != "" && getpresentAbsent != "0")
                                    {
                                        string[] splitarray = getpresentAbsent.Split('-');
                                        if (splitarray[0].ToString() != "")
                                        {

                                            if (splitarray[0].ToString().Trim().ToUpper() == "P")
                                            {
                                                prsentCount = prsentCount + 1;

                                                if (prestrollnotag == "")
                                                {
                                                    prestrollnotag = staff_code;
                                                }
                                                else
                                                {
                                                    prestrollnotag = prestrollnotag + "','" + staff_code;
                                                }
                                            }
                                            else
                                            {
                                                absentCount = absentCount + 1;

                                                if (absrolnotag == "")
                                                {
                                                    absrolnotag = staff_code;

                                                }
                                                else
                                                {
                                                    absrolnotag = absrolnotag + "','" + staff_code;
                                                }
                                            }
                                        }

                                    }


                                }
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Text = Convert.ToString(prsentCount);
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Tag = Convert.ToString(prestrollnotag);
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].ForeColor = Color.Black;
                                if (!totalmode.Contains(Convert.ToString(colCountval)))
                                {
                                    totalmode.Add(Convert.ToString(colCountval), Convert.ToString(prsentCount));
                                }
                                else
                                {
                                    int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colCountval)]);

                                    getvalue = getvalue + prsentCount;
                                    totalmode.Remove(Convert.ToString(colCountval));

                                    totalmode.Add(Convert.ToString(colCountval), Convert.ToString(getvalue));


                                }

                                colCountval++;
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Text = Convert.ToString(absentCount);
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Tag = Convert.ToString(absrolnotag);
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[rowcount, colCountval].ForeColor = Color.Black;
                                if (!totalmode.Contains(Convert.ToString(colCountval)))
                                {
                                    totalmode.Add(Convert.ToString(colCountval), Convert.ToString(absentCount));
                                }
                                else
                                {
                                    int getvalue = Convert.ToInt32(totalmode[Convert.ToString(colCountval)]);

                                    getvalue = getvalue + absentCount;
                                    totalmode.Remove(Convert.ToString(colCountval));

                                    totalmode.Add(Convert.ToString(colCountval), Convert.ToString(getvalue));


                                }

                            }


                            rowcount++;

                        }

                    }
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 0].BackColor = ColorTranslator.FromHtml("#80EDED");

                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 1].Text = "Total";
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 1].Tag = "Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 1].Font.Bold = true;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 1);

                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Text = Convert.ToString(overalltot);
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Tag = "Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].BackColor = ColorTranslator.FromHtml("#80EDED");
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 2].Font.Bold = true;

                    foreach (DictionaryEntry entry in totalmode)
                    {
                        int col = Convert.ToInt32(entry.Key);
                        string getval = Convert.ToString(entry.Value);

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Text = Convert.ToString(getval);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].ForeColor = ColorTranslator.FromHtml("#107532");

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Font.Bold = true;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].BackColor = ColorTranslator.FromHtml("#80EDED");
                    }


                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Select Any Department";
                    divPopAlert.Visible = true;
                    return;
                }

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 900;
                Fpspread1.Height = 420;
                Fpspread1.Visible = true;
            }

            if (rdbguest.Checked == true)//delsireff
            {
                if (ddlCollege.Items.Count > 0)
                {
                    collegeCode = ddlCollege.SelectedValue.ToString().Trim();
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No " + lblCollege.Text + " Found";
                    divPopAlert.Visible = true;
                    return;
                }
                string monyear = string.Empty;
                string fdate = txtDate.Text;
                string[] f_split = fdate.Split(new Char[] { '/' });
                string dateval = f_split[0];
                string monthval = f_split[1];
                string yearval = f_split[2];
                dateval = dateval.TrimStart('0');
                monthval = monthval.TrimStart('0');
                yearval = yearval.TrimStart('0');
                dateval = "D" + dateval + "";
                //   monyear = monthval + "/" + yearval;


                string messType = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegeCode + "'";
                DataTable dtStu = dirAcc.selectDataTable(messType);//delsi 0709

                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 2;
                Fpspread1.Sheets[0].ColumnCount = 2;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;


                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Total Strength";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);


                if (dtStu.Rows.Count > 0)
                {
                    int column = Fpspread1.Sheets[0].ColumnCount;
                    int colcount = 0;

                    for (int i = 0; i < dtStu.Rows.Count; i++)
                    {

                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        colcount++;
                        string messId = Convert.ToString(dtStu.Rows[i]["StudentType"]);
                        string messName = Convert.ToString(dtStu.Rows[i]["StudentTypeName"]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = messName;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = messId;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "p";
                        colcount++;
                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "A";

                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 2, 1, dtStu.Rows.Count);


                    }

                    DataSet studDetDS = new DataSet();
                    studDetDS.Clear();
                    studDetDS.Reset();

                    string query = "select StudMessType,CONVERT(nvarchar, Vi.VendorContactPK) as Code,h.id,Vi.VenContactName as Name,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,B.Building_Name,V.VendorAddress as discription from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No order by code asc";

                    studDetDS = da.select_method_wo_parameter(query, "text");//delsi 1007
                    int sno = 0;
                    if (studDetDS.Tables[0].Rows.Count > 0)
                    {
                        string allguesttot = string.Empty;
                        sno++;
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();

                        string getStrength = da.GetFunction("select Count(Vi.VendorContactPK) as Totelstrength from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No");

                        for (int val = 0; val < studDetDS.Tables[0].Rows.Count; val++)
                        {
                            string getcode = Convert.ToString(studDetDS.Tables[0].Rows[val]["Code"]);
                            if (allguesttot == "")
                            {
                                allguesttot = getcode;
                            }
                            else
                            {
                                allguesttot = allguesttot + "','" + getcode;
                            }

                        }


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Black;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(getStrength);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(allguesttot);


                        int colcountss = 2;
                        for (int vals = 0; vals < dtStu.Rows.Count; vals++)
                        {
                            int prsentCount = 0;
                            int absentCount = 0;
                            string prestrollnotag = string.Empty;
                            string absrolnotag = string.Empty;
                            int val = 0;
                            colcount++;
                            string messId = Convert.ToString(dtStu.Rows[vals]["StudentType"]);
                            val = Convert.ToInt32(messId) - 1;
                            string messName = Convert.ToString(dtStu.Rows[vals]["StudentTypeName"]);
                            studDetDS.Tables[0].DefaultView.RowFilter = "StudMessType='" + val + "'";
                            DataTable dtStuappfilters = studDetDS.Tables[0].DefaultView.ToTable();

                            if (dtStuappfilters.Rows.Count > 0)
                            {
                                for (int stttype = 0; stttype < dtStuappfilters.Rows.Count; stttype++)
                                {
                                    string rollno = Convert.ToString(dtStuappfilters.Rows[stttype]["Code"]);
                                    string getpresentAbsent = da.GetFunction("select " + dateval + " from HT_Attendance where AttnMonth='" + monthval + "' and AttnYear='" + yearval + "' and App_No='" + rollno + "'");

                                    if (getpresentAbsent != "" && getpresentAbsent != "0")
                                    {
                                        if (getpresentAbsent == "1")
                                        {
                                            prsentCount = prsentCount + 1;
                                            if (prestrollnotag == "")
                                            {
                                                prestrollnotag = rollno;
                                            }
                                            else
                                            {
                                                prestrollnotag = prestrollnotag + "','" + rollno;
                                            }
                                        }
                                        else
                                        {
                                            absentCount = absentCount + 1;
                                            if (absrolnotag == "")
                                            {
                                                absrolnotag = rollno;

                                            }
                                            else
                                            {
                                                absrolnotag = absrolnotag + "','" + rollno;
                                            }
                                        }

                                    }
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Text = Convert.ToString(prsentCount);

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Tag = Convert.ToString(prestrollnotag);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].ForeColor = Color.Black;
                                colcountss++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Text = Convert.ToString(absentCount);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Tag = Convert.ToString(absrolnotag);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].ForeColor = Color.Black;
                                colcountss++;

                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Text = Convert.ToString(prsentCount);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Tag = Convert.ToString(prestrollnotag);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].HorizontalAlign = HorizontalAlign.Center;
                                colcountss++;

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Text = Convert.ToString(absentCount);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Tag = Convert.ToString(absrolnotag);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcountss].ForeColor = Color.Black;

                            }
                        }

                    }
                    else
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Record Found";
                        divPopAlert.Visible = true;
                        return;
                    }

                }

                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Add Mess Type";
                    divPopAlert.Visible = true;
                    return;
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 900;
                Fpspread1.Height = 420;
                Fpspread1.Visible = true;

            }


        }
        catch
        {
        }
    }

    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string newcollcode = Convert.ToString(ddlCollege.SelectedItem.Value);
            string item = "select dept_code,dept_name from hrdept_master where college_code='" + newcollcode + "' order by dept_name";
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + newcollcode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + newcollcode + "') order by dept_name";
            }

            ds = da.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "dept_name";
                cbl_dept.DataValueField = "dept_code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }
            }
            else
            {
                txt_dept.Text = "--Select--";
                cb_dept.Checked = false;
            }
        }
        catch { }
    }



    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }

    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        if (Radioformat1.Checked == true)
        {
            poppernew.Visible = true;
            load();
            lb_column1.Items.Clear();
            columnordertype();
        }
        if (Radioformat2.Checked == true)
        {
            poppernew.Visible = true;
            loadstaff();
            lb_column1.Items.Clear();
            columnordertype();

        }
        if (rdbguest.Checked == true)
        {
            poppernew.Visible = true;
            loadguest();
            lb_column1.Items.Clear();
            columnordertype();

        }
    }

    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "54"));
        lb_selectcolumn.Items.Add(new ListItem("Roll No", "55"));
        lb_selectcolumn.Items.Add(new ListItem("Reg No", "57"));
        lb_selectcolumn.Items.Add(new ListItem("Admission No", "58"));
        // lb_selectcolumn.Items.Add(new ListItem("Application No", "59"));

        lb_selectcolumn.Items.Add(new ListItem("Batch", "3"));
        lb_selectcolumn.Items.Add(new ListItem("Degree", "1"));
        lb_selectcolumn.Items.Add(new ListItem("Branch", "2"));
        lb_selectcolumn.Items.Add(new ListItem("Semester", "4"));
        lb_selectcolumn.Items.Add(new ListItem("Section", "60"));
        lb_selectcolumn.Items.Add(new ListItem("SeatType", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Student Type", "63"));
        //  lb_selectcolumn.Items.Add(new ListItem("HostelName", "34"));
        //30.07.16

        lb_selectcolumn.Items.Add(new ListItem("Boarding", "122"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Id", "123"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "61"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "62"));
        lb_selectcolumn.Items.Add(new ListItem("Father Name", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Father Income", "84"));
        lb_selectcolumn.Items.Add(new ListItem("Father Occupation", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Father Mob No", "85"));
        lb_selectcolumn.Items.Add(new ListItem("Father Email Id", "86"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Name", "87"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Income", "88"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Occupation", "96"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Mob No", "89"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Email Id", "90"));
        // lb_selectcolumn.Items.Add(new ListItem("Guardian Name", "91"));
        // lb_selectcolumn.Items.Add(new ListItem("Guardian Email Id", "92"));
        //lb_selectcolumn.Items.Add(new ListItem("Guardian Mob No", "93"));
        //lb_selectcolumn.Items.Add(new ListItem("Place Of Birth", "94"));
        lb_selectcolumn.Items.Add(new ListItem("Adhaar Card No", "95"));
        //  lb_selectcolumn.Items.Add(new ListItem("Voter ID", "35"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Sub Caste", "83"));
        lb_selectcolumn.Items.Add(new ListItem("Citizen", "10"));
        //lb_selectcolumn.Items.Add(new ListItem("TamilOrginFromAndaman", "13"));
        //  lb_selectcolumn.Items.Add(new ListItem("Ex-serviceman", "64"));
        //  lb_selectcolumn.Items.Add(new ListItem("Rank", "74"));
        // lb_selectcolumn.Items.Add(new ListItem("Place", "75"));
        //lb_selectcolumn.Items.Add(new ListItem("Number", "76"));
        //lb_selectcolumn.Items.Add(new ListItem("IsDisable", "53"));
        //  lb_selectcolumn.Items.Add(new ListItem("VisualHandy", "14"));
      //  lb_selectcolumn.Items.Add(new ListItem("Residency", "48"));
       // lb_selectcolumn.Items.Add(new ListItem("Physically challange", "49"));
        //lb_selectcolumn.Items.Add(new ListItem("Learning Disability", "51"));
        //lb_selectcolumn.Items.Add(new ListItem("Other Disability", "52"));
        //lb_selectcolumn.Items.Add(new ListItem("Sports", "50"));
        //lb_selectcolumn.Items.Add(new ListItem("First Graduate", "15"));
        //lb_selectcolumn.Items.Add(new ListItem("MissionaryChild", "26"));
        //lb_selectcolumn.Items.Add(new ListItem("missionarydisc", "27"));
        //lb_selectcolumn.Items.Add(new ListItem("Hostel accommodation", "65"));
        //lb_selectcolumn.Items.Add(new ListItem("Blood Donor", "66"));
       // lb_selectcolumn.Items.Add(new ListItem("Reserved Caste", "67"));
       // lb_selectcolumn.Items.Add(new ListItem("Economic Backward", "68"));
       // lb_selectcolumn.Items.Add(new ListItem("Parents Old Student", "69"));
       // lb_selectcolumn.Items.Add(new ListItem("Driving License", "70"));
        //lb_selectcolumn.Items.Add(new ListItem("License No", "71"));
        //lb_selectcolumn.Items.Add(new ListItem("Tuition Fee Waiver", "72"));
        //lb_selectcolumn.Items.Add(new ListItem("Insurance", "73"));
        //lb_selectcolumn.Items.Add(new ListItem("Insurance Amount", "77"));
        //lb_selectcolumn.Items.Add(new ListItem("Insurance InsBy", "78"));
        //lb_selectcolumn.Items.Add(new ListItem("Insurance Nominee", "79"));
       // lb_selectcolumn.Items.Add(new ListItem("Insurance NominRelation", "80"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Street", "19"));
        lb_selectcolumn.Items.Add(new ListItem("City", "20"));
        lb_selectcolumn.Items.Add(new ListItem("State", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Country", "22"));
      //  lb_selectcolumn.Items.Add(new ListItem("PinCode", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "108"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Street", "109"));
        lb_selectcolumn.Items.Add(new ListItem("Communication City", "110"));
        lb_selectcolumn.Items.Add(new ListItem("Communication State", "111"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Country", "112"));
       // lb_selectcolumn.Items.Add(new ListItem("Communication PinCode", "113"));
        lb_selectcolumn.Items.Add(new ListItem("Student Mobile", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Alternate Mob No", "82"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailId", "56"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Phone No", "25"));


     //   lb_selectcolumn.Items.Add(new ListItem("Relative Name", "119"));
     //   lb_selectcolumn.Items.Add(new ListItem("RelationShip", "120"));
      //  lb_selectcolumn.Items.Add(new ListItem("Student/Staff", "121"));
        //lb_selectcolumn.Items.Add(new ListItem("Admission Date", "36"));
      //  lb_selectcolumn.Items.Add(new ListItem("Enrollment Date", "37"));
        lb_selectcolumn.Items.Add(new ListItem("Join Date", "38"));


        lb_selectcolumn.Items.Add(new ListItem("Refered By", "127"));
        //string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(query, "text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
        //    {
        //        lb_selectcolumn.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(ds.Tables[0].Rows[y]["MasterValue"]), Convert.ToString(ds.Tables[0].Rows[y]["MasterCode"])));
        //    }
        //}
    }

    public void loadstaff()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Staff Code", "1"));//staff_code

        lb_selectcolumn.Items.Add(new ListItem("Staff Name", "2"));//staff_name
        lb_selectcolumn.Items.Add(new ListItem("Appl No", "3"));//appl_no
        lb_selectcolumn.Items.Add(new ListItem("Department", "4"));//dept_name
        lb_selectcolumn.Items.Add(new ListItem("Designation", "5"));//desig_name
        lb_selectcolumn.Items.Add(new ListItem("Staff Type", "6"));//staff_type
        lb_selectcolumn.Items.Add(new ListItem("DOB", "7"));//date_of_birth
        lb_selectcolumn.Items.Add(new ListItem("Date Of Join", "8"));//exp_joindate
        lb_selectcolumn.Items.Add(new ListItem("Gender", "9"));//sex
        lb_selectcolumn.Items.Add(new ListItem("Caste", "10"));//Caste
        lb_selectcolumn.Items.Add(new ListItem("Religion", "11"));//religion
        lb_selectcolumn.Items.Add(new ListItem("Community", "12"));//Community
        lb_selectcolumn.Items.Add(new ListItem("Marital Status", "13"));//martial_status
        lb_selectcolumn.Items.Add(new ListItem("Date Of Apply", "14"));//dateofapply
        lb_selectcolumn.Items.Add(new ListItem("Email", "15"));//email
        lb_selectcolumn.Items.Add(new ListItem("Phone No", "16"));//Per_MobileNo
        lb_selectcolumn.Items.Add(new ListItem("Nationality", "17"));//Nationality

        lb_selectcolumn.Items.Add(new ListItem("Experience", "18"));//yofexp
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "19"));//bldgrp
        lb_selectcolumn.Items.Add(new ListItem("Adhar No", "20"));//adharcardno
        lb_selectcolumn.Items.Add(new ListItem("PAN No", "21"));//PANGIRNumber
        lb_selectcolumn.Items.Add(new ListItem("Appointment FT/PT", "22"));//StfNature


    }

    public void loadguest()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Guest Code", "1"));

        lb_selectcolumn.Items.Add(new ListItem("Guest Name", "2"));
        lb_selectcolumn.Items.Add(new ListItem("Floor Name", "3"));
        lb_selectcolumn.Items.Add(new ListItem("Room Name", "4"));
        lb_selectcolumn.Items.Add(new ListItem("Building Name", "5"));


    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        viewcolumorder();
    }
    public void viewcolumorder()
    {
        try
        {
            lb_column1.Items.Clear();
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code='" + ddlCollege.SelectedItem.Value + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext();
                        lb_column1.Items.Add(new System.Web.UI.WebControls.ListItem(loadval, Convert.ToString(sp[y])));
                    }
                }
            }
        }
        catch
        {
        }
    }


    public void loadtext()//delsii
    {
        if (colval == "1")
        {
            loadval = "Course";
            printval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = "Department";
            printval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch";
            printval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = "Semester";
            printval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "Parent Name";
            printval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "DOB";
            printval = "dob";
        }
        if (colval == "7")
        {
            loadval = "Parent Occupation";
            printval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
            printval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
            printval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
            printval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
            printval = "community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
            printval = "caste";
        }

        if (colval == "14")
        {
            loadval = "VisualHandy";
            printval = "visualhandy";
        }

        if (colval == "16")
        {
            loadval = "SeatType";
            printval = "seattype";
        }

        if (colval == "18")
        {
            loadval = "Address";
            printval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Street";
            printval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "City";
            printval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "State";
            printval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Country";
            printval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student Mobile";
            printval = "Student_Mobile";
        }

        if (colval == "25")
        {
            loadval = "Parent Phone No";
            printval = "parent_phnop";
        }



        if (colval == "54")
        {
            loadval = "Student Name";
            printval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll No";
            printval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "Student EmailId";
            printval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "Reg No";
            printval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "Admission No";
            printval = "roll_admit";
        }

        if (colval == "60")
        {
            loadval = "Section";
            printval = "sections";
        }
        if (colval == "61")
        {
            loadval = "Gender";
            printval = "sex";
        }
        if (colval == "62")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "Student Type";
            printval = "stud_type";
        }


        if (colval == "82")
        {
            loadval = "Alternate Mob No";
            printval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "Sub Caste";
            printval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "Father Income";
            printval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "Father Mob No";
            printval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "Father EmailId";
            printval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "Mother";
            printval = "mother";
        }
        if (colval == "88")
        {
            loadval = "Mother Income";
            printval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "Mother Mob No";
            printval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "Mother EmailId";
            printval = "emailM";
        }


        if (colval == "95")
        {
            loadval = "Adhaar Card No";
            printval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "Mother Occupation";
            printval = "motherocc";
        }


        if (colval == "108")
        {
            loadval = "Communication Address";
            printval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Communication Street";
            printval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "Communication City";
            printval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "Communication State";
            printval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Communication Country";
            printval = "Countryc";
        }

        if (colval == "122")
        {
            printval = "Boarding";
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            printval = "vehid";
            loadval = "Vehicle Id";
        }

        if (colval == "38")
        {
            printval = "Adm_Date";
            loadval = "Join Date";
        }


        if (colval == "127")//added
        {
            printval = "referby";
            loadval = "Refered By";
        }

        //if (Convert.ToInt32(colval) > 127)
        //{
        //    loadval = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        //    printval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        //}
    }

    public void loadtextstaff()//delsij
    {

        if (colval == "1")
        {
            loadval = "Staff Code";
            printval = "staff_code";
        }
        if (colval == "2")
        {
            loadval = "Staff Name";
            printval = "appl_name";

        }
        if (colval == "3")
        {
            loadval = "Appl No";
            printval = "appl_id";

        }
        if (colval == "4")
        {
            loadval = "Department";
            printval = "dept_name";
        }
        if (colval == "5")
        {
            loadval = "Designation";
            printval = "desig_name";
        }
        if (colval == "6")
        {
            loadval = "Staff Type";
            printval = "stftype";

        }
        if (colval == "7")
        {
            loadval = "DOB";
            printval = "date_of_birth";
        }
        if (colval == "8")
        {
            loadval = "Date Of Join";
            printval = "join_date";

        }
        if (colval == "9")
        {
            loadval = "Gender";
            printval = "sex";
        }
        if (colval == "10")
        {
            loadval = "Caste";
            printval = "caste";

        }
        if (colval == "11")
        {

            loadval = "Religion";
            printval = "religion";
        }

        if (colval == "12")
        {

            loadval = "Community";
            printval = "Community";
        }
        if (colval == "13")
        {

            loadval = "Marital Status";
            printval = "martial_status";
        }


        if (colval == "14")
        {

            loadval = "Date Of Apply";
            printval = "join_date";
        }
        if (colval == "15")
        {
            loadval = "Email";
            printval = "email";

        }
        if (colval == "16")
        {
            loadval = "Phone No";
            printval = "Per_MobileNo";
        }
        if (colval == "17")
        {
            loadval = "Nationality";
            printval = "Nationality";
        }

        if (colval == "18")
        {
            loadval = "Experience";
            printval = "yofexp";
        }

        if (colval == "19")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "20")
        {
            loadval = "Adhar No";
            printval = "adharcardno";
        }
        if (colval == "21")
        {
            loadval = "PAN No";
            printval = "PANGIRNumber";
        }

        if (colval == "22")
        {
            loadval = "Appointment FT/PT";
            printval = "StfNature";
        }


    }

    public void loadtextguest()//delsij
    {

        if (colval == "1")
        {
            loadval = "Guest Code";
            printval = "VendorContactPK";
        }
        if (colval == "2")
        {
            loadval = "Guest Name";
            printval = "Name";

        }
        if (colval == "3")
        {
            loadval = "Floor Name";
            printval = "Floor_Name";

        }
        if (colval == "4")
        {
            loadval = "Room Name";
            printval = "Room_Name";
        }
        if (colval == "5")
        {
            loadval = "Building Name";
            printval = "Building_Name";
        }
    }


    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (Radioformat1.Checked == true)
        {
            if (ddl_coltypeadd.SelectedIndex == -1)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }
            else if (ddl_coltypeadd.SelectedIndex == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Select any record";
            }
            else if (ddl_coltypeadd.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='PresentAbsentReport' and CollegeCode='" + ddlCollege.SelectedItem.Value + "' ";
                int delete = da.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Deleted Sucessfully";
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No records found";
                }
                columnordertype();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }
        }
        if (Radioformat2.Checked == true)
        {
            if (ddl_coltypeadd.SelectedIndex == -1)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }
            else if (ddl_coltypeadd.SelectedIndex == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Select any record";
            }
            else if (ddl_coltypeadd.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='PresentAbsentReportStaff' and CollegeCode='" + ddlCollege.SelectedItem.Value + "' ";
                int delete = da.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Deleted Sucessfully";
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No records found";
                }
                columnordertype();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }

        }

        if (rdbguest.Checked == true)
        {
            if (ddl_coltypeadd.SelectedIndex == -1)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }
            else if (ddl_coltypeadd.SelectedIndex == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Select any record";
            }
            else if (ddl_coltypeadd.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='PresentAbsentReportGuest' and CollegeCode='" + ddlCollege.SelectedItem.Value + "' ";
                int delete = da.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Deleted Sucessfully";
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No records found";
                }
                columnordertype();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }

        }
    }


    public void columnordertype()
    {
        if (Radioformat1.Checked == true)
        {

            ddl_colord.Items.Clear();
            ddl_coltypeadd.Items.Clear();
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReport' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_colord.DataSource = ds;
                ddl_colord.DataTextField = "MasterValue";
                ddl_colord.DataValueField = "MasterCode";
                ddl_colord.DataBind();
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "MasterValue";
                ddl_coltypeadd.DataValueField = "MasterCode";
                ddl_coltypeadd.DataBind();
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
        if (Radioformat2.Checked == true)
        {
            ddl_colord.Items.Clear();
            ddl_coltypeadd.Items.Clear();
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReportStaff' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";

            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_colord.DataSource = ds;
                ddl_colord.DataTextField = "MasterValue";
                ddl_colord.DataValueField = "MasterCode";
                ddl_colord.DataBind();
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "MasterValue";
                ddl_coltypeadd.DataValueField = "MasterCode";
                ddl_coltypeadd.DataBind();
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));

            }

        }

        if (rdbguest.Checked == true)
        {
            ddl_colord.Items.Clear();
            ddl_coltypeadd.Items.Clear();
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='PresentAbsentReportGuest' and CollegeCode='" + ddlCollege.SelectedItem.Value + "'";

            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_colord.DataSource = ds;
                ddl_colord.DataTextField = "MasterValue";
                ddl_colord.DataValueField = "MasterCode";
                ddl_colord.DataBind();
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "MasterValue";
                ddl_coltypeadd.DataValueField = "MasterCode";
                ddl_coltypeadd.DataBind();
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
                ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));

            }

        }

    }

    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selectcolumn.Items.Count > 0 && lb_selectcolumn.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_column1.Items.Count; j++)
                {
                    if (lb_column1.Items[j].Value == lb_selectcolumn.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selectcolumn.SelectedItem.Text, lb_selectcolumn.SelectedItem.Value);
                    lb_column1.Items.Add(lst);
                }
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }

    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            if (lb_selectcolumn.Items.Count > 0)
            {
                for (int j = 0; j < lb_selectcolumn.Items.Count; j++)
                {
                    lb_column1.Items.Add(new ListItem(lb_selectcolumn.Items[j].Text.ToString(), lb_selectcolumn.Items[j].Value.ToString()));
                }
            }
            lb_selectcolumn.Items.Clear();
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); 
        }
    }

    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_column1.Items.Count > 0 && lb_column1.SelectedItem.Value != "")
            {
                lb_column1.Items.RemoveAt(lb_column1.SelectedIndex);
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); 
        }
    }

    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            load();
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); 

        }
    }

    protected void btnok_click(object sender, EventArgs e)
    {
        if (Radioformat1.Checked == true)
        {
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                if (lb_column1.Items.Count > 0)
                {
                    poppernew.Visible = false;
                    savecolumnorder();
                    //if (savecolumnoder == "")
                    //{
                    //    fpspread1go1();
                    //}
                    //else
                    //{
                    //    if (rdb_cumm.Checked == true)
                    //    {
                    //        go();
                    //    }
                    //    else
                    //    {
                    //        fpspread1go1();
                    //    }
                    //    savecolumnoder = string.Empty;
                    //}
                    //lblalerterr.Visible = false;
                }
                else
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please select atleast one colunm then proceed!";
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Report Type";
            }
        }
        if (Radioformat2.Checked == true)
        {
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                if (lb_column1.Items.Count > 0)
                {
                    poppernew.Visible = false;
                    savecolumnorderstaff();
                }
                else
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please select atleast one colunm then proceed!";
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Report Type";
            }

        }
        if (rdbguest.Checked == true)
        {
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                if (lb_column1.Items.Count > 0)
                {
                    poppernew.Visible = false;
                    savecolumnorderguest();
                }
                else
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please select atleast one colunm then proceed!";
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Report Type";
            }

        }
    }


    public void savecolumnorder()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlCollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + userCode + "','" + ddlCollege.SelectedItem.Value + "')";
        int clsupdate = da.update_method_wo_parameter(clsinsert, "Text");
    }

    public void savecolumnorderstaff()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlCollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + userCode + "','" + ddlCollege.SelectedItem.Value + "')";
        int clsupdate = da.update_method_wo_parameter(clsinsert, "Text");
    }

    public void savecolumnorderguest()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlCollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + userCode + "','" + ddlCollege.SelectedItem.Value + "')";
        int clsupdate = da.update_method_wo_parameter(clsinsert, "Text");
    }


    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }

    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (Radioformat1.Checked == true)
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReport' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReport' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','PresentAbsentReport','" + ddlCollege.SelectedItem.Value + "')";
                int insert = da.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Added sucessfully";
                    txt_description11.Text = string.Empty;
                    //imgdiv33.Visible = false;           
                }
            }
            else
            {
                divPopAlert.Visible = true;
                //  pnl2.Visible = true;
                lblAlertMsg.Text = "Enter the description";
            }
            columnordertype();
        }
        if (Radioformat2.Checked == true)//delsi11
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReportStaff' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReportStaff' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','PresentAbsentReportStaff','" + ddlCollege.SelectedItem.Value + "')";
                int insert = da.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Added sucessfully";
                    txt_description11.Text = string.Empty;
                    //imgdiv33.Visible = false;           
                }
            }
            else
            {
                divPopAlert.Visible = true;
                //  pnl2.Visible = true;
                lblAlertMsg.Text = "Enter the description";
            }
            columnordertype();

        }


        if (rdbguest.Checked == true)//delsi11
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReportGuest' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='PresentAbsentReportGuest' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','PresentAbsentReportGuest','" + ddlCollege.SelectedItem.Value + "')";
                int insert = da.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Added sucessfully";
                    txt_description11.Text = string.Empty;
                    //imgdiv33.Visible = false;           
                }
            }
            else
            {
                divPopAlert.Visible = true;
                //  pnl2.Visible = true;
                lblAlertMsg.Text = "Enter the description";
            }
            columnordertype();

        }

    }

    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }


    public void fpspread1go1()
    {
        try
        {
            RollAndRegSettings();
            string orderStr = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (orderStr == "0")
            {
                if (roll == 0)
                    orderStr = " Order by roll_no,reg_no,roll_admit ";
                else if (roll == 1)
                    orderStr = " Order by roll_no,reg_no,roll_admit ";
                else if (roll == 2)
                    orderStr = " Order by roll_no ";
                else if (roll == 3)
                    orderStr = " Order by reg_no ";
                else if (roll == 4)
                    orderStr = " Order by roll_admit ";
                else if (roll == 5)
                    orderStr = " Order by roll_no,reg_no ";
                else if (roll == 6)
                    orderStr = " Order by reg_no,roll_admit ";
                else if (roll == 7)
                    orderStr = " Order by roll_no,roll_admit ";
            }
            else
            {
                if (orderStr == "0")
                    orderStr = "ORDER BY r.Roll_No";
                else if (orderStr == "1")
                    orderStr = "ORDER BY r.Reg_No";
                else if (orderStr == "2")
                    orderStr = "ORDER BY r.Stud_Name";
                else if (orderStr == "0,1,2")
                    orderStr = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                else if (orderStr == "0,1")
                    orderStr = "ORDER BY r.Roll_No,r.Reg_No";
                else if (orderStr == "1,2")
                    orderStr = "ORDER BY r.Reg_No,r.Stud_Name";
                else if (orderStr == "0,2")
                    orderStr = "ORDER BY r.Roll_No,r.Stud_Name";
            }
            // lbl_headernamespd2.Visible = true;
            //btn_viewsprd2.Visible = true;
            // lnk_admisstionformbtn.Visible = true;
            // img_settingpdf.Visible = true;
            Fpspread2.Visible = true;

            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;
            string boards = string.Empty;
            string states = string.Empty;
            int val = 0;
            int count = 0;
            int count1 = 0;
            int i = 0;
            string header = string.Empty;
            string actval = string.Empty;
            string rollval = string.Empty;
            string sectionvalue = string.Empty;
            string headertype1 = string.Empty;
            string headertype = string.Empty;
            activerow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            activecol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            string sec_textvalue = string.Empty;

            string Batch_tagvalue = string.Empty;
            string dept_tagvalue = string.Empty;
            string course_tagvalue = string.Empty;

            //  Batch_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            course_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            selectcolumnload();

            actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
            rollval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);//delsi 1007
            if (rollval == "" || rollval == "0")
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                lblAlertMsg.Visible = true;
                Fpspread2.Visible = false;
                div_report.Visible = false;
                return;

            }

            headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(2), Convert.ToInt32(activecol)].Tag);
            headertype1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(2), Convert.ToInt32(activecol)].Text);
            string dayscholerHostler = string.Empty;
            string memtype = string.Empty;
            string presentabs = string.Empty;

            if (headertype.Contains(','))
            {
                string[] splitval = headertype.Split(',');
                dayscholerHostler = Convert.ToString(splitval[0]);
                memtype = Convert.ToString(splitval[1]);
                presentabs = Convert.ToString(splitval[2]);

            }
            string query = string.Empty;

        //   query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,Dt.Dept_Name,StuPer_Id,parentF_Mobile,CONVERT(VARCHAR(11),dob,103) as dob ,case when sex='0' then 'Male' else 'Female' end as sex,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,a.parent_addressC,a.parent_name,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,mother,(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Roll_No in('" + rollval + "')   ORDER BY r.Reg_No ";//isconfirm ='1' and admission_status ='1'

           query = "select  distinct parent_addressP,Streetp,cityp,(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode) parent_statep,Countryp,Student_Mobile,parent_phnop,sections,bldgrp,r.stud_type,date_applied,alter_mobileno,SubCaste,parent_income,emailp,mother, mIncome,emailM,parentM_Mobile,Aadharcard_no,Streetc,cityc,parent_statec,isnull(Countryc,'') as Countryc ,Boarding,isnull(vehid,'') as vehid,CONVERT(VARCHAR(11),Adm_Date,103) as Adm_Date , (Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,r.app_no,r.Roll_No,r.Stud_Name,r.Current_Semester,r.Batch_Year,r.Reg_No,r.roll_admit,c.Course_Name,dt.Dept_Name,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,Dt.Dept_Name,StuPer_Id,parentF_Mobile,CONVERT(VARCHAR(11),dob,103) as dob ,case when sex='0' then 'Male' else 'Female' end as sex,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,a.parent_addressC,a.parent_name,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,mother,(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Roll_No in('" + rollval + "')   ORDER BY r.Reg_No ";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");

            if (query == "")
            {
                Fpspread2.Sheets[0].Visible = false;
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Kindly Select All List";
                lblAlertMsg.Visible = true;

                div_report.Visible = false;

                return;
            }
            else
            {
                if (query != "")
                {

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Sheets[0].Visible = false;
                        Fpspread2.Visible = false;
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Records Found";
                        //  lblerror.Visible = true;
                        // lblerror.Text = "No Records Found";
                        div_report.Visible = false;
                        // lbl_headernamespd2.Visible = false;
                        //lblvalidation1.Text = string.Empty;
                        return;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            div_report.Visible = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            int cc = 0;
                            int j = 0;

                            DataSet dss = new DataSet();
                            string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' ";
                            dss.Clear();
                            dss = da.select_method_wo_parameter(selcol1, "Text");
                            if (dss.Tables.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                    {
                                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                        if (value != "")
                                        {
                                            string[] valuesplit = value.Split(',');
                                            if (valuesplit.Length > 0)
                                            {
                                                for (int k = 0; k < valuesplit.Length; k++)
                                                {
                                                    cc++;
                                                    colval = Convert.ToString(valuesplit[k]);
                                                    loadtext();

                                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = printval;

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    divPopAlert.Visible = true;
                                    lblAlertMsg.Visible = true;
                                    lblAlertMsg.Text = "No Records Found";

                                    //  imgdiv2.Visible = true;
                                    // lbl_alert.Text = "No Records Found";
                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;
                                    //  img_settingpdf.Visible = false;
                                    //btn_viewsprd2.Visible = false;
                                    // lnk_admisstionformbtn.Visible = false;
                                    //lbl_headernamespd2.Visible = false;
                                    //lblvalidation1.Text = string.Empty;
                                    return;
                                }


                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Set Column Order";
                                Fpspread2.Visible = false;
                                div_report.Visible = false;
                                //  img_settingpdf.Visible = false;
                                // btn_viewsprd2.Visible = false;
                                // lnk_admisstionformbtn.Visible = false;
                                // lbl_headernamespd2.Visible = false;
                                // lblvalidation1.Text = string.Empty;
                                return;
                            }
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                string rollno = ds.Tables[0].Rows[i]["roll_no"].ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                cc = 0;
                                string text = string.Empty;
                                for (int k = 1; k < Fpspread2.Sheets[0].ColumnCount; k++)
                                {
                                    cc++;
                                    string col = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                    if (col == "Countryp" || col == "Countryc")//delsiref
                                    {
                                        text = da.GetFunction("select textval from textvaltable where TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                    }

                                    if (col.ToLower() == "cityp" || col.ToLower() == "cityc")
                                    {
                                        if (!Convert.ToString(ds.Tables[0].Rows[i][col]).Any(char.IsLetter))
                                            text = da.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                    }
                                    if (col.ToLower() == "bldgrp")
                                    {
                                        if (!Convert.ToString(ds.Tables[0].Rows[i][col]).Any(char.IsLetter))
                                            text = da.GetFunction("select textval from textvaltable where TextCriteria ='bgrou' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                    
                                    }
                                    if (col.ToLower() == "community")
                                    {
                                        if (!Convert.ToString(ds.Tables[0].Rows[i][col]).Any(char.IsLetter))
                                            text = da.GetFunction("select textval from textvaltable where TextCriteria ='comm' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                    
                                    }


                                    if (text == "0")
                                    {
                                        text = string.Empty;
                                    }
                                    if (text == "")
                                    {
                                        text = string.Empty;
                                    }


                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                }


                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                    }
                }

            }


        }
        catch (Exception ex)
        {


        }
    }


    public void fpspread1go1staff()//delsiss
    {
        try
        {
            up_spd1.Visible = true;
            Fpspread2.Visible = true;
            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;


            int val = 0;
            int count = 0;
            int count1 = 0;
            int i = 0;
            string header = string.Empty;
            string actval = string.Empty;
            string staffCode = string.Empty;
            string applid = string.Empty;
            string headertype1 = string.Empty;
            string headertype = string.Empty;
            activerow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            activecol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            string sec_textvalue = string.Empty;
            string stfcode = string.Empty;

            string dept_tagvalue = string.Empty;
            dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            selectcolumnload();

            actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
            stfcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);//delsi 1007
            if (stfcode == "" || stfcode == "0")
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                lblAlertMsg.Visible = true;
                Fpspread2.Visible = false;
                div_report.Visible = false;
                return;

            }

            headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(2), Convert.ToInt32(activecol)].Tag);
            headertype1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(2), Convert.ToInt32(activecol)].Text);
            string dayscholerHostler = string.Empty;
            string memtype = string.Empty;
            string presentabs = string.Empty;

            if (headertype.Contains(','))
            {
                string[] splitval = headertype.Split(',');
                dayscholerHostler = Convert.ToString(splitval[0]);
                memtype = Convert.ToString(splitval[1]);
                presentabs = Convert.ToString(splitval[2]);

            }
            string query = string.Empty;

            //   query = "  select sa.appl_no,appl_id,s.staff_code,appl_name,t.desig_code,t.dept_code,father_name,s.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,stftype,CONVERT(varchar(20),dateofapply,103) as dateofapply,d.desig_name,h.dept_name,yofexp,bldgrp,CONVERT(varchar(20),interviewdate,103) as interviewdate,com_mobileno,CONVERT(varchar(20),join_date,103) as join_date ,CONVERT(varchar(20),retr_date,103) as retr_date,CONVERT(varchar(20),appointed_date,103) as appointed_date , CONVERT(varchar(20),relieve_date,103) as  relieve_date,s.adharcardno,subjects,qualification,email from staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d where sa.appl_no =s.appl_no and s.staff_code =t.staff_code and d.desig_code =t.desig_code and h.dept_code =t.dept_code and s.college_code =d.collegeCode and h.college_code =s.college_code and h.college_code = d.collegeCode and (resign =0 and settled =0 and isnull(Discontinue,'0') ='0') and latestrec=1 and sa.college_code='13' and sa.interviewstatus ='appointed' and t.dept_code in('" + dept_tagvalue + "') and s.staff_code in('" + stfcode + "')";

            query = "  select sa.appl_no,appl_id,s.staff_code,appl_name,PANGIRNumber,email,Per_MobileNo,StfNature,t.desig_code,t.dept_code,father_name,s.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,stftype,caste,religion,martial_status,Community,Nationality,CONVERT(varchar(20),dateofapply,103) as dateofapply,d.desig_name,h.dept_name,yofexp,bldgrp,CONVERT(varchar(20),interviewdate,103) as interviewdate,com_mobileno,CONVERT(varchar(20),join_date,103) as join_date ,CONVERT(varchar(20),retr_date,103) as retr_date,CONVERT(varchar(20),appointed_date,103) as appointed_date , CONVERT(varchar(20),relieve_date,103) as  relieve_date,s.adharcardno,subjects,qualification,email from staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d where sa.appl_no =s.appl_no and s.staff_code =t.staff_code and d.desig_code =t.desig_code and h.dept_code =t.dept_code and s.college_code =d.collegeCode and h.college_code =s.college_code and h.college_code = d.collegeCode and (resign =0 and settled =0 and isnull(Discontinue,'0') ='0') and latestrec=1 and sa.college_code='" + ddlCollege.SelectedItem.Value + "' and sa.interviewstatus ='appointed' and t.dept_code in('" + dept_tagvalue + "') and s.staff_code in('" + stfcode + "')";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");

            if (query == "")
            {
                Fpspread2.Sheets[0].Visible = false;
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Kindly Select All List";
                lblAlertMsg.Visible = true;

                div_report.Visible = false;

                return;
            }
            else
            {
                if (query != "")
                {

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Sheets[0].Visible = false;
                        Fpspread2.Visible = false;
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Records Found";

                        div_report.Visible = false;

                        return;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.SaveChanges();
                            div_report.Visible = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            int cc = 0;
                            int j = 0;

                            DataSet dss = new DataSet();
                            string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' ";
                            dss.Clear();
                            dss = da.select_method_wo_parameter(selcol1, "Text");
                            if (dss.Tables.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                    {
                                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                        if (value != "")
                                        {
                                            string[] valuesplit = value.Split(',');
                                            if (valuesplit.Length > 0)
                                            {
                                                for (int k = 0; k < valuesplit.Length; k++)
                                                {
                                                    cc++;
                                                    colval = Convert.ToString(valuesplit[k]);
                                                    loadtextstaff();
                                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = printval;

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    divPopAlert.Visible = true;
                                    lblAlertMsg.Visible = true;
                                    lblAlertMsg.Text = "No Records Found";

                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;

                                    return;
                                }


                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Set Column Order";
                                Fpspread2.Visible = false;
                                div_report.Visible = false;

                                return;
                            }
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                string rollno = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                cc = 0;
                                string text = string.Empty;
                                for (int k = 1; k < Fpspread2.Sheets[0].ColumnCount; k++)
                                {
                                    cc++;
                                    string col = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                }


                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                    }
                }

            }

        }
        catch (Exception ex)
        {


        }
    }

    public void fpspread1go1guest()//delsiss
    {
        try
        {
            up_spd1.Visible = true;
            Fpspread2.Visible = true;
            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;


            int val = 0;
            int count = 0;
            int count1 = 0;
            int i = 0;

            string actval = string.Empty;
            string Code = string.Empty;
            string applid = string.Empty;

            activerow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            activecol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);

            string dept_tagvalue = string.Empty;
            dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            selectcolumnload();

            actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
            Code = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);//delsi 1007
            if (Code == "" || Code == "0")
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                lblAlertMsg.Visible = true;
                Fpspread2.Visible = false;
                div_report.Visible = false;

                return;

            }
            string query = string.Empty;
            query = "select StudMessType,CONVERT(nvarchar, Vi.VendorContactPK) as VendorContactPK,h.id,Vi.VenContactName as Name,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,B.Building_Name,V.VendorAddress as discription from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No and VendorContactPK in('" + Code + "') order by code asc";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");

            if (query == "")
            {
                Fpspread2.Sheets[0].Visible = false;
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Kindly Select All List";
                lblAlertMsg.Visible = true;

                div_report.Visible = false;

                return;
            }
            else
            {
                if (query != "")
                {

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Sheets[0].Visible = false;
                        Fpspread2.Visible = false;
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Records Found";

                        div_report.Visible = false;

                        return;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.SaveChanges();
                            div_report.Visible = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            int cc = 0;
                            int j = 0;

                            DataSet dss = new DataSet();
                            string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' ";
                            dss.Clear();
                            dss = da.select_method_wo_parameter(selcol1, "Text");
                            if (dss.Tables.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                    {
                                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                        if (value != "")
                                        {
                                            string[] valuesplit = value.Split(',');
                                            if (valuesplit.Length > 0)
                                            {
                                                for (int k = 0; k < valuesplit.Length; k++)
                                                {
                                                    cc++;
                                                    colval = Convert.ToString(valuesplit[k]);
                                                    loadtextguest();
                                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = printval;

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    divPopAlert.Visible = true;
                                    lblAlertMsg.Visible = true;
                                    lblAlertMsg.Text = "No Records Found";

                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;

                                    return;
                                }


                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Set Column Order";
                                Fpspread2.Visible = false;
                                div_report.Visible = false;

                                return;
                            }
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                string rollno = ds.Tables[0].Rows[i]["VendorContactPK"].ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                cc = 0;
                                string text = string.Empty;
                                for (int k = 1; k < Fpspread2.Sheets[0].ColumnCount; k++)
                                {
                                    cc++;
                                    string col = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");

                                }


                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                    }
                }

            }

        }
        catch (Exception ex)
        {

        }
    }


    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = da.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }

    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    protected void fpspread2_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
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
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "please Enter Report Name";

            }
            else
            {
                divPopAlert.Visible = false;
                lblAlertMsg.Visible = false;
                lblAlertMsg.Text = "";

            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); 
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(Fpspread2, report);

                divPopAlert.Visible = false;
                lblAlertMsg.Visible = false;
                lblAlertMsg.Text = "";

            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter Your Report Name";
                // lbl_norec.Text = "Please Enter Your Report Name";
                //lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Present and Absent Count Report";
            string pagename = "PresentnAbsentCountDetails.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); 
        }
    }

    public void loadlcolumns()
    {
        try
        {
            string linkname = "StudentStrengthCommon column order settings";
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + userCode + "' and college_code='" + ddlCollege.SelectedItem.Value + "' ";
            dscol.Clear();
            dscol = da.select_method_wo_parameter(selcol, "Text");
            if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colval = Convert.ToString(valuesplit[k]);
                                loadtext();
                                lb_column1.Items.Add(new ListItem(loadval, colval));
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

    public void selectcolumnload()
    {
        columnname = string.Empty;
        columnname1 = string.Empty;
        string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
        int cc = 0;
        string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlCollege.SelectedItem.Value + "' ";
        ds = da.select_method_wo_parameter(selcol1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int c = 0; c < ds.Tables[0].Rows.Count; c++)
            {
                string value = Convert.ToString(ds.Tables[0].Rows[c]["LinkValue"]);
                if (value != "")
                {
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            cc++;
                            colval = Convert.ToString(valuesplit[k]);
                            string c_name = columnload(colval);
                            string c_name1 = columnload1(colval);
                            if (c_name != "")
                            {
                                if (columnname == "")
                                {
                                    columnname = c_name;
                                }
                                else
                                {
                                    columnname = columnname + "," + c_name;
                                }
                                if (columnname1 == "")
                                {
                                    columnname1 = c_name1;
                                }
                                else
                                {
                                    columnname1 = columnname1 + "," + c_name1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }





    public string columnload(string v)//delsi
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "isnull( r.Sections,'') as Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "r.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "(Select Stage_Name FROM Stage_Master T WHERE Boarding = T.stage_id) Boarding";
        }
        if (colval == "123")
        {
            value = "vehid";
        }
        if (colval == "43")
        {
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")//delsii
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = "CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }

        return value;
    }

    public string columnload1(string v)
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        //if (colval == "28")
        //{
        //    value = "Institute_name";
        //}
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "''Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "a.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "'' Boarding";
        }
        if (colval == "123")
        {
            value = "''vehid";
        }
        if (colval == "43")
        {
            // value = "case when r.Mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end Mode ";
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = " CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "38")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }

        return value;//delsii
    }






}