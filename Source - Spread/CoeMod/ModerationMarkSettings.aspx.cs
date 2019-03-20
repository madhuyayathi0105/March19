using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.IO;
using System.Linq;
using System.Text;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class ModerationMarkSettings : System.Web.UI.Page
{
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    ReuasableMethods rs = new ReuasableMethods();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
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
            }
            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
           
            if (!IsPostBack)
            {
                Bindcollege();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                loadSem();
                rbRegular.Checked = true;
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

    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            string groupUserCode = string.Empty;
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
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
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
            //txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            //chkDegree.Checked = false;
            //cblDegree.Items.Clear();
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
                string selDegree = "SELECT DISTINCT c.Edu_Level FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY c.Edu_Level ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlEdulevl.DataSource = ds;
                ddlEdulevl.DataTextField = "Edu_Level";
                ddlEdulevl.DataValueField = "Edu_Level";
                ddlEdulevl.DataBind();
                //checkBoxListselectOrDeselect(cblDegree, true);
                //CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void loadSem()
    {
        ds.Clear();
        collegeCode = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        if (ddlCollege.Items.Count > 0)
            collegeCode = ddlCollege.SelectedValue.ToString().Trim();
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        //if (cblDegree.Items.Count > 0)
        //    valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        string SelSem = string.Empty;
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
        {
            SelSem = "select distinct current_semester from Registration  order by Current_Semester";//and degree_code in('" + valDegree + "')
            ds = da.select_method_wo_parameter(SelSem, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "current_semester";
                cbl_sem.DataValueField = "current_semester";
                cbl_sem.DataBind();
                checkBoxListselectOrDeselect(cbl_sem, true);
                CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
            }
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
            //if (cblDegree.Items.Count > 0)
            //    valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Edu_Level in('" + ddlEdulevl.SelectedItem.ToString() + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
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

    public void bindMonthandYear()
    {
        try
        {
            ddlExtMin.Items.Clear();
            ddlCiaMin.Items.Clear();
            ddlExtMin.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Considered", "1"));
            ddlExtMin.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Not Considered", "2"));
            ddlExtMin.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Empty", "3"));

            ddlCiaMin.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Considered", "1"));
            ddlCiaMin.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Not Considered", "2"));
            ddlCiaMin.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Empty", "3"));
        }
        catch
        {
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            loadSem();
            content.Visible = false;
           

        }
        catch (Exception ex)
        {
          
        }
    }

    protected void ddlEdulevl_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        loadSem();
        content.Visible = false;
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
            loadSem();
            content.Visible = false;
         

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
            loadSem();
            content.Visible = false;
         


        }
        catch (Exception ex)
        {
        }
    }

    //protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
    //        bindbranch();
    //        loadSem();
          

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
    //        bindbranch();
    //        loadSem();
           
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            loadSem();
            content.Visible = false;
           
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
            loadSem();
            content.Visible = false;
           
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
        content.Visible = false;

    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
        content.Visible = false;
    }

    protected void Radiochanged(object sender, EventArgs e)
    {
        content.Visible = false;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        bindMonthandYear();
        content.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string stuflag = string.Empty;
            if (rbRegular.Checked)
            {
                stuflag = "1";
            }
            else if (rbArear.Checked)
            {
                stuflag = "2";
            }
            else
            {
                lblAlertMsg.Text = "Choose Arear/Regular";
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                return;
            }

            string valBatch = rs.getCblSelectedValue(cblBatch);
            string valDegree = rs.getCblSelectedValue(cblBranch);
            string valSem = rs.getCblSelectedValue(cbl_sem);
            string edulevel = ddlEdulevl.SelectedValue.ToString();
            string strbatch = getCblSelectedValueComma(cblBatch);
            string strDegree = getCblSelectedValueComma(cblBranch);
            string strSem = getCblSelectedValueComma(cbl_sem);
            string MinEse = txtExtMin.Text;
            string MinCia = txtCiaMin.Text;
            string ModMark = txtModeration.Text;

            string valCia = ddlCiaMin.SelectedValue.ToString();
            string vaESE = ddlExtMin.SelectedValue.ToString();

            string value = vaESE + "," + valCia;

            string colCode = ddlCollege.SelectedValue.ToString();
            int save = 0;
            string[] batchloop = strbatch.Split(',');
            string[] degreeloop=strDegree.Split(',');
            for (int i = 0; i < batchloop.Length; i++)
            {
                for (int j = 0; j < degreeloop.Length; j++)
                {

                    if (ddlCiaMin.SelectedValue.ToString() != "0" && ddlExtMin.SelectedValue.ToString() != "0" && !string.IsNullOrEmpty(MinEse))
                    {
                        string SelectQ = "if exists(select * from New_ModSettings where college_code='" + colCode + "' and LinkName='MaximumModerationNew' and stuflag='" + stuflag + "' and BatchYear='" + Convert.ToString(batchloop[i]) + "' and DegreeCode='" + degreeloop[j] + "') update New_ModSettings SET LinkValue='" + ModMark + "',Semester='" + strSem + "',MinCIA='" + MinCia + "',MinESE='" + MinEse + "',value='" + value + "' where college_code='" + colCode + "' and LinkName='MaximumModerationNew' and BatchYear='" + Convert.ToString(batchloop[i]) + "' and DegreeCode='" + degreeloop[j] + "' and stuflag='" + stuflag + "'  else insert into New_ModSettings (LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag) values('MaximumModerationNew','" + ModMark + "','" + colCode + "','" + Convert.ToString(batchloop[i]) + "','" + degreeloop[j] + "','" + strSem + "','" + MinCia + "','" + MinEse + "','" + value + "','" + stuflag + "')";

                        save = da.update_method_wo_parameter(SelectQ, "Text");
                    }
                }
            }
            if (save > 0)
            {
                lblAlertMsg.Text = "Saved Sucessfully";
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Saved";
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                return;
            }
          
        }
        catch
        {

        }
    }

    public string getCblSelectedValueComma(CheckBoxList cblSelected)
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
                        selectedvalue.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopAlert.Visible = false;
        lblAlertMsg.Visible = false;
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

}