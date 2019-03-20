using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.IO;
using System.Text;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class ModurationApply : System.Web.UI.Page
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
    string usercode = string.Empty;
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
                chkindividual.Checked = true;
                Bindcollege();
                bindMonthandYear();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                loadSem();
                bindSubject();
                if (ChkBundlewise.Checked == true)
                {
                    //ddlsem1.Enabled = false;
                    //ddlsubtype.Enabled = false;
                    //ddlSubject.Enabled = false;
                    //UpdatePanel24.Visible = true;
                    txtBundleNo.Enabled = true;
                    ddlbranch1.Enabled = false;
                    ddldegree1.Enabled = false;
                    ddlsem1.Enabled = false;
                    ddlSubject.Enabled = false;
                }
                else
                {
                    ddlbranch1.Enabled = true;
                    ddldegree1.Enabled = true;
                    ddlsem1.Enabled = true;
                    ddlSubject.Enabled = true;
                    txtBundleNo.Enabled = false;
                    //UpdatePanel24.Visible = false;

                }
                lblBeMod.Visible = false;
                lblAfMod.Visible = false;
                fpspread.Visible = false;
                fpspread1.Visible = false;
                fpspread2.Visible = false;

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
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and  exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();

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
            if (!chkCommon.Checked)
            {
                if (ddlBatch.Items.Count > 0)
                    valBatch = ddlBatch.SelectedValue.ToString().Trim();
            }
            else
            {
                valBatch = rs.getCblSelectedValue(cblBatch);
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  and r.Exam_Flag<>'debar' AND r.Batch_Year in('" + valBatch + "') AND r.college_code in('" + collegeCode + "')  " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree1.DataSource = ds;
                ddldegree1.DataTextField = "course_name";
                ddldegree1.DataValueField = "course_id";
                ddldegree1.DataBind();

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

    public void loadSem()
    {
        ds.Clear();
        collegeCode = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        if (ddlCollege.Items.Count > 0)
            collegeCode = ddlCollege.SelectedValue.ToString().Trim();
        //if (ddlBatch.Items.Count > 0)
        //    valBatch = ddlBatch.SelectedValue.ToString().Trim();
        if (ddlbranch1.Items.Count > 0)
            valDegree = ddlbranch1.SelectedValue.ToString().Trim();
        string SelSem = string.Empty;
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree))//&& !string.IsNullOrEmpty(valBatch) 
        {
            SelSem = "select distinct current_semester from Registration order by Current_Semester";////where Batch_Year in('" + valBatch + "')
            ds = da.select_method_wo_parameter(SelSem, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsem1.DataSource = ds;
                ddlsem1.DataTextField = "current_semester";
                ddlsem1.DataValueField = "current_semester";
                ddlsem1.DataBind();

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
            string valDegree = string.Empty;
            if (!chkCommon.Checked)//rs.GetSelectedItemsValueAsString(cblBranch);
            {
                if (ddlBatch.Items.Count > 0)
                    valBatch = ddlBatch.SelectedValue.ToString().Trim();
                if (ddldegree1.Items.Count > 0)
                    valDegree = ddldegree1.SelectedValue.ToString().Trim();
            }
            else
            {
                valBatch=rs.GetSelectedItemsValueAsString(cblBatch);
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);
            }
          

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(valBatch))//
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "')  AND c.Course_Id in('" + valDegree + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch1.DataSource = ds;
                ddlbranch1.DataTextField = "dept_name";
                ddlbranch1.DataValueField = "degree_code";
                ddlbranch1.DataBind();

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

    public void bindSubject()
    {
        try
        {
            ds.Clear();
            ddlSubject.Items.Clear();
            collegeCode = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;

            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            if (!chkCommon.Checked)
            {
                if (ddlBatch.Items.Count > 0)
                    valBatch = ddlBatch.SelectedValue.ToString().Trim();
                if (ddlbranch1.Items.Count > 0)
                    valDegree = ddlbranch1.SelectedValue.ToString().Trim();
                if (ddlsem1.Items.Count > 0)
                    sem = ddlsem1.SelectedValue.ToString().Trim();
            }
            else
            {
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
                sem = rs.getCblSelectedValue(cbl_sem);
            }

            string sql = string.Empty;

            if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem))//
            {
                string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester in ('" + sem + "')  and ed.batch_year in('" + valBatch + "')  and d.Degree_Code in('" + valDegree + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and d.college_code='" + collegeCode + "'";//

                //qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";

                ds = da.select_method_wo_parameter(qeryss, "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlSubject.DataSource = ds;
                    ddlSubject.DataTextField = "subnamecode";
                    ddlSubject.DataValueField = "Subject_Code";
                    ddlSubject.DataBind();

                    cblsubject.DataSource = ds;
                    cblsubject.DataTextField = "subnamecode";
                    cblsubject.DataValueField = "Subject_Code";
                    cblsubject.DataBind();
                    checkBoxListselectOrDeselect(cblsubject, true);
                    CallCheckboxListChange(chksubject, cblsubject, txtSubject, lblSubject.Text, "--Select--");
                }
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
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year - l));
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void clear()
    {
        try
        {
            lblerr1.Visible = false;
            btnsave1.Visible = false;
            btnprintt.Visible = false;
            fpspread.Visible = false;
            fpspread1.Visible = false;
            lblBeMod.Visible = false;
            lblAfMod.Visible = false;
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            fpspread2.Visible = false;
            if (ddlreptype.SelectedItem.ToString() == "Special Moderation")//2 mod
            {
                if (chkMultiple.Checked || chkCommon.Checked)
                {
                    ddlSubject.Visible = false;
                    Div5.Visible = true;
                }
                else
                {
                    ddlSubject.Visible = true;
                    Div5.Visible = false;
                }
            }
            
        }
        catch
        {
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            loadSem();
            bindSubject();
            lblBeMod.Visible = false;
            lblAfMod.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    protected void lblrepttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        binddegree();
        bindbranch();
        loadSem();
        bindSubject();
    }

    protected void ddldegree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindbranch();
            loadSem();
            bindSubject();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            loadSem();
            bindSubject();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlsem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindSubject();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
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
            loadSem();
            bindSubject();

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
            bindSubject();


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
            loadSem();
            bindSubject();

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
            loadSem();
            bindSubject();
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
            loadSem();
            bindSubject();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
        bindSubject();

    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            loadSem();
            bindSubject();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
        bindSubject();
    }

    protected void chksubject_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chksubject, cblsubject, txtSubject, lblSubject.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chksubject, cblsubject, txtSubject, lblSubject.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkCommon.Checked)
            {
                if (ddlreptype.SelectedItem.ToString() == "Genral Moderation")
                    General();
                else if (ddlreptype.SelectedItem.ToString() == "Special Moderation")
                    splMod();
            }
            else
            {
                if (ChkBundlewise.Checked == false)
                {
                    if (ddldegree1.SelectedIndex < 0)
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select Degree";
                        return;
                    }
                    if (ddlbranch1.SelectedIndex < 0)
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select branch";
                        return;
                    }
                    if (ddlsem1.SelectedIndex < 0)
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select Semester";
                        return;
                    }
                    if (!chkMultiple.Checked)
                    {
                        if (ddlreptype.SelectedItem.ToString() != "Genral Moderation" && ddlreptype.SelectedItem.ToString() != "Degree Moderation")
                        {
                            if (ddlSubject.SelectedIndex < 0)
                            {
                                lblerr1.Visible = true;
                                lblerr1.Text = "Please Select Subject";
                                return;
                            }
                            buttongo();
                        }
                        else
                        {
                            buttongo();
                        }
                    }
                    else
                    {
                        // clear();
                        buttongo();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtBundleNo.Text))
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Enter BundleNo";
                        return;
                    }
                    else
                    {
                        //clear();
                        buttongo();
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void buttongo()
    {
        try
        {

            if (chkMultiple.Checked)
            {
                if (ddlreptype.SelectedItem.ToString() == "Special Moderation")
                {
                    spclMultipleMod();
                }
            }
            else
            {
                if (ddlreptype.SelectedItem.ToString() == "Genral Moderation")
                {
                    GenralMod();
                }
                else if (ddlreptype.SelectedItem.ToString() == "Degree Moderation")
                {
                    DegreeMod();
                }
                else
                {
                    clear();
                    fpspread1.Visible = false;
                    int markround = 0;
                    lblerr1.Visible = false;
                    DataSet ds2 = new DataSet();
                    DataSet ds1 = new DataSet();
                    string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);
                    string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
                    string BatchYear = Convert.ToString(ddlBatch.SelectedValue);
                    string DegreeCode = Convert.ToString(ddlbranch1.SelectedValue);

                    string bundleNo = string.Empty;

                    if (ChkBundlewise.Checked && !string.IsNullOrEmpty(txtBundleNo.Text))
                    {
                        subjectCodeNew = da.GetFunction("select distinct  s.subject_code from exam_seating es,Exam_Details ed,subject s where s.subject_no=es.subject_no and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "' and es.bundle_no='" + txtBundleNo.Text + "' ");
                        bundleNo = txtBundleNo.Text;
                    }


                    string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
                    if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
                    {
                        int num = 0;
                        if (int.TryParse(getmarkround, out num))
                        {
                            markround = Convert.ToInt32(getmarkround);
                        }
                    }
                    double modMark = 0;
                    if (!string.IsNullOrEmpty(txtMod.Text))
                        double.TryParse(txtMod.Text, out modMark);

                    #region Dummy Number Display

                    byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
                    string dummyNumberType = string.Empty;

                    if (DummyNumberType() == 1)
                    {
                        dummyNumberType = " and subject='" + subjectCodeNew + "' ";
                    }
                    else
                    {
                        dummyNumberType = " and isnull(subject,'')='' ";
                    }
                    string selDummyQ = string.Empty;

                    selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlmonth.SelectedValue.ToString() + "' and exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' and degreecode='" + ddlbranch1.SelectedValue + "'";

                    DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
                    bool showDummyNumber = ShowDummyNumber();
                    if (showDummyNumber)
                    {
                        //if (dtMappedNumbers.Rows.Count == 0)
                        //{
                        //    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                        //    lblAlertMsg.Visible = true;
                        //    lblAlertMsg.Text = "No Dummy Numbers Generated";
                        //    divPopAlert.Visible = true;
                        //    return;
                        //}
                    }
                    #endregion

                    string degreeval = string.Empty;
                    string degreevalregmoder = string.Empty;
                    string degreevalttab = string.Empty;
                    string degreevalregis = string.Empty;

                    if (!ChkBundlewise.Checked)
                    {
                        degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                        degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                        degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                        degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
                    }

                    //Moderation Settings
                    DataTable dtModSett = new DataTable();
                    string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

                    dtModSett = dirAcc.selectDataTable(strMod);
                    //---------------------

                    fpspread.Width = 880;
                    fpspread.Height = 0;
                    fpspread.Visible = true;
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Sheets[0].ColumnCount = 0;
                    fpspread.Sheets[0].ColumnCount = 8;

                    fpspread.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
                    MyStyle.Font.Size = FontUnit.Medium;
                    MyStyle.Font.Name = "Book Antiqua";
                    MyStyle.Font.Bold = true;
                    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                    MyStyle.ForeColor = Color.Black;
                    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                    fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
                    fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                    fpspread.Sheets[0].AutoPostBack = false;
                    fpspread.CommandBar.Visible = false;

                    //double minicamoderation = 0;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
                    fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 150;
                    fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 80;
                    fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
                    fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
                    fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 120;
                    fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 150;
                    fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 80;

                    fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
                    {
                        string qeryss = string.Empty;
                        qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year='" + BatchYear + "' and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                        ds1 = da.select_method_wo_parameter(qeryss, "text");

                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            string subject_no = subjectCodeNew;
                            string getdetails = string.Empty;
                            //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                            // string sem = ddlsem1.SelectedValue.ToString();
                            if (ChkBundlewise.Checked)
                            {
                                getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "'";
                            }

                            else
                            {
                                getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.Batch_Year='" + BatchYear + "'  " + degreeval + " and s.subject_code='" + subjectCodeNew + "' and r.college_code='" + CollegeCode + "'";
                            }

                            getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + subjectCodeNew + "' " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                            getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code='" + subjectCodeNew + "' ";

                            ds2 = da.select_method_wo_parameter(getdetails, "Text");

                            int height = 50;
                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                                //if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "CIA";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "ESC";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Moderation";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "After Moderation";
                                fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Result";
                                fpspread.Sheets[0].Columns[0].Visible = true;
                                int sno = 0;
                                foreach (DataRow dr in ds2.Tables[0].Rows)
                                {


                                    string regNo = Convert.ToString(dr["reg_no"]);
                                    string rollNo = Convert.ToString(dr["roll_no"]);
                                    string ev1 = Convert.ToString(dr["internal_mark"]);
                                    string ev2 = Convert.ToString(dr["external_mark"]);
                                    string result = Convert.ToString(dr["result"]);
                                    string total = Convert.ToString(dr["total"]);
                                    string batch = Convert.ToString(dr["Batch_Year"]);
                                    string degCode = Convert.ToString(dr["degree_code"]);
                                    string examCode = Convert.ToString(dr["exam_code"]);
                                    string SubjectNo = Convert.ToString(dr["subject_no"]);
                                    string cursem = Convert.ToString(dr["Current_Semester"]);

                                    double minintmark = 0;
                                    double maxintmark = 0;
                                    double minextmark = 0;
                                    double maxextmark = 0;
                                    double mintotmark = 0;
                                    double maxtotmark = 0;
                                    string stuflag = string.Empty;
                                    string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                    if (dtregArr == "0")
                                        stuflag = "1";
                                    else
                                        stuflag = "2";

                                    DataTable dtModmark = new DataTable();
                                    if (dtModSett.Rows.Count > 0)
                                    {
                                        dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                        dtModmark = dtModSett.DefaultView.ToTable();
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}

                                    //Double modMark1 = 0;
                                    Double minModCIA = 0;
                                    Double minModESE = 0;
                                    string elgVal = string.Empty;
                                    if (dtModmark.Rows.Count > 0)
                                    {
                                        //double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                        elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}
                                    string Esc = Convert.ToString(dr["external_mark"]);
                                    double getESC = 0;
                                    double.TryParse(Esc, out getESC);
                                    string CIA = Convert.ToString(dr["internal_mark"]);
                                    double getCIA = 0;
                                    double.TryParse(CIA, out getCIA);
                                    double gettot = 0;
                                    double.TryParse(total, out gettot);

                                    double afterMod = 0;
                                    double NeedMark = 0;
                                    string afterESC = string.Empty;
                                    string aftertot = string.Empty;
                                    string afterResult = string.Empty;

                                    if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && ev2 != "-19" && ev2 != "-19" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                        minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                        maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                        minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                        maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                        mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                        maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                        if (ddlreptype.SelectedItem.ToString() == "Special Moderation")//2 mod
                                        {
                                            if (gettot < mintotmark && minModCIA <= getCIA && minModESE <= getESC)
                                            {
                                                if (mintotmark >= gettot)
                                                    NeedMark = mintotmark - gettot;
                                                if (NeedMark > 0)
                                                {
                                                    if (NeedMark <= modMark)//Round off Mod
                                                    {
                                                        double chkminESC = NeedMark + getESC;
                                                        if (chkminESC >= minextmark)
                                                        {
                                                            afterMod = NeedMark + getESC;
                                                            afterESC = NeedMark.ToString();
                                                            aftertot = (NeedMark + gettot).ToString();
                                                            afterResult = "Pass";
                                                        }
                                                        else
                                                        {
                                                            NeedMark = minextmark - getESC;
                                                            if (NeedMark <= modMark)
                                                            {
                                                                double chkmintot = NeedMark + getESC + getCIA;
                                                                if (chkmintot >= mintotmark)
                                                                {
                                                                    afterMod = NeedMark + getESC;
                                                                    afterESC = NeedMark.ToString();
                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                    afterResult = "Pass";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (getESC < minextmark && minModCIA <= getCIA && minModESE <= getESC)
                                                {
                                                    NeedMark = minextmark - getESC;
                                                    if (mintotmark <= gettot + NeedMark && NeedMark + getESC >= minextmark)
                                                    {
                                                        afterMod = NeedMark + getESC;
                                                        afterESC = NeedMark.ToString();
                                                        aftertot = (NeedMark + gettot).ToString();
                                                        afterResult = "Pass";
                                                    }
                                                }
                                            }

                                        }

                                        else if (ddlreptype.SelectedItem.ToString() == "Genral Moderation")
                                        {
                                            GenralMod();
                                        }

                                        else if (ddlreptype.SelectedItem.ToString() == "Round Off Moderation")
                                        {
                                            if (gettot < mintotmark || getESC < minextmark)
                                            {
                                                NeedMark = 1;

                                                //if (NeedMark <= modMark)//Round off Mod
                                                //{


                                                double chkminESC = NeedMark + getESC;
                                                if (chkminESC >= minextmark)
                                                {
                                                    if (mintotmark <= gettot + NeedMark)
                                                    {
                                                        afterMod = NeedMark + getESC;
                                                        afterESC = NeedMark.ToString();
                                                        aftertot = (NeedMark + gettot).ToString();
                                                        afterResult = "Pass";
                                                    }
                                                }
                                                else
                                                {
                                                    double chkmintot = NeedMark + gettot;
                                                    if (chkmintot >= mintotmark && getESC >= minextmark)
                                                    {
                                                        afterMod = NeedMark + getESC;
                                                        afterESC = NeedMark.ToString();
                                                        aftertot = (NeedMark + gettot).ToString();
                                                        afterResult = "Fail";
                                                    }
                                                }

                                            }
                                        }

                                        else if (ddlreptype.SelectedItem.ToString() == "Degree Moderation")
                                        {
                                            //modMark = modMark + 1;

                                            if (gettot < mintotmark)
                                            {
                                                NeedMark = mintotmark - gettot;

                                                //if (NeedMark <= modMark)//Round off Mod
                                                //{
                                                double chkminESC = NeedMark + getESC;
                                                if (chkminESC >= minextmark)
                                                {
                                                    afterMod = NeedMark + getESC;
                                                    afterESC = NeedMark.ToString();
                                                    aftertot = (NeedMark + gettot).ToString();
                                                    afterResult = "Pass";
                                                }
                                                else if (chkminESC < minextmark)
                                                {
                                                    NeedMark = 0;
                                                    NeedMark = minextmark - getESC;
                                                    afterMod = NeedMark + getESC;
                                                    afterESC = NeedMark.ToString();
                                                    aftertot = (NeedMark + gettot).ToString();
                                                    afterResult = "Pass";
                                                }
                                                else
                                                {

                                                }
                                                //}
                                            }
                                            else
                                            {
                                                if (getESC < minextmark)
                                                {
                                                    NeedMark = minextmark - getESC;
                                                    if (mintotmark <= gettot + NeedMark && NeedMark + getESC >= minextmark)
                                                    {
                                                        afterMod = NeedMark + getESC;
                                                        afterESC = NeedMark.ToString();
                                                        aftertot = (NeedMark + gettot).ToString();
                                                        afterResult = "Pass";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (ev1 == "-1")
                                    {
                                        ev1 = "AAA";
                                    }
                                    else if (ev1 == "-2")
                                    {
                                        ev1 = "NE";
                                    }
                                    else if (ev1 == "-3")
                                    {
                                        ev1 = "RA";
                                    }
                                    else if (ev1 == "-4")
                                    {
                                        ev1 = "LT";
                                    }
                                    else if (ev1 == "-19")
                                    {
                                        ev1 = "W";
                                    }
                                    else if (ev1.Trim() != "")
                                    {
                                        ev1 = ev1;
                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    if (ev2 == "-1")
                                    {
                                        ev2 = "AAA";
                                    }
                                    else if (ev2 == "-2")
                                    {
                                        ev2 = "NE";
                                    }
                                    else if (ev2 == "-3")
                                    {
                                        ev2 = "RA";
                                    }
                                    else if (ev2 == "-4")
                                    {
                                        ev2 = "LT";
                                    }
                                    else if (ev2 == "-19")
                                    {
                                        ev2 = "W";
                                    }
                                    else if (ev2.Trim() != "")
                                    {
                                        ev2 = ev2;
                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    sno++;
                                    fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dr["Batch_Year"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dr["subject_no"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["reg_no"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["degree_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["roll_no"]);


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;//
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = ev1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["exam_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dr["Current_Semester"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = ev2;

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = gettot.ToString();

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = (afterESC != "0") ? afterESC : "";

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = (aftertot != "0") ? aftertot : "";


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = (!string.IsNullOrEmpty(afterResult)) ? afterResult : result;

                                    if (!string.IsNullOrEmpty(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text))
                                        fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.SkyBlue;

                                    height = height + 20;
                                }
                                btnsave1.Visible = true;
                                btnprintt.Visible = true;
                                lblerr1.Visible = false;
                                fpspread.Height = height;
                                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                                fpspread.SaveChanges();
                                fpspread.Visible = true;
                                if (fpspread.Sheets[0].RowCount > 0)
                                {
                                    int bfMod = 0;
                                    int afMod = 0;
                                    for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                                    {
                                        string result = Convert.ToString(fpspread.Sheets[0].Cells[i, 7].Text);
                                        string Mod = Convert.ToString(fpspread.Sheets[0].Cells[i, 5].Text);
                                        if (result.ToLower() == "pass" && string.IsNullOrEmpty(Mod))
                                        {
                                            bfMod = bfMod + 1;
                                            afMod = afMod + 1;
                                        }
                                        else if (result.ToLower() == "pass" && !string.IsNullOrEmpty(Mod))
                                        {
                                            afMod = afMod + 1;
                                        }
                                    }
                                    lblBeMod.Visible = true;
                                    lblAfMod.Visible = true;
                                    lblBeMod.Text = "Before Moderation-" + bfMod;
                                    lblAfMod.Text = "After Moderation-" + afMod;
                                }
                                else
                                {
                                    lblBeMod.Visible = false;
                                    lblAfMod.Visible = false;
                                }


                            }
                            else
                            {
                                lblerr1.Visible = true;
                                lblerr1.Text = "No Record Found";
                                fpspread.Visible = false;
                                btnsave1.Visible = false;
                                btnprintt.Visible = false;
                                lblBeMod.Visible = false;
                                lblAfMod.Visible = false;
                            }

                        }
                        else
                        {
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Record Found";
                            fpspread.Visible = false;
                            btnsave1.Visible = false;
                            btnprintt.Visible = false;
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                        }
                    }
                }
            }
            

        }
        catch
        {
        }
    }

    protected void GenralMod()
    {
        try
        {
            clear();
            fpspread1.Visible = false;
            int markround = 0;
            lblerr1.Visible = false;
            DataSet ds2 = new DataSet();
            DataSet ds1 = new DataSet();
            string subjectCodeNew = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;
            collegeCode = string.Empty;
            string sql = string.Empty;
            DataTable dtsubject = new DataTable();
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            if (ddlbranch1.Items.Count > 0)
                valDegree = ddlbranch1.SelectedValue.ToString().Trim();
            if (ddlsem1.Items.Count > 0)
                sem = ddlsem1.SelectedValue.ToString().Trim();

            string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string BatchYear = Convert.ToString(ddlBatch.SelectedValue);
            string DegreeCode = Convert.ToString(ddlbranch1.SelectedValue);
            string bundleNo = string.Empty;

            if (ChkBundlewise.Checked && !string.IsNullOrEmpty(txtBundleNo.Text))
            {
                subjectCodeNew = da.GetFunction("select distinct  s.subject_code from exam_seating es,Exam_Details ed,subject s where s.subject_no=es.subject_no and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "' and es.bundle_no='" + txtBundleNo.Text + "' ");
                bundleNo = txtBundleNo.Text;
            }
            else
            {
                ds.Clear();
                ddlSubject.Items.Clear();


                if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(BatchYear))//
                {
                    string qeryss = "SELECT distinct s.subject_name,s.subject_code FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + sem + "'  and d.Degree_Code in('" + valDegree + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and d.college_code='" + collegeCode + "'";
                    dtsubject = dirAcc.selectDataTable(qeryss);
                }
                if (dtsubject.Rows.Count > 0)
                {
                    foreach (DataRow dtsub in dtsubject.Rows)
                    {
                        string subjectCode = Convert.ToString(dtsub["subject_code"]);
                        if (string.IsNullOrEmpty(subjectCodeNew))
                            subjectCodeNew = "'" + subjectCode + "'";
                        else
                            subjectCodeNew = subjectCodeNew + "," + "'" + subjectCode + "'";
                    }
                }
            }


            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }
            double modMark = 0;
            if (!string.IsNullOrEmpty(txtMod.Text))
                double.TryParse(txtMod.Text, out modMark);

            #region Dummy Number Display

            //byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
            //string dummyNumberType = string.Empty;

            //if (DummyNumberType() == 1)
            //{
            //    dummyNumberType = " and subject='" + subjectCodeNew + "' ";
            //}
            //else
            //{
            //    dummyNumberType = " and isnull(subject,'')='' ";
            //}
            //string selDummyQ = string.Empty;

            //selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlmonth.SelectedValue.ToString() + "' and exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' and degreecode='" + ddlbranch1.SelectedValue + "'";

            //DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
            //bool showDummyNumber = ShowDummyNumber();
            //if (showDummyNumber)
            //{
            //    if (dtMappedNumbers.Rows.Count == 0)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
            //        lblAlertMsg.Visible = true;
            //        lblAlertMsg.Text = "No Dummy Numbers Generated";
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            #endregion

            string degreeval = string.Empty;
            string degreevalregmoder = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;
            Hashtable hatStudntMark = new Hashtable();
            if (!ChkBundlewise.Checked)
            {
                degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            }

            //Moderation Settings
            DataTable dtModSett = new DataTable();
            string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

            dtModSett = dirAcc.selectDataTable(strMod);
            //---------------------

            fpspread.Width = 880;
            fpspread.Height = 0;
            fpspread.Visible = true;
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 0;
            fpspread.Sheets[0].ColumnCount = 8;

            fpspread.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;

            //double minicamoderation = 0;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 120;
            fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 80;

            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
            {
                string qeryss = string.Empty;
                qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no and r.batch_year='" + BatchYear + "' and r.Batch_Year='" + BatchYear + "' " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                ds1 = da.select_method_wo_parameter(qeryss, "text");

                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    string subject_no = subjectCodeNew;
                    string getdetails = string.Empty;
                    //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                    // string sem = ddlsem1.SelectedValue.ToString();
                    if (ChkBundlewise.Checked)
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "' order by r.reg_no";
                    }

                    else
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.batch_year='" + BatchYear + "' " + degreeval + " and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "' order by r.reg_no";
                    }

                    getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in(" + subjectCodeNew + ") " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                    getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in(" + subjectCodeNew + ")";

                    ds2 = da.select_method_wo_parameter(getdetails, "Text");

                    int height = 50;
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                        //if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "CIA";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "ESC";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "After Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Result";
                        fpspread.Sheets[0].Columns[0].Visible = true;

                        foreach (DataRow dr1 in dtsubject.Rows)
                        {
                            string subjectCode = Convert.ToString(dr1["subject_code"]);

                            ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                            DataTable dicSubject = ds2.Tables[0].DefaultView.ToTable();
                            if (dicSubject.Rows.Count > 0)
                            {
                                int sno = 0;
                                string subjCode = Convert.ToString(dicSubject.Rows[0]["subject_code"]);
                                string SubName = Convert.ToString(dicSubject.Rows[0]["subject_name"]);
                                fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = subjCode + " - " + SubName;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = Color.LightPink;
                                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 8);
                                foreach (DataRow dr in dicSubject.Rows)
                                {
                                    //if (!string.IsNullOrEmpty(txtMod.Text))
                                    //    double.TryParse(txtMod.Text, out modMark);

                                    string regNo = Convert.ToString(dr["reg_no"]);
                                    string rollNo = Convert.ToString(dr["roll_no"]);
                                    string ev1 = Convert.ToString(dr["internal_mark"]);
                                    string ev2 = Convert.ToString(dr["external_mark"]);
                                    string result = Convert.ToString(dr["result"]);
                                    string total = Convert.ToString(dr["total"]);
                                    string batch = Convert.ToString(dr["Batch_Year"]);
                                    string degCode = Convert.ToString(dr["degree_code"]);
                                    string examCode = Convert.ToString(dr["exam_code"]);
                                    string SubjectNo = Convert.ToString(dr["subject_no"]);
                                    string cursem = Convert.ToString(dr["Current_Semester"]);

                                    string stuflag = string.Empty;
                                    string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                    if (dtregArr == "0")
                                        stuflag = "1";
                                    else
                                        stuflag = "2";
                                    DataTable dtModmark = new DataTable();
                                    if (dtModSett.Rows.Count > 0)
                                    {
                                        dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                        dtModmark = dtModSett.DefaultView.ToTable();
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}

                                    //Double modMark = 0;
                                    Double minModCIA = 0;
                                    Double minModESE = 0;
                                    string elgVal = string.Empty;
                                    if (dtModmark.Rows.Count > 0)
                                    {
                                        //double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                        elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}
                                    double minintmark = 0;
                                    double maxintmark = 0;
                                    double minextmark = 0;
                                    double maxextmark = 0;
                                    double mintotmark = 0;
                                    double maxtotmark = 0;

                                    string Esc = Convert.ToString(dr["external_mark"]);
                                    double getESC = 0;
                                    double.TryParse(Esc, out getESC);
                                    string CIA = Convert.ToString(dr["internal_mark"]);
                                    double getCIA = 0;
                                    double.TryParse(CIA, out getCIA);
                                    double gettot = 0;
                                    double.TryParse(total, out gettot);

                                    double afterMod = 0;
                                    double NeedMark = 0;
                                    string afterESC = string.Empty;
                                    string aftertot = string.Empty;
                                    string afterResult = string.Empty;

                                    if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                        minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                        maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                        minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                        maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                        mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                        maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                        if (ddlreptype.SelectedItem.ToString() == "Genral Moderation")//2 mod
                                        {
                                            if (minModESE <= getESC && minModCIA <= getCIA)
                                            {
                                                if (gettot < mintotmark || getESC < minextmark)
                                                {
                                                    NeedMark = mintotmark - gettot;

                                                    if (NeedMark <= modMark)//Round off Mod
                                                    {
                                                        double chkminESC = NeedMark + getESC;
                                                        if (chkminESC >= minextmark && mintotmark <= chkminESC + getCIA)
                                                        {
                                                            if (!hatStudntMark.ContainsKey(regNo))
                                                            {
                                                                hatStudntMark.Add(regNo, NeedMark);
                                                                afterMod = NeedMark + getESC;
                                                                afterESC = NeedMark.ToString();
                                                                aftertot = (NeedMark + gettot).ToString();
                                                                afterResult = "Pass";
                                                            }
                                                            else
                                                            {
                                                                double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                double chkmod = NeedMark + mark;
                                                                if (chkmod > modMark)
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    hatStudntMark[regNo] = chkmod;
                                                                    afterMod = NeedMark + getESC;
                                                                    afterESC = NeedMark.ToString();
                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                    afterResult = "Pass";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            NeedMark = minextmark - getESC;
                                                            if (NeedMark <= modMark)
                                                            {
                                                                double chkmintot = NeedMark + getESC + getCIA;
                                                                if (chkmintot >= mintotmark)
                                                                {
                                                                    if (!hatStudntMark.ContainsKey(regNo))
                                                                    {
                                                                        afterMod = NeedMark + getESC;
                                                                        afterESC = NeedMark.ToString();
                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                        afterResult = "Pass";
                                                                    }
                                                                    else
                                                                    {
                                                                        double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                        double chkmod = NeedMark + mark;
                                                                        if (chkmod > modMark)
                                                                        {

                                                                        }
                                                                        else
                                                                        {
                                                                            hatStudntMark[regNo] = chkmod;
                                                                            afterMod = NeedMark + getESC;
                                                                            afterESC = NeedMark.ToString();
                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                            afterResult = "Pass";
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        NeedMark = 0;
                                                        if (getESC < minextmark)
                                                        {
                                                            NeedMark = minextmark - getESC;
                                                            if (NeedMark <= modMark)
                                                            {
                                                                double chkminESC = NeedMark + getESC;
                                                                if (chkminESC >= minextmark && mintotmark <= chkminESC + getCIA)
                                                                {
                                                                    if (!hatStudntMark.ContainsKey(regNo))
                                                                    {
                                                                        hatStudntMark.Add(regNo, NeedMark);
                                                                        afterMod = NeedMark + getESC;
                                                                        afterESC = NeedMark.ToString();
                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                        afterResult = "Pass";
                                                                    }
                                                                    else
                                                                    {
                                                                        double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                        double chkmod = NeedMark + mark;
                                                                        if (chkmod > modMark)
                                                                        {

                                                                        }
                                                                        else
                                                                        {
                                                                            hatStudntMark[regNo] = chkmod;
                                                                            afterMod = NeedMark + getESC;
                                                                            afterESC = NeedMark.ToString();
                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                            afterResult = "Pass";
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
                                    if (ev1 == "-1")
                                    {
                                        ev1 = "AAA";
                                    }
                                    else if (ev1 == "-2")
                                    {
                                        ev1 = "NE";
                                    }
                                    else if (ev1 == "-3")
                                    {
                                        ev1 = "RA";
                                    }
                                    else if (ev1 == "-4")
                                    {
                                        ev1 = "LT";
                                    }
                                    else if (ev1.Trim() != "")
                                    {
                                        //ev1 = ev1;

                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    if (ev2 == "-1")
                                    {
                                        ev2 = "AAA";
                                    }
                                    else if (ev2 == "-2")
                                    {
                                        ev2 = "NE";
                                    }
                                    else if (ev2 == "-3")
                                    {
                                        ev2 = "RA";
                                    }
                                    else if (ev2 == "-4")
                                    {
                                        ev2 = "LT";
                                    }
                                    else if (ev2.Trim() != "")
                                    {
                                        //ev2 = ev2;
                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    sno++;
                                    fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dr["Batch_Year"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dr["subject_no"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["reg_no"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["degree_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["roll_no"]);


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;//
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = ev1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["exam_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dr["Current_Semester"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = ev2;

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = gettot.ToString();

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = (afterESC != "0") ? afterESC : "";

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = (aftertot != "0") ? aftertot : "";


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = (!string.IsNullOrEmpty(afterResult)) ? afterResult : result;

                                    if (!string.IsNullOrEmpty(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text))
                                        fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.SkyBlue;

                                    height = height + 20;
                                }
                            }
                        }
                        btnsave1.Visible = true;
                        btnprintt.Visible = true;
                        lblerr1.Visible = false;
                        fpspread.Height = height;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                        fpspread.SaveChanges();
                        fpspread.Visible = true;
                        if (fpspread.Sheets[0].RowCount > 0)
                        {
                            int bfMod = 0;
                            int afMod = 0;
                            for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string result = Convert.ToString(fpspread.Sheets[0].Cells[i, 7].Text);
                                string Mod = Convert.ToString(fpspread.Sheets[0].Cells[i, 5].Text);
                                if (result.ToLower() == "pass" && string.IsNullOrEmpty(Mod))
                                {
                                    bfMod = bfMod + 1;
                                    afMod = afMod + 1;
                                }
                                else if (result.ToLower() == "pass" && !string.IsNullOrEmpty(Mod))
                                {
                                    afMod = afMod + 1;
                                }
                            }
                            lblBeMod.Visible = true;
                            lblAfMod.Visible = true;
                            lblBeMod.Text = "Before Moderation-" + bfMod;
                            lblAfMod.Text = "After Moderation-" + afMod;
                        }
                        else
                        {
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                        }


                    }

                    else
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "No Record Found";
                        fpspread.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        lblBeMod.Visible = false;
                        lblAfMod.Visible = false;
                    }

                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Record Found";
                    fpspread.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    lblBeMod.Visible = false;
                    lblAfMod.Visible = false;
                }
            }

        }
        catch
        {
        }
    }

    protected void DegreeMod()
    {

        try
        {
            clear();
            fpspread1.Visible = false;
            int markround = 0;
            lblerr1.Visible = false;
            DataSet ds2 = new DataSet();
            DataSet ds1 = new DataSet();
            string subjectCodeNew = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;
            collegeCode = string.Empty;
            string sql = string.Empty;
            DataTable dtsubject = new DataTable();
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            if (ddlbranch1.Items.Count > 0)
                valDegree = ddlbranch1.SelectedValue.ToString().Trim();
            if (ddlsem1.Items.Count > 0)
                sem = ddlsem1.SelectedValue.ToString().Trim();

            string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string Batchyear = Convert.ToString(ddlBatch.SelectedValue);
            string bundleNo = string.Empty;

            if (ChkBundlewise.Checked && !string.IsNullOrEmpty(txtBundleNo.Text))
            {
                subjectCodeNew = da.GetFunction("select distinct  s.subject_code from exam_seating es,Exam_Details ed,subject s where s.subject_no=es.subject_no and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "' and es.bundle_no='" + txtBundleNo.Text + "' ");
                bundleNo = txtBundleNo.Text;
            }
            else
            {
                ds.Clear();
                ddlSubject.Items.Clear();
                if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem))//&& !string.IsNullOrEmpty(valBatch)
                {
                    string qeryss = "SELECT distinct s.subject_name,s.subject_code FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id   and d.Degree_Code in('" + valDegree + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and d.college_code='" + collegeCode + "'";//and sy.semester='" + sem + "'
                    dtsubject = dirAcc.selectDataTable(qeryss);
                }
                if (dtsubject.Rows.Count > 0)
                {
                    foreach (DataRow dtsub in dtsubject.Rows)
                    {
                        string subjectCode = Convert.ToString(dtsub["subject_code"]);
                        if (string.IsNullOrEmpty(subjectCodeNew))
                            subjectCodeNew = "'" + subjectCode + "'";
                        else
                            subjectCodeNew = subjectCodeNew + "," + "'" + subjectCode + "'";
                    }
                }
            }


            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }
            double modMark = 0;
            if (!string.IsNullOrEmpty(txtMod.Text))
                double.TryParse(txtMod.Text, out modMark);

            #region Dummy Number Display

            //byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
            //string dummyNumberType = string.Empty;

            //if (DummyNumberType() == 1)
            //{
            //    dummyNumberType = " and subject='" + subjectCodeNew + "' ";
            //}
            //else
            //{
            //    dummyNumberType = " and isnull(subject,'')='' ";
            //}
            //string selDummyQ = string.Empty;

            //selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlmonth.SelectedValue.ToString() + "' and exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' and degreecode='" + ddlbranch1.SelectedValue + "'";

            //DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
            //bool showDummyNumber = ShowDummyNumber();
            //if (showDummyNumber)
            //{
            //    if (dtMappedNumbers.Rows.Count == 0)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
            //        lblAlertMsg.Visible = true;
            //        lblAlertMsg.Text = "No Dummy Numbers Generated";
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            #endregion

            string degreeval = string.Empty;
            string degreevalregmoder = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;
            Hashtable hatStudntMark = new Hashtable();
            if (!ChkBundlewise.Checked)
            {
                degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            }

            fpspread.Width = 1030;
            fpspread.Height = 0;
            fpspread.Visible = true;
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 0;
            fpspread.Sheets[0].ColumnCount = 9;

            fpspread.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;

            //double minicamoderation = 0;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 120;
            fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[8].Width = 80;

            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
            {
                string qeryss = string.Empty;
                qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no and r.Batch_Year='"+Batchyear+"' " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                ds1 = da.select_method_wo_parameter(qeryss, "text");

                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    string subject_no = subjectCodeNew;
                    string getdetails = string.Empty;
                    //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                    // string sem = ddlsem1.SelectedValue.ToString();
                    if (ChkBundlewise.Checked)
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "' and result<>'pass' order by r.reg_no";
                    }

                    else
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.Batch_Year='" + Batchyear + "'  " + degreeval + " and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "' and result<>'pass' order by r.reg_no";
                    }

                    getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in(" + subjectCodeNew + ") " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                    getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in(" + subjectCodeNew + ")";

                    ds2 = da.select_method_wo_parameter(getdetails, "Text");
                    int sno = 0;
                    int height = 50;
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                        //if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "CIA";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "ESC";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "After Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Result";
                        fpspread.Sheets[0].Columns[0].Visible = true;


                        //foreach (DataRow dr1 in ds2.Tables[0].Rows)
                        //{
                        //    string subjectCode = Convert.ToString(dr1["Roll_No"]);

                            //ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                            //DataTable dicSubject = ds2.Tables[0].DefaultView.ToTable();

                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds2.Tables[0].Rows)
                                {
                                    if (!string.IsNullOrEmpty(txtMod.Text))
                                        double.TryParse(txtMod.Text, out modMark);

                                    string regNo = Convert.ToString(dr["reg_no"]);
                                    string rollNo = Convert.ToString(dr["roll_no"]);
                                    string ev1 = Convert.ToString(dr["internal_mark"]);
                                    string ev2 = Convert.ToString(dr["external_mark"]);
                                    string result = Convert.ToString(dr["result"]);
                                    string total = Convert.ToString(dr["total"]);
                                    string batch = Convert.ToString(dr["Batch_Year"]);
                                    string degCode = Convert.ToString(dr["degree_code"]);
                                    string examCode = Convert.ToString(dr["exam_code"]);
                                    string SubjectNo = Convert.ToString(dr["subject_no"]);
                                    string subjectCode = Convert.ToString(dr["subject_code"]);

                                    double minintmark = 0;
                                    double maxintmark = 0;
                                    double minextmark = 0;
                                    double maxextmark = 0;
                                    double mintotmark = 0;
                                    double maxtotmark = 0;

                                    string Esc = Convert.ToString(dr["external_mark"]);
                                    double getESC = 0;
                                    double.TryParse(Esc, out getESC);
                                    string CIA = Convert.ToString(dr["internal_mark"]);
                                    double getCIA = 0;
                                    double.TryParse(CIA, out getCIA);
                                    double gettot = 0;
                                    double.TryParse(total, out gettot);

                                    double afterMod = 0;
                                    double NeedMark = 0;
                                    string afterESC = string.Empty;
                                    string aftertot = string.Empty;
                                    string afterResult = string.Empty;

                                    if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                        ds2.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable degfailCount = ds2.Tables[0].DefaultView.ToTable();
                                        if (degfailCount.Rows.Count < 2)
                                        {
                                            minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                            maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                            minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                            maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                            mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                            maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                            if (ddlreptype.SelectedItem.ToString() == "Degree Moderation")//2 mod
                                            {
                                                if (gettot < mintotmark)
                                                {
                                                    NeedMark = mintotmark - gettot;

                                                    if (NeedMark <= modMark)//Round off Mod
                                                    {
                                                        double chkminESC = NeedMark + getESC;
                                                        if (chkminESC >= minextmark)
                                                        {
                                                            if (!hatStudntMark.ContainsKey(regNo))
                                                            {
                                                                hatStudntMark.Add(regNo, NeedMark);
                                                                afterMod = NeedMark + getESC;
                                                                afterESC = NeedMark.ToString();
                                                                aftertot = (NeedMark + gettot).ToString();
                                                                afterResult = "Pass";
                                                            }
                                                            else
                                                            {
                                                                double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                double chkmod = NeedMark + mark;
                                                                if (chkmod > modMark)
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    hatStudntMark[regNo] = chkmod;
                                                                    afterMod = NeedMark + getESC;
                                                                    afterESC = NeedMark.ToString();
                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                    afterResult = "Pass";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            NeedMark = minextmark - getESC;
                                                            if (NeedMark <= modMark)
                                                            {
                                                                double chkmintot = NeedMark + getESC + getCIA;
                                                                if (chkmintot >= mintotmark)
                                                                {
                                                                    if (!hatStudntMark.ContainsKey(regNo))
                                                                    {
                                                                        afterMod = NeedMark + getESC;
                                                                        afterESC = NeedMark.ToString();
                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                        afterResult = "Pass";
                                                                    }
                                                                    else
                                                                    {
                                                                        double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                        double chkmod = NeedMark + mark;
                                                                        if (chkmod > modMark)
                                                                        {

                                                                        }
                                                                        else
                                                                        {
                                                                            hatStudntMark[regNo] = chkmod;
                                                                            afterMod = NeedMark + getESC;
                                                                            afterESC = NeedMark.ToString();
                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                            afterResult = "Pass";
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
                                    if (ev1 == "-1")
                                    {
                                        ev1 = "AAA";
                                    }
                                    else if (ev1 == "-2")
                                    {
                                        ev1 = "NE";
                                    }
                                    else if (ev1 == "-3")
                                    {
                                        ev1 = "RA";
                                    }
                                    else if (ev1 == "-4")
                                    {
                                        ev1 = "LT";
                                    }
                                    else if (ev1.Trim() != "")
                                    {
                                        ev1 = ev1;

                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    if (ev2 == "-1")
                                    {
                                        ev2 = "AAA";
                                    }
                                    else if (ev2 == "-2")
                                    {
                                        ev2 = "NE";
                                    }
                                    else if (ev2 == "-3")
                                    {
                                        ev2 = "RA";
                                    }
                                    else if (ev2 == "-4")
                                    {
                                        ev2 = "LT";
                                    }
                                    else if (ev2.Trim() != "")
                                    {
                                        ev2 = ev2;
                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    if (afterESC != "0" && afterESC != "")// 
                                    {
                                        sno++;
                                        fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dr["Batch_Year"]);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dr["subject_no"]);

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["reg_no"]);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["degree_code"]);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["roll_no"]);

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = subjectCode;
                                        //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["degree_code"]);
                                        //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dr["roll_no"]);


                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;//
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = ev1;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dr["exam_code"]);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dr["Current_Semester"]);

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = ev2;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = gettot.ToString();

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                       // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = (afterESC != "0") ? afterESC : "";

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = (aftertot != "0") ? aftertot : "";


                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                        //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = (!string.IsNullOrEmpty(afterResult)) ? afterResult : result;

                                        if (!string.IsNullOrEmpty(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text))
                                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.SkyBlue;

                                        height = height + 20;
                                    }
                                }
                            }
                        //}
                        btnsave1.Visible = true;
                        btnprintt.Visible = true;
                        lblerr1.Visible = false;
                        fpspread.Height = height;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                        fpspread.SaveChanges();
                        fpspread.Visible = true;
                        if (fpspread.Sheets[0].RowCount > 0)
                        {
                            int bfMod = 0;
                            int afMod = 0;
                            for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string result = Convert.ToString(fpspread.Sheets[0].Cells[i, 6].Text);
                                string Mod = Convert.ToString(fpspread.Sheets[0].Cells[i, 8].Text);
                                if (result.ToLower() == "pass" && string.IsNullOrEmpty(Mod))
                                {
                                    bfMod = bfMod + 1;
                                    afMod = afMod + 1;
                                }
                                else if (result.ToLower() == "pass" && !string.IsNullOrEmpty(Mod))
                                {
                                    afMod = afMod + 1;
                                }
                            }
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                            lblBeMod.Text = "Before Moderation-" + bfMod;
                            lblAfMod.Text = "After Moderation-" + afMod;
                        }
                        else
                        {
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                        }

                    }

                    else
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "No Record Found";
                        fpspread.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        lblBeMod.Visible = false;
                        lblAfMod.Visible = false;
                    }

                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Record Found";
                    fpspread.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    lblBeMod.Visible = false;
                    lblAfMod.Visible = false;
                }
            }

        }
        catch
        {
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            fpspread1.Width = 880;
            fpspread1.Height = 0;
            fpspread1.Visible = false;
            fpspread.Visible = false;
            fpspread1.Sheets[0].RowCount = 0;
            fpspread1.Sheets[0].ColumnCount = 0;
            fpspread1.Sheets[0].ColumnCount = 8;

            fpspread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread1.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread1.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;

            //double minicamoderation = 0;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fpspread1.Sheets[0].ColumnHeader.Columns[0].Width = 70;
            fpspread1.Sheets[0].ColumnHeader.Columns[1].Width = 150;
            fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 80;
            fpspread1.Sheets[0].ColumnHeader.Columns[3].Width = 120;
            fpspread1.Sheets[0].ColumnHeader.Columns[4].Width = 110;
            fpspread1.Sheets[0].ColumnHeader.Columns[5].Width = 120;
            fpspread1.Sheets[0].ColumnHeader.Columns[6].Width = 150;
            fpspread1.Sheets[0].ColumnHeader.Columns[7].Width = 80;

            fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);
            string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string BatchYear = Convert.ToString(ddlBatch.SelectedValue);
            string DegreeCode = Convert.ToString(ddlbranch1.SelectedValue);

            string bundleNo = string.Empty;
            lblerr1.Visible = false;
            if (ChkBundlewise.Checked && !string.IsNullOrEmpty(txtBundleNo.Text))
            {
                subjectCodeNew = da.GetFunction("select distinct  s.subject_code from exam_seating es,Exam_Details ed,subject s where s.subject_no=es.subject_no and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "' and es.bundle_no='" + txtBundleNo.Text + "' ");
                bundleNo = txtBundleNo.Text;
            }


            string SelectQ = "select roll_no,batch_year,degree_code,exam_code,m.subject_no,Semester,bf_moderation_extmrk,af_moderation_extmrk,passmark,modtype from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + subjectCodeNew + "'  and m.exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "' and m.exam_month='" + Convert.ToString(ddlmonth.SelectedValue) + "'; select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "'   and s.subject_code='" + subjectCodeNew + "';";

            DataSet dsModStudent = da.select_method_wo_parameter(SelectQ, "text");

            int height = 50;
            if (dsModStudent.Tables.Count > 1 && dsModStudent.Tables[0].Rows.Count > 0)
            {
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "CIA";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Before ESE";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "After ESE";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Moderation";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "After Moderation";
                fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Result";
                fpspread1.Sheets[0].Columns[0].Visible = true;

                int sno = 0;
                foreach (DataRow dr in dsModStudent.Tables[0].Rows)
                {
                    string rollNo = Convert.ToString(dr["roll_no"]);
                    string batchYear = Convert.ToString(dr["batch_year"]);
                    string degreeCode = Convert.ToString(dr["degree_code"]);
                    string exam_code = Convert.ToString(dr["exam_code"]);
                    string subjectNo = Convert.ToString(dr["subject_no"]);
                    string semester = Convert.ToString(dr["Semester"]);
                    string bfMod = Convert.ToString(dr["bf_moderation_extmrk"]);
                    string afMod = Convert.ToString(dr["af_moderation_extmrk"]);
                    string modMark = Convert.ToString(dr["passmark"]);
                    string Modtype = Convert.ToString(dr["modtype"]);


                    dsModStudent.Tables[1].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                    DataTable dtMark = dsModStudent.Tables[1].DefaultView.ToTable();

                    string regno = Convert.ToString(dtMark.Rows[0]["reg_no"]);
                    string tot = Convert.ToString(dtMark.Rows[0]["total"]);
                    string CIA = Convert.ToString(dtMark.Rows[0]["internal_mark"]);
                    string result = Convert.ToString(dtMark.Rows[0]["result"]);
                    //double cianew = 0;

                    sno++;
                    fpspread1.Sheets[0].RowCount = fpspread1.Sheets[0].RowCount + 1;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Tag = batchYear;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Note = subjectNo;

                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Text = regno;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Tag = degreeCode;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Note = rollNo;


                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;//
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = CIA;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Tag = exam_code;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Note = semester;

                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 3].Text = bfMod;

                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 4].Text = afMod.ToString();


                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 5].Text = modMark;

                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 6].Text = tot;


                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 7].Text = result;
                    height = height + 100;
                    Button3.Visible = true;
                }
                btnsave1.Visible = false;
                btnprintt.Visible = false;
                lblerr1.Visible = false;
                fpspread1.Height = height;
                fpspread1.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                fpspread1.SaveChanges();
                fpspread1.Visible = true;

            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Moderation Applied";
            }
        }
        catch
        {

        }
    }

    protected void btnsavel1_click(object sender, EventArgs e)
    {
        try
        {
            //clear();
            int Count = 0;
            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                string ModMark = Convert.ToString(fpspread.Sheets[0].Cells[r, 5].Text);
                //string totMod = txtMod.Text;
                if (!string.IsNullOrEmpty(ModMark))
                {
                    string modtype = Convert.ToString(ddlreptype.SelectedItem);
                    string subNo = Convert.ToString(fpspread.Sheets[0].Cells[r, 0].Note);
                    string batchYear = Convert.ToString(fpspread.Sheets[0].Cells[r, 0].Tag);

                    string rollNo = Convert.ToString(fpspread.Sheets[0].Cells[r, 1].Note);
                    string regNo = Convert.ToString(fpspread.Sheets[0].Cells[r, 1].Text);
                    string degCode = Convert.ToString(fpspread.Sheets[0].Cells[r, 1].Tag);
                    string examCode = Convert.ToString(fpspread.Sheets[0].Cells[r, 2].Tag);
                    string semester = Convert.ToString(fpspread.Sheets[0].Cells[r, 2].Note);
                    string beforeESE = Convert.ToString(fpspread.Sheets[0].Cells[r, 3].Text);

                    string beforetot = Convert.ToString(fpspread.Sheets[0].Cells[r, 4].Text);
                    string modMark = Convert.ToString(fpspread.Sheets[0].Cells[r, 5].Text);
                    string aftertot = Convert.ToString(fpspread.Sheets[0].Cells[r, 6].Text);
                    string Result = Convert.ToString(fpspread.Sheets[0].Cells[r, 7].Text);
                    //string semester = 

                    string examMonth = Convert.ToString(ddlmonth.SelectedValue);
                    string examYear = Convert.ToString(ddlyear.SelectedItem);

                    double totMod = 0;
                    if (!string.IsNullOrEmpty(txtMod.Text))
                        double.TryParse(txtMod.Text, out totMod);

                    if (beforeESE.Trim().ToLower() != "aaa" && beforeESE.Trim().ToLower() != "ab" && beforeESE.Trim().ToLower() != "aa" && beforeESE.Trim().ToLower() != "a" && beforeESE.Trim().ToLower() != "ne" && beforeESE.Trim().ToLower() != "nr" && beforeESE.Trim().ToLower() != "m" && beforeESE.Trim().ToLower() != "lt")
                    {
                        double bfMod = 0;
                        double.TryParse(beforeESE, out bfMod);
                        double Mod = 0;
                        double.TryParse(modMark, out Mod);
                        double afmod = Mod + bfMod;

                        double remainModmark = totMod - Mod;


                        if (!string.IsNullOrEmpty(modMark))
                        {
                            string updateQ = "if exists(select * from moderation where exam_code='" + examCode + "' and subject_no='" + subNo + "' and roll_no='" + rollNo + "' and degree_code='" + degCode + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "') update moderation set roll_no='" + rollNo + "',bf_moderation_extmrk='" + beforeESE + "',af_moderation_extmrk='" + Convert.ToString(afmod) + "',passmark='" + modMark + "',remainingmark='" + Convert.ToString(remainModmark) + "',moderation_mark='" + totMod + "',Modtype='" + modtype + "' where exam_code='" + examCode + "' and subject_no='" + subNo + "' and roll_no='" + rollNo + "' and degree_code='" + degCode + "' and semester='" + semester + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "' else insert into moderation(batch_year,degree_code,exam_code,subject_no,Semester,roll_no,bf_moderation_extmrk,af_moderation_extmrk,passmark,remainingmark,moderation_mark,exam_month,exam_year,Modtype) values('" + batchYear + "','" + degCode + "','" + examCode + "','" + subNo + "','" + semester + "','" + rollNo + "','" + beforeESE + "','" + Convert.ToString(afmod) + "','" + Convert.ToString(modMark) + "','" + Convert.ToString(remainModmark) + "','" + totMod + "','" + examMonth + "','" + examYear + "','" + modtype + "')";
                            Count = dirAcc.updateData(updateQ);


                            string UpdateMarkEntry = "update mark_entry SET result='" + Result + "',external_mark='" + Convert.ToString(afmod) + "',total='" + aftertot + "' where roll_no='" + rollNo + "' and exam_code='" + examCode + "' and subject_no='" + subNo + "'";
                            Count = dirAcc.updateData(UpdateMarkEntry);

                        }

                    }
                }
            }
            if (Count > 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Sucessfully";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnprintt_print(object sender, EventArgs e)
    {
        //string degreedetails = Convert.ToString(ddlyear.SelectedItem) + "-" + Convert.ToString(ddlmonth.SelectedItem) + "-" + Convert.ToString(ddlbranch1.SelectedItem) + "-" + Convert.ToString(ddlSubject.SelectedItem);
        string degreedetails = Convert.ToString(ddlyear.SelectedItem) + "-" + Convert.ToString(ddlmonth.SelectedItem) + "-" + "Moderation Report";
        fpspread.SaveChanges();
        Printcontrol.loadspreaddetails(fpspread, "ModurationApply.aspx", degreedetails);
        //fpsalary.Sheets[0].Rows[0].Visible = true;
        Printcontrol.Visible = true;
    }

    private bool ShowDummyNumber()
    {
        bool retval = false;
        string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);

        string saveDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowDummyNumberOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + userCode + "'  ").Trim();
        if (saveDummy == "1")
        {
            retval = true;
        }
        return retval;
    }

    private byte DummyNumberType()
    {
        string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
        byte retval = 0;//0-common , 1- subjectwise
        string typeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + userCode + "'  ").Trim();
        if (typeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    private byte getDummyNumberMode()
    {
        string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
        byte retval = 0;//0-Serial , 1- Random
        string modeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + userCode + "'  ").Trim();
        if (modeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    protected void chkBundleNo_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkBundlewise.Checked == true)
        {
            clear();
            //ddlsem1.Enabled = false;
            //ddlsubtype.Enabled = false;
            //ddlSubject.Enabled = false;
            //UpdatePanel24.Visible = true;
            txtBundleNo.Enabled = true;
            ddlbranch1.Enabled = false;
            ddldegree1.Enabled = false;
            ddlsem1.Enabled = false;
            ddlSubject.Enabled = false;
        }
        else
        {
            clear();
            ddlbranch1.Enabled = true;
            ddldegree1.Enabled = true;
            ddlsem1.Enabled = true;
            ddlSubject.Enabled = true;
            txtBundleNo.Enabled = false;
            //UpdatePanel24.Visible = false;

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getbundleno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            string query = "";
            WebService ws = new WebService();

            //staff query
            //query = "select distinct ISNULL(es.bundle_no,'0') as bundleNo from exam_seating es,Exam_Details ed where ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examyear + "' order by bundleNo asc";
            query = "select distinct ISNULL(es.bundle_no,'0') as bundleNo from exam_seating es  where bundle_no like '" + prefixText + "%' order by bundleNo asc";


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
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
            //da.sendErrorMail(ex, collegecode, "Exam Application");
        }
    }

    protected void btnHelp_Click(object sender, EventArgs e)
    {
        StringBuilder SbHtml = new StringBuilder();

        #region I Page
        SbHtml.Append("<html>");
        SbHtml.Append("<body>");
        SbHtml.Append("<div style='height:845px; width: 655px; border:1px solid black; margin:0px; margin-left: 5px;page-break-after: always;'>");

        #region Header

        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px; font-weight: bold;'>");
        SbHtml.Append("<tr style='text-align:right;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:center;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Moderation Apply Help</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");

        #endregion

        #region  Details

        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px; font-weight: bold;'>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>1.Round Off Moderation:</span>");
        
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");

        SbHtml.Append("<span>In this Moderation used to any students are need 1 Mark to get Minimum total. It apply and reach minimum total for that student</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        #endregion

        #region FooterDetails

        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px;'>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>If any Complaint or Suggestions,Please Contect to Palpap!</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        SbHtml.Append("</div>");
        SbHtml.Append("</body>");
        SbHtml.Append("</html>");

        contentDiv.InnerHtml = SbHtml.ToString();
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btnPrint", "PrintDiv();", true);

        #endregion

        #endregion
    }

    protected void chkindividual_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkindividual.Checked)
            {
                chkindividual.Checked = true;
                chkCommon.Checked = false;
                chkMultiple.Checked = false;
                ddlreptype.Items.Clear();
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Special Moderation", "2"));
                ddlreptype.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Genral Moderation", "3"));
                ddlreptype.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));

                Div1.Visible = false;
                ddlsem1.Visible = true;
                div2.Visible = false;
                ddlBatch.Visible = true;
                ddldegree1.Visible = true;
                div3.Visible = false;
                ddlbranch1.Visible = true;
                div4.Visible = false;
                txtMod.Visible = true;
                txtfrom.Visible = false;
                txtTo.Visible = false;
                lbldum1.Visible = false;
                ddlSubject.Visible = true;
                Div5.Visible = false;
                clear();
            }
            else
            {
                chkindividual.Checked = false;
                chkCommon.Checked = true;
                chkMultiple.Checked = false;
                ddlreptype.Items.Clear();
                //ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Special Moderation", "1"));
                ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Genral Moderation", "2"));
                //ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));
                Div1.Visible = true;
                ddlsem1.Visible = false;
                div2.Visible = true;
                ddlBatch.Visible = false;
                ddldegree1.Visible = false;
                div3.Visible = true;
                ddlbranch1.Visible = false;
                div4.Visible = true;

                txtMod.Visible = false;
                txtfrom.Visible = true;
                txtTo.Visible = true;
                lbldum1.Visible = true;
                ddlSubject.Visible = false;
                Div5.Visible = true;
                clear();
            }
        }
        catch
        {

        }
    }

    protected void chkCommon_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkCommon.Checked)
            {
                chkindividual.Checked = false;
                chkCommon.Checked = true;
                chkMultiple.Checked = false;
                ddlreptype.Items.Clear();
                chkMultiple.Checked = false;
                //ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Special Moderation", "1"));
                ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Genral Moderation", "2"));
                //ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));
                Div1.Visible = true;
                ddlsem1.Visible = false;
                div2.Visible = true;
                ddlBatch.Visible = false;
                ddldegree1.Visible = false;
                div3.Visible = true;
                ddlbranch1.Visible = false;
                div4.Visible = true;
                txtMod.Visible = false;
                txtfrom.Visible = true;
                txtTo.Visible = true;
                lbldum1.Visible = true;
                ddlSubject.Visible = false;
                Div5.Visible = true;
                clear();
                
            }
            else
            {
                chkindividual.Checked = true;
                chkCommon.Checked = false;
               chkMultiple.Checked = false;
                ddlreptype.Items.Clear();
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Special Moderation", "2"));
                ddlreptype.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Genral Moderation", "3"));
                ddlreptype.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));

                Div1.Visible = false;
                ddlsem1.Visible = true;
                div2.Visible = false;
                ddlBatch.Visible = true;
                ddldegree1.Visible = true;
                div3.Visible = false;
                ddlbranch1.Visible = true;
                div4.Visible = false;
                txtMod.Visible = true;
                txtfrom.Visible = false;
                txtTo.Visible = false;
                lbldum1.Visible = false;
                ddlSubject.Visible = true;
                Div5.Visible = false;
                clear();
            }
        }
        catch
        {

        }
    }

    public void splMod()
    {
        try
        {
            Button2.Visible = true;
            Button1.Visible = true;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            lblerr1.Visible = false;
            int from = 0;
            int to = 0;
            int.TryParse(txtfrom.Text,out from);
            int.TryParse(txtTo.Text, out to);
            fpspread2.Sheets[0].RowCount = 0;
            fpspread2.Sheets[0].ColumnCount = 0;
            fpspread2.Sheets[0].ColumnHeader.RowCount = 2;
            fpspread2.Visible = true;
            fpspread2.RowHeader.Visible = false;
            fpspread2.CommandBar.Visible = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
           // fpspread2.Sheets[0].FrozenColumnCount = 2;
            int width = 300;
            //MyStyle.Font.Name = "Book Antiqua";
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 350;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Degree";
         
            fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";  //added by Mullai
            fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);


            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Before Moderation";
            fpspread2.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Pass %";

            int m = 4;
            int n = 4;
            if (from < to)
            {
                for (int sub = from; sub <= to; sub++)
                {

                    fpspread2.Sheets[0].ColumnCount++;
                    //fpspread2.Sheets[0].ColumnHeader.Columns[fpspread2.Sheets[0].ColumnCount-1].Width = 50;
                    fpspread2.Sheets[0].ColumnHeader.Cells[0, m].Text = sub.ToString();
                    fpspread2.Sheets[0].ColumnHeader.Cells[1, n].Text = "BS";
                    fpspread2.Sheets[0].ColumnCount++;
                    fpspread2.Sheets[0].ColumnHeader.Cells[1, n + 1].Text = "Pass %";
                    n = n + 2;
                    width = width + 100;
                    //fpspread2.Columns[sub].Width = 50;
                    //fpspread2.Sheets[0].ColumnCount++;
                    //fpspread2.Sheets[0].ColumnCount--;
                    fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, m, 1, 2);
                    m = m + 2;
                }
                fpspread2.Sheets[0].ColumnCount++;
                fpspread2.Sheets[0].ColumnHeader.Cells[0, fpspread2.Sheets[0].ColumnCount-1].Text = "Recommended Mod Mark";
                fpspread2.Sheets[0].ColumnHeader.Cells[0, fpspread2.Sheets[0].ColumnCount - 1].CellType = txt;
                fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread2.Sheets[0].ColumnCount-1, 2, 1);
            }
                
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Enter Valid Range";
            }

            //Moderation Settings
            DataTable dtModSett = new DataTable();
            string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

            dtModSett = dirAcc.selectDataTable(strMod);
            //---------------------
            DataSet ds2 = new DataSet();
            DataSet ds1 = new DataSet();
            string subjectCodeNew = string.Empty;
            string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string BatchYear = string.Empty;
            string DegreeCode = string.Empty;

            if(cblsubject.Items.Count>0)
                subjectCodeNew = rs.getCblSelectedValue(cblsubject);
            if (cblBranch.Items.Count > 0)
                DegreeCode = rs.getCblSelectedValue(cblBranch);
            if (cblBatch.Items.Count > 0)
                BatchYear = rs.getCblSelectedValue(cblBatch);
            if (string.IsNullOrEmpty(BatchYear))
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "please Select Batch";
                divPopAlert.Visible = true;
                fpspread2.Visible = false;
                return;
            }
            if (string.IsNullOrEmpty(DegreeCode))
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "please Select Degree";
                divPopAlert.Visible = true;
                fpspread2.Visible = false;
                return;
            }
            if (string.IsNullOrEmpty(subjectCodeNew))
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "please Select Subject";
                divPopAlert.Visible = true;
                fpspread2.Visible = false;
                return;
            }
            string bundleNo = string.Empty;
            int markround = 0;
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }
            double modMark = 0;
            if (!string.IsNullOrEmpty(txtMod.Text))
                double.TryParse(txtMod.Text, out modMark);

            #region Dummy Number Display

            byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
            string dummyNumberType = string.Empty;

            if (DummyNumberType() == 1)
            {
                dummyNumberType = " and subject='" + subjectCodeNew + "' ";
            }
            else
            {
                dummyNumberType = " and isnull(subject,'')='' ";
            }
            string selDummyQ = string.Empty;

            selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlmonth.SelectedValue.ToString() + "' and exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' and degreecode='" + ddlbranch1.SelectedValue + "'";

            DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
            bool showDummyNumber = ShowDummyNumber();
            if (showDummyNumber)
            {
                //if (dtMappedNumbers.Rows.Count == 0)
                //{
                //    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                //    lblAlertMsg.Visible = true;
                //    lblAlertMsg.Text = "No Dummy Numbers Generated";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            #endregion

            string degreeval = string.Empty;
            string degreevalregmoder = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;

                degreeval = " and ed.degree_code in('" + DegreeCode + "')";
                degreevalregmoder = " and M.degree_code in('"+DegreeCode + "')";
                degreevalttab = " and e.degree_code in('" +DegreeCode + "')";
                degreevalregis = " and r.degree_code in('" +DegreeCode + "')";

              

            if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
            {
                string qeryss = string.Empty;
                qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year in('" + BatchYear + "') and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                ds1 = da.select_method_wo_parameter(qeryss, "text");

                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    string subject_no = subjectCodeNew;
                    string getdetails = string.Empty;
                    //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                    // string sem = ddlsem1.SelectedValue.ToString();
                    if (ChkBundlewise.Checked)
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_name, ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "'";
                    }

                    else
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester,s.subject_code,s.subject_name,  me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.Batch_Year in('" + BatchYear + "')  " + degreeval + " and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "'";
                    }

                    getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in('" + subjectCodeNew + "') " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                    getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in('" + subjectCodeNew + "') ";

                    ds2 = da.select_method_wo_parameter(getdetails, "Text");

                    int countval = 0;
                    int bfpass = 0;
                    int afpass = 0;
                    string subname = string.Empty;
                    string depbranch = string.Empty;
                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            
                            DataTable dicSubjectCode = ds2.Tables[0].DefaultView.ToTable(true, "subject_code");
                            foreach (DataRow dtrow in dicSubjectCode.Rows)
                            {
                                int i = 0;
                                string subjectCode1 = Convert.ToString(dtrow["subject_code"]);
                             
                                ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode1 + "'";
                                DataTable dicSubStud = ds2.Tables[0].DefaultView.ToTable();
                                countval = 0;
                                fpspread2.Sheets[0].RowCount = fpspread2.Sheets[0].RowCount + 1;
                                int lastcol=0;
                                for (int a = from; a <= to; a++)
                                {
                                    i = i + 4;
                                bfpass = 0;
                                afpass = 0;
                                countval = 0;
                                double bfpar = 0;
                                double afper = 0;
                                foreach (DataRow dr in dicSubStud.Rows)
                                {
                                    //if (!string.IsNullOrEmpty(txtMod.Text))
                                    //    double.TryParse(txtMod.Text, out modMark);
                                    modMark = a;
                                    string regNo = Convert.ToString(dr["reg_no"]);
                                    string rollNo = Convert.ToString(dr["roll_no"]);
                                    string ev1 = Convert.ToString(dr["internal_mark"]);
                                    string ev2 = Convert.ToString(dr["external_mark"]);
                                    string result = Convert.ToString(dr["result"]);
                                    string total = Convert.ToString(dr["total"]);
                                    string batch = Convert.ToString(dr["Batch_Year"]);
                                    string degCode = Convert.ToString(dr["degree_code"]);
                                    string examCode = Convert.ToString(dr["exam_code"]);
                                    string SubjectNo = Convert.ToString(dr["subject_no"]);
                                    string subjectCode = Convert.ToString(dr["subject_code"]);
                                    string cursem = Convert.ToString(dr["Current_Semester"]);
                                     subname = Convert.ToString(dr["subject_name"]);
                                     string degdept = "select de.Dept_Name,c.Course_Name from Degree d,Department de,course c where d.Degree_Code='" + degCode + "' and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id";
                                     DataSet deb = da.select_method_wo_parameter(degdept, "text");
                                     if (deb.Tables[0].Rows.Count > 0 && deb.Tables.Count > 0)
                                     {
                                         depbranch = Convert.ToString(deb.Tables[0].Rows[0]["Course_Name"]) + "-" + Convert.ToString(deb.Tables[0].Rows[0]["Dept_Name"]);
                                     }

                                    double minintmark = 0;
                                    double maxintmark = 0;
                                    double minextmark = 0;
                                    double maxextmark = 0;
                                    double mintotmark = 0;
                                    double maxtotmark = 0;
                                    string stuflag = string.Empty;
                                    string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                    if (dtregArr == "0")
                                        stuflag = "1";
                                    else
                                        stuflag = "2";

                                    DataTable dtModmark = new DataTable();
                                    if (dtModSett.Rows.Count > 0)
                                    {
                                        dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                        dtModmark = dtModSett.DefaultView.ToTable();
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}

                                    //Double modMark1 = 0;
                                    Double minModCIA = 0;
                                    Double minModESE = 0;
                                    string elgVal = string.Empty;
                                    if (dtModmark.Rows.Count > 0)
                                    {
                                        //double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                        elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}
                                    string Esc = Convert.ToString(dr["external_mark"]);
                                    double getESC = 0;
                                    double.TryParse(Esc, out getESC);
                                    string CIA = Convert.ToString(dr["internal_mark"]);
                                    double getCIA = 0;
                                    double.TryParse(CIA, out getCIA);
                                    double gettot = 0;
                                    double.TryParse(total, out gettot);

                                    double afterMod = 0;
                                    double NeedMark = 0;
                                    string afterESC = string.Empty;
                                    string aftertot = string.Empty;
                                    string afterResult = string.Empty;

                                    if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && ev2 != "-19" && ev2 != "-19" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                        if (dtMinmax.Rows.Count > 0)
                                        {
                                            minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                            maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                            minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                            maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                            mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                            maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);
                                        }
                                        if (ddlreptype.SelectedItem.ToString() == "Special Moderation")//2 mod
                                        {
                                            if (minModCIA <= getCIA && minModESE <= getESC)
                                            {
                                                if (gettot < mintotmark)
                                                {
                                                    if (mintotmark >= gettot)
                                                        NeedMark = mintotmark - gettot;
                                                    if (NeedMark > 0)
                                                    {
                                                        if (NeedMark <= modMark)//Round off Mod
                                                        {
                                                            double chkminESC = NeedMark + getESC;
                                                            if (chkminESC >= minextmark)
                                                            {
                                                                afterMod = NeedMark + getESC;
                                                                afterESC = NeedMark.ToString();
                                                                aftertot = (NeedMark + gettot).ToString();
                                                                afterResult = "Pass";
                                                                countval = countval + 1;
                                                            }
                                                            else
                                                            {
                                                                NeedMark = minextmark - getESC;
                                                                if (NeedMark <= modMark)
                                                                {
                                                                    double chkmintot = NeedMark + getESC + getCIA;
                                                                    if (chkmintot >= mintotmark)
                                                                    {
                                                                        afterMod = NeedMark + getESC;
                                                                        afterESC = NeedMark.ToString();
                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                        afterResult = "Pass";
                                                                        countval = countval + 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    if (getESC < minextmark)
                                                    {
                                                        NeedMark = minextmark - getESC;
                                                        if (mintotmark <= gettot + NeedMark && NeedMark + getESC >= minextmark)
                                                        {
                                                            afterMod = NeedMark + getESC;
                                                            afterESC = NeedMark.ToString();
                                                            aftertot = (NeedMark + gettot).ToString();
                                                            afterResult = "Pass";
                                                            countval = countval + 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                    if (result.ToLower() == "pass")
                                        bfpass = bfpass + 1;
                                    if (afterResult.ToLower() == "pass")
                                        afpass = afpass + 1;
                                }
                                int totoStudent = dicSubStud.Rows.Count;
                                bfpar =Convert.ToDouble(bfpass) / Convert.ToDouble(totoStudent);
                                bfpar = bfpar * 100;
                                bfpar = Math.Round(bfpar, 2, MidpointRounding.AwayFromZero);
                                afper = (Convert.ToDouble(bfpass + afpass) / Convert.ToDouble(totoStudent)) * 100;
                                afper = Math.Round(afper, 2, MidpointRounding.AwayFromZero);

                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, i+1].HorizontalAlign = HorizontalAlign.Center;
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(bfpar);
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjectCode1);
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subname);
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(depbranch);
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, i].Text = Convert.ToString(countval);
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, i + 1].Text = Convert.ToString(afper);

                                lastcol=i+1;
                                i = i - 2;
                                }
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, lastcol+1].CellType = txt;
                                fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, lastcol + 1].Locked = false;
                            }
                        }
                    
                        else
                        {
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Record Found";
                            fpspread.Visible = false;
                            btnsave1.Visible = false;
                            btnprintt.Visible = false;
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                        }
                        
                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Record Found";
                    fpspread.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    lblBeMod.Visible = false;
                    lblAfMod.Visible = false;
                }
            }
            fpspread2.Sheets[0].PageSize = fpspread2.Sheets[0].RowCount;
            //fpspread2.Sheets[0].AutoPostBack = false;
            fpspread2.Width = width;
            fpspread2.Height = 600;
            fpspread2.SaveChanges();

        }
        catch
        {

        }
    }  

    public void General()
    {
        try
        {
            Button2.Visible = true;
            //Button1.Visible = true;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            lblerr1.Visible = false;
            int from = 0;
            int to = 0;
            int.TryParse(txtfrom.Text, out from);
            int.TryParse(txtTo.Text, out to);
            fpspread2.Sheets[0].RowCount = 0;
            fpspread2.Sheets[0].ColumnCount = 0;
            fpspread2.Sheets[0].ColumnHeader.RowCount = 4;
            fpspread2.Visible = true;
            fpspread2.RowHeader.Visible = false;
            fpspread2.CommandBar.Visible = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            //fpspread2.Sheets[0].FrozenColumnCount = 2;
            int width = 300;
            //MyStyle.Font.Name = "Book Antiqua";
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 150;

            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Moderation Mark";
            fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 4, 1);

            fpspread2.Sheets[0].ColumnCount++;
            fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "% of Pass StudentsBefore Moderation";
            fpspread2.Sheets[0].ColumnHeader.Cells[2, 1].Text = "% of Pass StudentsAfter Moderation";

            int colcount = fpspread2.Sheets[0].ColumnCount;
            int sapncol = 0;
            int n = 0;
            for (int i = 0; i < cblBatch.Items.Count; i++)
            {
                if (cblBatch.Items[i].Selected)
                {
                    n = n + 2;
                    sapncol++;
                    fpspread2.Sheets[0].ColumnCount = colcount;
                    fpspread2.Sheets[0].ColumnHeader.Cells[3, n-1].Text = Convert.ToString(cblBatch.Items[i].Text);
                    colcount++;
                    fpspread2.Sheets[0].ColumnCount = colcount;
                    fpspread2.Sheets[0].ColumnHeader.Cells[3, n].Text = "Pass %";
                    colcount++;
                    width = width + 100;
                    fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(1, n-1, 1, 2);
                }
               
            }

                fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, colcount);
                fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, colcount);
                fpspread2.Sheets[0].PageSize = fpspread2.Sheets[0].RowCount;
                fpspread2.Sheets[0].AutoPostBack = false;
                fpspread2.Width = width;
                fpspread2.Height = 600;
                fpspread2.SaveChanges();

                //if (false)
                //{

                int markround = 0;
                lblerr1.Visible = false;
                DataSet ds2 = new DataSet();
                DataSet ds1 = new DataSet();
                string subjectCodeNew = string.Empty;
                string valBatch = string.Empty;
                string valDegree = string.Empty;
                string sem = string.Empty;
                string BatchYear = string.Empty;
                collegeCode = string.Empty;
                string sql = string.Empty;
                DataTable dtsubject = new DataTable();

                if (ddlCollege.Items.Count > 0)
                    collegeCode = ddlCollege.SelectedValue.ToString().Trim();

                if (cblBranch.Items.Count > 0)
                    valDegree = rs.getCblSelectedValue(cblBranch);
                if (cbl_sem.Items.Count > 0)
                    sem = rs.getCblSelectedValue(cbl_sem);
                if (cblBatch.Items.Count > 0)
                    BatchYear = rs.getCblSelectedValue(cblBatch);

                if (string.IsNullOrEmpty(BatchYear))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Batch";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                if (string.IsNullOrEmpty(valDegree))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Degree";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                if (string.IsNullOrEmpty(sem))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Semester";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
                ds.Clear();
                ddlSubject.Items.Clear();

                if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(BatchYear))//
                {
                    string qeryss = "SELECT distinct s.subject_name,s.subject_code FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester in('" + sem + "')  and sy.batch_year in('" + BatchYear + "') and d.Degree_Code in('" + valDegree + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and d.college_code='" + collegeCode + "'";
                    dtsubject = dirAcc.selectDataTable(qeryss);
                }
                if (dtsubject.Rows.Count > 0)
                {
                    foreach (DataRow dtsub in dtsubject.Rows)
                    {
                        string subjectCode = Convert.ToString(dtsub["subject_code"]);
                        if (string.IsNullOrEmpty(subjectCodeNew))
                            subjectCodeNew = "'" + subjectCode + "'";
                        else
                            subjectCodeNew = subjectCodeNew + "," + "'" + subjectCode + "'";
                    }
                }

                string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
                if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
                {
                    int num = 0;
                    if (int.TryParse(getmarkround, out num))
                    {
                        markround = Convert.ToInt32(getmarkround);
                    }
                }
                double modMark = 0;
                if (!string.IsNullOrEmpty(txtMod.Text))
                    double.TryParse(txtMod.Text, out modMark);

                string degreeval = string.Empty;
                string degreevalregmoder = string.Empty;
                string degreevalttab = string.Empty;
                string degreevalregis = string.Empty;
                Hashtable hatStudntMark = new Hashtable();

                degreeval = " and ed.degree_code in('" + valDegree + "')";
                degreevalregmoder = " and M.degree_code in('" + valDegree + "')";
                degreevalttab = " and e.degree_code in('" + valDegree + "')";
                degreevalregis = " and r.degree_code in('" + valDegree + "')";

                //Moderation Settings
                DataTable dtModSett = new DataTable();
                string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

                dtModSett = dirAcc.selectDataTable(strMod);
                //---------------------

                if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
                {
                    string qeryss = string.Empty;
                    qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no and  r.Batch_Year in('" + BatchYear + "') " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                    ds1 = da.select_method_wo_parameter(qeryss, "text");

                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        string subject_no = subjectCodeNew;
                        string getdetails = string.Empty;
                        //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                        // string sem = ddlsem1.SelectedValue.ToString();
                        if (false)
                        {
                            //getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "' order by r.reg_no";
                        }

                        else
                        {
                            getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.batch_year in('" + BatchYear + "') " + degreeval + " and s.subject_code in(" + subjectCodeNew + ") and r.college_code='" + CollegeCode + "' order by r.reg_no";
                        }

                        getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in(" + subjectCodeNew + ") " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                        getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in(" + subjectCodeNew + ")";

                        ds2 = da.select_method_wo_parameter(getdetails, "Text");

                        int countval = 0;
                        int colval = 0;
                        Double bfpass = 0;
                        Double afpass = 0;
                        Double bfperc = 0;
                        Double afperc = 0;

                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            for (int s = from; s <= to; s++)
                            {
                                hatStudntMark.Clear();
                                modMark = s;
                                fpspread2.Sheets[0].RowCount = fpspread2.Sheets[0].RowCount + 1;
                                colval = 0;
                                countval = 0;
                                for (int c = 0; c < cblBatch.Items.Count; c++)
                                {

                                    if (cblBatch.Items[c].Selected)
                                    {
                                        colval++;
                                        string batch1 = Convert.ToString(cblBatch.Items[c].Text);
                                        countval = 0;
                                        ds2.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch1 + "'";
                                        DataTable dicBatch = ds2.Tables[0].DefaultView.ToTable();

                                        if (dicBatch.Rows.Count > 0)
                                        {
                                            bfpass = 0;
                                            afpass = 0;
                                            bfperc = 0;
                                            afperc = 0;
                                            foreach (DataRow dr in dicBatch.Rows)
                                            {
                                                string regNo = Convert.ToString(dr["reg_no"]);
                                                string rollNo = Convert.ToString(dr["roll_no"]);
                                                string ev1 = Convert.ToString(dr["internal_mark"]);
                                                string ev2 = Convert.ToString(dr["external_mark"]);
                                                string result = Convert.ToString(dr["result"]);
                                                string total = Convert.ToString(dr["total"]);
                                                string batch = Convert.ToString(dr["Batch_Year"]);
                                                string degCode = Convert.ToString(dr["degree_code"]);
                                                string examCode = Convert.ToString(dr["exam_code"]);
                                                string SubjectNo = Convert.ToString(dr["subject_no"]);
                                                string cursem = Convert.ToString(dr["Current_Semester"]);

                                                #region settings
                                                string stuflag = string.Empty;
                                                string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                                if (dtregArr == "0")
                                                    stuflag = "1";
                                                else
                                                    stuflag = "2";
                                                DataTable dtModmark = new DataTable();
                                                if (dtModSett.Rows.Count > 0)
                                                {
                                                    dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                                    dtModmark = dtModSett.DefaultView.ToTable();
                                                }
                                                
                                                //Double modMark = 0;
                                                Double minModCIA = 0;
                                                Double minModESE = 0;
                                                string elgVal = string.Empty;
                                                if (dtModmark.Rows.Count > 0)
                                                {
                                                    //double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                                    double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                                    double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                                    elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                                }
                                               
                                                #endregion

                                                double minintmark = 0;
                                                double maxintmark = 0;
                                                double minextmark = 0;
                                                double maxextmark = 0;
                                                double mintotmark = 0;
                                                double maxtotmark = 0;

                                                string Esc = Convert.ToString(dr["external_mark"]);
                                                double getESC = 0;
                                                double.TryParse(Esc, out getESC);
                                                string CIA = Convert.ToString(dr["internal_mark"]);
                                                double getCIA = 0;
                                                double.TryParse(CIA, out getCIA);
                                                double gettot = 0;
                                                double.TryParse(total, out gettot);

                                                double afterMod = 0;
                                                double NeedMark = 0;
                                                string afterESC = string.Empty;
                                                string aftertot = string.Empty;
                                                string afterResult = string.Empty;

                                                if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                                {
                                                    ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                                    DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                                    minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                                    maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                                    minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                                    maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                                    mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                                    maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                                    if (ddlreptype.SelectedItem.ToString() == "Genral Moderation")//2 mod
                                                    {
                                                        if (minModESE <= getESC && minModCIA <= getCIA)
                                                        {
                                                            if (gettot < mintotmark || getESC < minextmark)
                                                            {
                                                                if (mintotmark > gettot)
                                                                    NeedMark = mintotmark - gettot;
                                                                if (NeedMark > 0)
                                                                {
                                                                    if (NeedMark <= modMark)//Round off Mod
                                                                    {
                                                                        double chkminESC = NeedMark + getESC;
                                                                        if (chkminESC >= minextmark)
                                                                        {
                                                                            if (!hatStudntMark.ContainsKey(regNo))
                                                                            {
                                                                                hatStudntMark.Add(regNo, NeedMark);
                                                                                afterMod = NeedMark + getESC;
                                                                                afterESC = NeedMark.ToString();
                                                                                aftertot = (NeedMark + gettot).ToString();
                                                                                afterResult = "Pass";
                                                                                countval = countval + 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                                double chkmod = NeedMark + mark;
                                                                                if (chkmod > modMark)
                                                                                {

                                                                                }
                                                                                else
                                                                                {
                                                                                    hatStudntMark[regNo] = chkmod;
                                                                                    afterMod = NeedMark + getESC;
                                                                                    afterESC = NeedMark.ToString();
                                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                                    afterResult = "Pass";
                                                                                    countval = countval + 1;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (mintotmark > gettot)
                                                                                NeedMark = mintotmark - gettot;
                                                                            if (NeedMark <= modMark)
                                                                            {
                                                                                double chkmintot = NeedMark + gettot;

                                                                                if (chkmintot >= mintotmark && getESC >= minextmark)
                                                                                {
                                                                                    if (!hatStudntMark.ContainsKey(regNo))
                                                                                    {
                                                                                        hatStudntMark.Add(regNo, NeedMark);
                                                                                        afterMod = NeedMark + getESC;
                                                                                        afterESC = NeedMark.ToString();
                                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                                        afterResult = "Pass";
                                                                                        countval = countval + 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                                        double chkmod = NeedMark + mark;
                                                                                        if (chkmod > modMark)
                                                                                        {

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            hatStudntMark[regNo] = chkmod;
                                                                                            afterMod = NeedMark + getESC;
                                                                                            afterESC = NeedMark.ToString();
                                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                                            afterResult = "Pass";
                                                                                            countval = countval + 1;
                                                                                        }
                                                                                    }

                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        NeedMark = 0;
                                                                        if (getESC < minextmark)
                                                                        {
                                                                            NeedMark = minextmark - getESC;
                                                                            double chkminESC = NeedMark + getESC;
                                                                            if (NeedMark <= modMark)
                                                                            {
                                                                                if (chkminESC >= minextmark)
                                                                                {
                                                                                    if (!hatStudntMark.ContainsKey(regNo))
                                                                                    {
                                                                                        hatStudntMark.Add(regNo, NeedMark);
                                                                                        afterMod = NeedMark + getESC;
                                                                                        afterESC = NeedMark.ToString();
                                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                                        afterResult = "Pass";
                                                                                        countval = countval + 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        double mark = Convert.ToDouble(hatStudntMark[regNo]);
                                                                                        double chkmod = NeedMark + mark;
                                                                                        if (chkmod > modMark)
                                                                                        {

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            hatStudntMark[regNo] = chkmod;
                                                                                            afterMod = NeedMark + getESC;
                                                                                            afterESC = NeedMark.ToString();
                                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                                            afterResult = "Pass";
                                                                                            countval = countval + 1;
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
                                                }
                                                if (afterResult.ToLower() == "pass")
                                                    afpass = afpass + 1;
                                                if (result.ToLower() == "pass")
                                                    bfpass = bfpass + 1;
                                            }
                                            double totalstud = dicBatch.Rows.Count;
                                            bfperc = bfpass / totalstud * 100;
                                            afperc = afpass + bfpass / totalstud * 100;
                                            afperc = Math.Round(afperc, 2, MidpointRounding.AwayFromZero);
                                            bfperc = Math.Round(bfperc, 2, MidpointRounding.AwayFromZero);
                                        }

                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, colval + 1].HorizontalAlign = HorizontalAlign.Center;

                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, 0].Text = "+" + s.ToString();
                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, colval].Text = countval.ToString();
                                        fpspread2.Sheets[0].Cells[fpspread2.Sheets[0].RowCount - 1, colval + 1].Text = afperc.ToString();
                                        fpspread2.Sheets[0].ColumnHeader.Cells[1, colval].Text = bfperc.ToString();
                                        colval++;
                                        lblerr1.Visible = false;
                                        fpspread2.Sheets[0].PageSize = fpspread2.Sheets[0].RowCount;
                                        fpspread2.SaveChanges();
                                        fpspread2.Visible = true;
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Record Found";
                            fpspread2.Visible = false;

                        }
                    }
                    else
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "No Record Found";
                        fpspread2.Visible = false;

                    }

                }
            //}
        }
        catch
        {

        }
    }

    protected void Button1_click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreptype.SelectedItem.ToString() == "Special Moderation")
            {
                DataSet ds2 = new DataSet();
                DataSet ds1 = new DataSet();
                string subjectCodeNew = string.Empty;
                string examMonth = Convert.ToString(ddlmonth.SelectedValue);
                string examYear = Convert.ToString(ddlyear.SelectedValue);
                string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
                string BatchYear = string.Empty;
                string DegreeCode = string.Empty;
                      
                if (cblsubject.Items.Count > 0)
                    subjectCodeNew = rs.getCblSelectedValue(cblsubject);
                if (cblBranch.Items.Count > 0)
                    DegreeCode = rs.getCblSelectedValue(cblBranch);
                if (cblBatch.Items.Count > 0)
                    BatchYear = rs.getCblSelectedValue(cblBatch);
                if (string.IsNullOrEmpty(BatchYear))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Batch";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                if (string.IsNullOrEmpty(DegreeCode))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Degree";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                if (string.IsNullOrEmpty(subjectCodeNew))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "please Select Subject";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                //Moderation Settings
                DataTable dtModSett = new DataTable();
                string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

                dtModSett = dirAcc.selectDataTable(strMod);
                //---------------------

                string bundleNo = string.Empty;
                int markround = 0;
                string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
                if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
                {
                    int num = 0;
                    if (int.TryParse(getmarkround, out num))
                    {
                        markround = Convert.ToInt32(getmarkround);
                    }
                }
                double modMark = 0;
                string degreeval = string.Empty;
                string degreevalregmoder = string.Empty;
                string degreevalttab = string.Empty;
                string degreevalregis = string.Empty;

                degreeval = " and ed.degree_code in('" + DegreeCode + "')";
                degreevalregmoder = " and M.degree_code in('" + DegreeCode + "')";
                degreevalttab = " and e.degree_code in('" + DegreeCode + "')";
                degreevalregis = " and r.degree_code in('" + DegreeCode + "')";
                fpspread2.SaveChanges();
                int Count = 0;
                for (int i = 0; i < fpspread2.Sheets[0].RowCount; i++)
                {
                    int cellval = fpspread2.Sheets[0].ColumnCount;
                    string colcount = fpspread2.Sheets[0].Cells[i, cellval-1].Text;
                    string subCode = fpspread2.Sheets[0].Cells[i, 0].Text;
                    Double val=0;
                    double.TryParse(colcount,out val);
                    int countval = 0;
                    if (!string.IsNullOrEmpty(colcount) && val > 0 && !string.IsNullOrEmpty(subCode))
                    {
                        subjectCodeNew = subCode;
                        if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
                        {
                            string qeryss = string.Empty;
                            qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year in('" + BatchYear + "') and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                            ds1 = da.select_method_wo_parameter(qeryss, "text");

                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                string subject_no = subjectCodeNew;
                                string getdetails = string.Empty;
                                //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                                // string sem = ddlsem1.SelectedValue.ToString();
                                if (ChkBundlewise.Checked)
                                {
                                    getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code='" + subjectCodeNew + "' and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "'";
                                }

                                else
                                {
                                    getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,ed.exam_code,r.Current_Semester,s.subject_code, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.Batch_Year in('" + BatchYear + "')  " + degreeval + " and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "'";
                                }

                                getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in('" + subjectCodeNew + "') " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                                getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in('" + subjectCodeNew + "') ";

                                ds2 = da.select_method_wo_parameter(getdetails, "Text");

                                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                {
                                    DataTable dicSubjectCode = ds2.Tables[0].DefaultView.ToTable(true, "subject_code");
                                    foreach (DataRow dtrow in dicSubjectCode.Rows)
                                    {
                                        string subjectCode1 = Convert.ToString(dtrow["subject_code"]);
                                        ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode1 + "'";
                                        DataTable dicSubStud = ds2.Tables[0].DefaultView.ToTable();
                                        fpspread2.Sheets[0].RowCount = fpspread2.Sheets[0].RowCount + 1;
                                        
                                            foreach (DataRow dr in dicSubStud.Rows)
                                            {
                                                if (!string.IsNullOrEmpty(txtMod.Text))
                                                    double.TryParse(txtMod.Text, out modMark);
                                                //modMark = val;
                                                string regNo = Convert.ToString(dr["reg_no"]);
                                                string rollNo = Convert.ToString(dr["roll_no"]);
                                                string ev1 = Convert.ToString(dr["internal_mark"]);
                                                string ev2 = Convert.ToString(dr["external_mark"]);
                                                string result = Convert.ToString(dr["result"]);
                                                string total = Convert.ToString(dr["total"]);
                                                string batch = Convert.ToString(dr["Batch_Year"]);
                                                string degCode = Convert.ToString(dr["degree_code"]);
                                                string examCode = Convert.ToString(dr["exam_code"]);
                                                string SubjectNo = Convert.ToString(dr["subject_no"]);
                                                string subjectCode = Convert.ToString(dr["subject_code"]);
                                                string cursem = Convert.ToString(dr["Current_Semester"]);
                                                double minintmark = 0;
                                                double maxintmark = 0;
                                                double minextmark = 0;
                                                double maxextmark = 0;
                                                double mintotmark = 0;
                                                double maxtotmark = 0;
                                                string stuflag = string.Empty;
                                                string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                                if (dtregArr == "0")
                                                    stuflag = "1";
                                                else
                                                    stuflag = "2";

                                                DataTable dtModmark = new DataTable();
                                                if (dtModSett.Rows.Count > 0)
                                                {
                                                    dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                                    dtModmark = dtModSett.DefaultView.ToTable();
                                                }
                                                Double minModCIA = 0;
                                                Double minModESE = 0;
                                                string elgVal = string.Empty;
                                                if (dtModmark.Rows.Count > 0)
                                                {
                                                    //double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                                    double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                                    double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                                    elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                                }
                                               
                                                string Esc = Convert.ToString(dr["external_mark"]);
                                                double getESC = 0;
                                                double.TryParse(Esc, out getESC);
                                                string CIA = Convert.ToString(dr["internal_mark"]);
                                                double getCIA = 0;
                                                double.TryParse(CIA, out getCIA);
                                                double gettot = 0;
                                                double.TryParse(total, out gettot);

                                                double afterMod = 0;
                                                double NeedMark = 0;
                                                string afterESC = string.Empty;
                                                string aftertot = string.Empty;
                                                string afterResult = string.Empty;

                                                if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && ev2 != "-19" && ev2 != "-19" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                                {
                                                    ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                                    DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                                    minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                                    maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                                    minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                                    maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                                    mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                                    maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                                    if (ddlreptype.SelectedItem.ToString() == "Special Moderation")//2 mod
                                                    {
                                                        if (minModCIA <= getCIA && minModESE <= getESC)
                                                        {
                                                            if (gettot < mintotmark)
                                                            {
                                                                if (mintotmark >= gettot)
                                                                    NeedMark = mintotmark - gettot;
                                                                if (NeedMark > 0)
                                                                {
                                                                    if (NeedMark <= modMark)//Round off Mod
                                                                    {
                                                                        double chkminESC = NeedMark + getESC;
                                                                        if (chkminESC >= minextmark)
                                                                        {
                                                                            afterMod = NeedMark + getESC;
                                                                            afterESC = NeedMark.ToString();
                                                                            aftertot = (NeedMark + gettot).ToString();
                                                                            afterResult = "Pass";
                                                                            countval = countval + 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            NeedMark = minextmark - getESC;
                                                                            if (NeedMark <= modMark)
                                                                            {
                                                                                double chkmintot = NeedMark + getESC + getCIA;
                                                                                if (chkmintot >= mintotmark)
                                                                                {
                                                                                    afterMod = NeedMark + getESC;
                                                                                    afterESC = NeedMark.ToString();
                                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                                    afterResult = "Pass";
                                                                                    countval = countval + 1;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (getESC < minextmark)
                                                                {
                                                                    NeedMark = minextmark - getESC;
                                                                    if (mintotmark <= gettot + NeedMark && NeedMark + getESC >= minextmark)
                                                                    {
                                                                        afterMod = NeedMark + getESC;
                                                                        afterESC = NeedMark.ToString();
                                                                        aftertot = (NeedMark + gettot).ToString();
                                                                        afterResult = "Pass";
                                                                        countval = countval + 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                //Save Function----------
                                               
                                                if (afterResult.ToLower() == "pass")
                                                {
                                                    if (NeedMark>0)
                                                    {
                                                        string updateQ = "if exists(select * from moderation where exam_code='" + examCode + "' and subject_no='" + SubjectNo + "' and roll_no='" + rollNo + "' and degree_code='" + degCode + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "') update moderation set roll_no='" + rollNo + "',bf_moderation_extmrk='" + getESC + "',af_moderation_extmrk='" + Convert.ToString(afterMod) + "',passmark='" + NeedMark + "',moderation_mark='" + modMark + "',Modtype='" + ddlreptype.SelectedItem.ToString() + "' where exam_code='" + examCode + "' and subject_no='" + SubjectNo + "' and roll_no='" + rollNo + "' and degree_code='" + degCode + "' and semester='" + cursem + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "' else insert into moderation(batch_year,degree_code,exam_code,subject_no,Semester,roll_no,bf_moderation_extmrk,af_moderation_extmrk,passmark,moderation_mark,exam_month,exam_year,Modtype) values('" + BatchYear + "','" + degCode + "','" + examCode + "','" + SubjectNo + "','" + cursem + "','" + rollNo + "','" + getESC + "','" + Convert.ToString(afterMod) + "','" + Convert.ToString(NeedMark) + "','" + aftertot + "','" + examMonth + "','" + examYear + "','" + ddlreptype.SelectedItem.ToString() + "')";
                                                        Count = dirAcc.updateData(updateQ);
                                                        string UpdateMarkEntry = "update mark_entry SET result='" + afterResult + "',external_mark='" + Convert.ToString(afterESC) + "',total='" + aftertot + "' where roll_no='" + rollNo + "' and exam_code='" + examCode + "' and subject_no='" + SubjectNo + "'";
                                                        Count = dirAcc.updateData(UpdateMarkEntry);
                                                    }
                                                }
                                                //-------------------
                                            }
                                        //}
                                    }
                                }
                                else
                                {
                                    lblerr1.Visible = true;
                                    lblerr1.Text = "No Record Found";
                                }

                            }
                            else
                            {
                                lblerr1.Visible = true;
                                lblerr1.Text = "No Record Found";
                               
                            }
                        }
                    }
                }
                if (Count != 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Saved Sucessfully";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Not Saved";
                    divPopAlert.Visible = true;
                    fpspread2.Visible = false;
                    return;
                }

            }
        }
        catch
        {

        }
    }

    protected void Button2_print(object sender, EventArgs e)
    {
        string degreedetails = Convert.ToString(ddlyear.SelectedItem) + "-" + Convert.ToString(ddlmonth.SelectedItem) + "-" + Convert.ToString(ddlbranch1.SelectedItem) + "-" + Convert.ToString(ddlSubject.SelectedItem);
        if (chkCommon.Checked)
            degreedetails = string.Empty;
        fpspread2.SaveChanges();
        Printcontrol.loadspreaddetails(fpspread2, "ModurationApply.aspx", degreedetails);
        //fpsalary.Sheets[0].Rows[0].Visible = true;
        Printcontrol.Visible = true;
    }
    protected void Button3_print(object sender, EventArgs e)
    {
        string degreedetails = Convert.ToString(ddlyear.SelectedItem) + "-" + Convert.ToString(ddlmonth.SelectedItem) + "-" + Convert.ToString(ddlbranch1.SelectedItem) + "-" + Convert.ToString(ddlSubject.SelectedItem);
        fpspread1.SaveChanges();
        Printcontrol.loadspreaddetails(fpspread1, "ModurationApply.aspx", "Moderation Report");
        //fpsalary.Sheets[0].Rows[0].Visible = true;
        Printcontrol.Visible = true;
    }

    protected void chkMultiple_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkMultiple.Checked)
            {
                chkindividual.Checked = false;
                chkCommon.Checked = false;
                chkMultiple.Checked = true;
                ddlreptype.Items.Clear();
                //ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Special Moderation", "1"));
                //ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Genral Moderation", "2"));
                //ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));
                Div1.Visible = true;
                ddlsem1.Visible = false;
                div2.Visible = true;
                ddlBatch.Visible = false;
                ddldegree1.Visible = false;
                div3.Visible = true;
                ddlbranch1.Visible = false;
                div4.Visible = true;
                txtMod.Visible = true;
                txtfrom.Visible = false;
                txtTo.Visible = false;
                lbldum1.Visible = false;
                ddlSubject.Visible = false;
                Div5.Visible = true;
                clear();

            }
            else
            {
                chkindividual.Checked = true;
                chkCommon.Checked = false;
                ddlreptype.Items.Clear();
                ddlreptype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Round Off Moderation", "1"));
                ddlreptype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Special Moderation", "2"));
                ddlreptype.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Genral Moderation", "3"));
                ddlreptype.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Degree Moderation", "4"));

                Div1.Visible = false;
                ddlsem1.Visible = true;
                div2.Visible = false;
                ddlBatch.Visible = true;
                ddldegree1.Visible = true;
                div3.Visible = false;
                ddlbranch1.Visible = true;
                div4.Visible = false;
                txtMod.Visible = true;
                txtfrom.Visible = false;
                txtTo.Visible = false;
                lbldum1.Visible = false;
                ddlSubject.Visible = true;
                Div5.Visible = false;
                clear();
            }
        }
        catch
        {

        }
    }

    protected void spclMultipleMod()
    {
        try
        {
            clear();
            fpspread1.Visible = false;
            int markround = 0;
            lblerr1.Visible = false;
            DataSet ds2 = new DataSet();
            DataSet ds1 = new DataSet();
            string subjectCodeNew = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;
            collegeCode = string.Empty;
            string sql = string.Empty;
            DataTable dtsubject = new DataTable();
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            if (ddlbranch1.Items.Count > 0)
                valDegree = ddlbranch1.SelectedValue.ToString().Trim();
            if (ddlsem1.Items.Count > 0)
                sem = ddlsem1.SelectedValue.ToString().Trim();

            string CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string BatchYear = Convert.ToString(ddlBatch.SelectedValue);
            string DegreeCode = Convert.ToString(ddlbranch1.SelectedValue);
            string bundleNo = string.Empty;

            if (cblsubject.Items.Count > 0)
            {
                subjectCodeNew = rs.getCblSelectedValue(cblsubject);
            }
            if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(BatchYear))//
            {
                string qeryss = "SELECT distinct s.subject_name,s.subject_code FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + sem + "'  and d.Degree_Code in('" + valDegree + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and s.subject_code in('" + subjectCodeNew + "') and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and d.college_code='" + collegeCode + "'";
                dtsubject = dirAcc.selectDataTable(qeryss);
            }
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }
            double modMark = 0;
            if (!string.IsNullOrEmpty(txtMod.Text))
                double.TryParse(txtMod.Text, out modMark);

            #region Dummy Number Display

            //byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
            //string dummyNumberType = string.Empty;

            //if (DummyNumberType() == 1)
            //{
            //    dummyNumberType = " and subject='" + subjectCodeNew + "' ";
            //}
            //else
            //{
            //    dummyNumberType = " and isnull(subject,'')='' ";
            //}
            //string selDummyQ = string.Empty;

            //selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlmonth.SelectedValue.ToString() + "' and exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' and degreecode='" + ddlbranch1.SelectedValue + "'";

            //DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
            //bool showDummyNumber = ShowDummyNumber();
            //if (showDummyNumber)
            //{
            //    if (dtMappedNumbers.Rows.Count == 0)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
            //        lblAlertMsg.Visible = true;
            //        lblAlertMsg.Text = "No Dummy Numbers Generated";
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            #endregion

            string degreeval = string.Empty;
            string degreevalregmoder = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;
            Hashtable hatStudntMark = new Hashtable();
            if (!ChkBundlewise.Checked)
            {
                degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            }

            //Moderation Settings
            DataTable dtModSett = new DataTable();
            string strMod = "select LinkName,LinkValue,college_code,BatchYear,DegreeCode,Semester,MinCIA,MinESE,value,stuflag from New_ModSettings";

            dtModSett = dirAcc.selectDataTable(strMod);
            //---------------------

            fpspread.Width = 880;
            fpspread.Height = 0;
            fpspread.Visible = true;
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 0;
            fpspread.Sheets[0].ColumnCount = 8;

            fpspread.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;

            //double minicamoderation = 0;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 120;
            fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 150;
            fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 80;

            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
            {
                string qeryss = string.Empty;
                qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no and r.batch_year='" + BatchYear + "' and r.Batch_Year='" + BatchYear + "' " + degreeval + " and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "'  and isnull(r.Reg_No,'') <>'' ";
                ds1 = da.select_method_wo_parameter(qeryss, "text");

                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    //string subject_no = subjectCodeNew;
                    string getdetails = string.Empty;
                    //string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                    // string sem = ddlsem1.SelectedValue.ToString();
                    if (ChkBundlewise.Checked)
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r,exam_seating es  where DATEPART(year,es.edate)='" + ddlyear.SelectedItem.ToString() + "'  and es.regno=r.Reg_No and  s.subject_no=es.subject_no r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "'  and s.subject_code in('" + subjectCodeNew + "') and es.bundle_no='" + bundleNo + "' and r.college_code='" + CollegeCode + "' order by r.reg_no";
                    }

                    else
                    {
                        getdetails = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s,Registration r where r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and r.batch_year='" + BatchYear + "' " + degreeval + " and s.subject_code in('" + subjectCodeNew + "') and r.college_code='" + CollegeCode + "' order by r.reg_no";
                    }

                    getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code in('" + subjectCodeNew + "') " + degreevalregmoder + " and m.exam_year='" + ddlyear.SelectedItem.ToString() + "'";

                    getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlmonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlyear.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code in('" + subjectCodeNew + "')";

                    ds2 = da.select_method_wo_parameter(getdetails, "Text");

                    int height = 50;
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                        //if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "CIA";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "ESC";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "After Moderation";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Result";
                        fpspread.Sheets[0].Columns[0].Visible = true;

                        foreach (DataRow dr1 in dtsubject.Rows)
                        {
                            string subjectCode = Convert.ToString(dr1["subject_code"]);

                            ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                            DataTable dicSubject = ds2.Tables[0].DefaultView.ToTable();
                            if (dicSubject.Rows.Count > 0)
                            {
                                int sno = 0;
                                string subjCode = Convert.ToString(dicSubject.Rows[0]["subject_code"]);
                                string SubName = Convert.ToString(dicSubject.Rows[0]["subject_name"]);
                                fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = subjCode + " - " + SubName;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = Color.LightPink;
                                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 8);
                                foreach (DataRow dr in dicSubject.Rows)
                                {
                                    //if (!string.IsNullOrEmpty(txtMod.Text))
                                    //    double.TryParse(txtMod.Text, out modMark);

                                    string regNo = Convert.ToString(dr["reg_no"]);
                                    string rollNo = Convert.ToString(dr["roll_no"]);
                                    string ev1 = Convert.ToString(dr["internal_mark"]);
                                    string ev2 = Convert.ToString(dr["external_mark"]);
                                    string result = Convert.ToString(dr["result"]);
                                    string total = Convert.ToString(dr["total"]);
                                    string batch = Convert.ToString(dr["Batch_Year"]);
                                    string degCode = Convert.ToString(dr["degree_code"]);
                                    string examCode = Convert.ToString(dr["exam_code"]);
                                    string SubjectNo = Convert.ToString(dr["subject_no"]);
                                    string cursem = Convert.ToString(dr["Current_Semester"]);

                                    string stuflag = string.Empty;
                                    string dtregArr = da.GetFunction("select isnull(attempts,'0') from mark_entry where roll_no='" + rollNo + "' and subject_no='" + SubjectNo + "' order by attempts desc");
                                    if (dtregArr == "0")
                                        stuflag = "1";
                                    else
                                        stuflag = "2";
                                    DataTable dtModmark = new DataTable();
                                    if (dtModSett.Rows.Count > 0)
                                    {
                                        dtModSett.DefaultView.RowFilter = "stuflag='" + stuflag + "' and BatchYear='" + batch + "' and DegreeCode='" + degCode + "' and Semester like '%" + cursem + "%' ";
                                        dtModmark = dtModSett.DefaultView.ToTable();
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}

                                    //Double modMark = 0;
                                    Double minModCIA = 0;
                                    Double minModESE = 0;
                                    string elgVal = string.Empty;
                                    if (dtModmark.Rows.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["LinkValue"]), out modMark);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinCIA"]), out minModCIA);
                                        double.TryParse(Convert.ToString(dtModmark.Rows[0]["MinESE"]), out minModESE);
                                        elgVal = Convert.ToString(dtModmark.Rows[0]["value"]);
                                    }
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Setting Not found')", true);
                                    //}
                                    double minintmark = 0;
                                    double maxintmark = 0;
                                    double minextmark = 0;
                                    double maxextmark = 0;
                                    double mintotmark = 0;
                                    double maxtotmark = 0;

                                    string Esc = Convert.ToString(dr["external_mark"]);
                                    double getESC = 0;
                                    double.TryParse(Esc, out getESC);
                                    string CIA = Convert.ToString(dr["internal_mark"]);
                                    double getCIA = 0;
                                    double.TryParse(CIA, out getCIA);
                                    double gettot = 0;
                                    double.TryParse(total, out gettot);

                                    double afterMod = 0;
                                    double NeedMark = 0;
                                    string afterESC = string.Empty;
                                    string aftertot = string.Empty;
                                    string afterResult = string.Empty;

                                    if (ev1 != "-1" && ev1 != "-2" && ev1 != "-3" && ev1 != "-4" && ev2 != "-1" && ev2 != "-2" && ev2 != "-3" && ev2 != "-4" && !string.IsNullOrEmpty(ev1) && !string.IsNullOrEmpty(ev2))
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = "Reg_no='" + regNo + "'";
                                        DataTable dtMinmax = ds1.Tables[0].DefaultView.ToTable();
                                        minintmark = Convert.ToDouble(dtMinmax.Rows[0]["min_int_marks"]);
                                        maxintmark = Convert.ToDouble(dtMinmax.Rows[0]["max_int_marks"]);
                                        minextmark = Convert.ToDouble(dtMinmax.Rows[0]["min_ext_marks"]);
                                        maxextmark = Convert.ToDouble(dtMinmax.Rows[0]["max_ext_marks"]);
                                        mintotmark = Convert.ToDouble(dtMinmax.Rows[0]["mintotal"]);
                                        maxtotmark = Convert.ToDouble(dtMinmax.Rows[0]["maxtotal"]);

                                        if (ddlreptype.SelectedItem.ToString() == "Special Moderation")//2 mod
                                        {
                                            if (gettot < mintotmark && minModCIA <= getCIA && minModESE <= getESC)
                                            {
                                                if (mintotmark >= gettot)
                                                    NeedMark = mintotmark - gettot;
                                                if (NeedMark > 0)
                                                {
                                                    if (NeedMark <= modMark)//Round off Mod
                                                    {
                                                        double chkminESC = NeedMark + getESC;
                                                        if (chkminESC >= minextmark)
                                                        {
                                                            afterMod = NeedMark + getESC;
                                                            afterESC = NeedMark.ToString();
                                                            aftertot = (NeedMark + gettot).ToString();
                                                            afterResult = "Pass";
                                                        }
                                                        else
                                                        {
                                                            NeedMark = minextmark - getESC;
                                                            if (NeedMark <= modMark)
                                                            {
                                                                double chkmintot = NeedMark + getESC + getCIA;
                                                                if (chkmintot >= mintotmark)
                                                                {
                                                                    afterMod = NeedMark + getESC;
                                                                    afterESC = NeedMark.ToString();
                                                                    aftertot = (NeedMark + gettot).ToString();
                                                                    afterResult = "Pass";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (getESC < minextmark && minModCIA <= getCIA && minModESE <= getESC)
                                                {
                                                    NeedMark = minextmark - getESC;
                                                    if (mintotmark <= gettot + NeedMark && NeedMark + getESC >= minextmark)
                                                    {
                                                        afterMod = NeedMark + getESC;
                                                        afterESC = NeedMark.ToString();
                                                        aftertot = (NeedMark + gettot).ToString();
                                                        afterResult = "Pass";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (ev1 == "-1")
                                    {
                                        ev1 = "AAA";
                                    }
                                    else if (ev1 == "-2")
                                    {
                                        ev1 = "NE";
                                    }
                                    else if (ev1 == "-3")
                                    {
                                        ev1 = "RA";
                                    }
                                    else if (ev1 == "-4")
                                    {
                                        ev1 = "LT";
                                    }
                                    else if (ev1.Trim() != "")
                                    {
                                        //ev1 = ev1;

                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    if (ev2 == "-1")
                                    {
                                        ev2 = "AAA";
                                    }
                                    else if (ev2 == "-2")
                                    {
                                        ev2 = "NE";
                                    }
                                    else if (ev2 == "-3")
                                    {
                                        ev2 = "RA";
                                    }
                                    else if (ev2 == "-4")
                                    {
                                        ev2 = "LT";
                                    }
                                    else if (ev2.Trim() != "")
                                    {
                                        //ev2 = ev2;
                                    }
                                    else
                                    {
                                        ev1 = string.Empty;
                                    }
                                    sno++;
                                    fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dr["Batch_Year"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dr["subject_no"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["reg_no"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["degree_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["roll_no"]);


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;//
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = ev1;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["exam_code"]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dr["Current_Semester"]);

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = ev2;

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = gettot.ToString();

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = (afterESC != "0") ? afterESC : "";

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = (aftertot != "0") ? aftertot : "";


                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                   // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txt;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = (!string.IsNullOrEmpty(afterResult)) ? afterResult : result;

                                    if (!string.IsNullOrEmpty(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text))
                                        fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.SkyBlue;

                                    height = height + 20;
                                }
                            }
                        }
                        btnsave1.Visible = true;
                        btnprintt.Visible = true;
                        lblerr1.Visible = false;
                        fpspread.Height = height;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                        fpspread.SaveChanges();
                        fpspread.Visible = true;
                        if (fpspread.Sheets[0].RowCount > 0)
                        {
                            int bfMod = 0;
                            int afMod = 0;
                            for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string result = Convert.ToString(fpspread.Sheets[0].Cells[i, 7].Text);
                                string Mod = Convert.ToString(fpspread.Sheets[0].Cells[i, 5].Text);
                                if (result.ToLower() == "pass" && string.IsNullOrEmpty(Mod))
                                {
                                    bfMod = bfMod + 1;
                                    afMod = afMod + 1;
                                }
                                else if (result.ToLower() == "pass" && !string.IsNullOrEmpty(Mod))
                                {
                                    afMod = afMod + 1;
                                }
                            }
                            lblBeMod.Visible = true;
                            lblAfMod.Visible = true;
                            lblBeMod.Text = "Before Moderation-" + bfMod;
                            lblAfMod.Text = "After Moderation-" + afMod;
                        }
                        else
                        {
                            lblBeMod.Visible = false;
                            lblAfMod.Visible = false;
                        }


                    }

                    else
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "No Record Found";
                        fpspread.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        lblBeMod.Visible = false;
                        lblAfMod.Visible = false;
                    }

                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Record Found";
                    fpspread.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    lblBeMod.Visible = false;
                    lblAfMod.Visible = false;
                }
            }
        }
        catch
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

   
}