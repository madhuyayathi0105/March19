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

public partial class CoeMod_Supplymentary : System.Web.UI.Page
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
    DAccess2 da=new DAccess2();
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
                bindMonthandYear();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                loadSem();
              
                FpSpread1.Visible = false;
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
        if (cblDegree.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        string SelSem = string.Empty;
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
        {
            SelSem = "select distinct current_semester from Registration where  isnull(cc,0)=0 and isnull(delflag,0)=0  order by Current_Semester";// Batch_Year in('" + valBatch + "') and and degree_code in('" + valDegree + "')
            ds = da.select_method_wo_parameter(SelSem,"text");
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

    public string loadmarkat(string mr)
    {
        string strgetval = "";
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "NE";
        }
        else if (mr == "-3")
        {
            strgetval = "RA";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        else
        {
            strgetval = mr;
        }
        return strgetval;
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          
            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            loadSem();
           
            
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
          
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
       
       
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            loadSem();
          
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
       
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
           string valDegree = string.Empty;
           string valBatch = string.Empty;
           string valSem = string.Empty;
           string collCode = Convert.ToString(ddlCollege.SelectedValue);
           Label1.Visible = false;
           Button2.Visible = false;
           TextBox1.Visible = false;
           Button1.Visible = false;
           DataSet ds1 = new DataSet();
     
           if (cblBatch.Items.Count > 0)
                  valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
           if (cblBranch.Items.Count > 0)
                  valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
           if (cbl_sem.Items.Count > 0)
                  valSem = rs.GetSelectedItemsValueAsString(cbl_sem);

            string query1=string.Empty;
            query1 = "select r.reg_no,r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester,sy.semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total,c.Course_Name,de.Dept_Name  from mark_entry me,Exam_Details ed,subject s,Registration r,syllabus_master sy,Degree d,Department de,course c where c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and r.degree_code=d.degree_code and s.syll_code=sy.syll_code and  r.Roll_No=me.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.exam_code=ed.exam_code and me.subject_no=s.subject_no and r.batch_year=ed.batch_year and ed.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlyear.SelectedValue) + "'  and ed.degree_code in('" + valDegree + "') and  ed.batch_year in('" + valBatch + "')  and  r.college_code='" + collCode + "' and result<>'pass' group by c.Course_Name,de.Dept_Name,r.reg_no,r.degree_code,r.batch_year,s.subject_no,s.subject_code,s.subject_name,ed.exam_code,r.Current_Semester,sy.semester, me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total   order by r.reg_no";//sy.semester=r.Current_Semester and

           ds1 = da.select_method_wo_parameter(query1, "text");

           if (ds1.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                g1btnprint.Visible = false;
                g1btnexcel.Visible = false;
                Printcontrol.Visible = false;
                txtexcelname.Visible = false;
                //lblexportxl.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FpSpread1.Width = 970;
                FpSpread1.Height = 900;
               
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.ForeColor = Color.Black;
                MyStyle.Font.Bold = true;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.CommandBar.Visible = false;
               
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg. No.";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "INT";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "EXT";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Final Mark";
       
                FpSpread1.Sheets[0].Columns[0].Width = 70;
                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Width = 120;
                FpSpread1.Sheets[0].Columns[5].Width = 170;
                FpSpread1.Sheets[0].Columns[6].Width = 70;
                FpSpread1.Sheets[0].Columns[7].Width = 70;
                FpSpread1.Sheets[0].Columns[8].Width = 70;

                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                int sno = 0;
                int maxarrCount = 0;
                int.TryParse(txtMaxArrCount.Text,out maxarrCount);

                if (maxarrCount > 0)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataTable dicStudent = ds1.Tables[0].DefaultView.ToTable(true, "reg_no", "roll_no");
                        foreach (DataRow dt in dicStudent.Rows)
                        {
                            string regNo = Convert.ToString(dt["reg_no"]);
                            string rollNo = Convert.ToString(dt["roll_no"]);
                            ds1.Tables[0].DefaultView.RowFilter = "reg_no='" + regNo + "'";
                            DataTable dtSubjectCount = ds1.Tables[0].DefaultView.ToTable();
                            if (dtSubjectCount.Rows.Count <= maxarrCount)
                            {
                                for (int j = 0; j < dtSubjectCount.Rows.Count; j++)
                                {
                                    string cusem = Convert.ToString(dtSubjectCount.Rows[j]["Current_Semester"]);
                                    string subSem = Convert.ToString(dtSubjectCount.Rows[j]["semester"]);
                                    string subjectCode = Convert.ToString(dtSubjectCount.Rows[j]["subject_code"]);
                                    string SubName = Convert.ToString(dtSubjectCount.Rows[j]["subject_name"]);
                                    string Course = Convert.ToString(dtSubjectCount.Rows[j]["Course_Name"]);
                                    string deptname = Convert.ToString(dtSubjectCount.Rows[j]["Dept_Name"]);
                                    string CIA = loadmarkat(Convert.ToString(dtSubjectCount.Rows[j]["internal_mark"]));
                                    string EXt = loadmarkat(Convert.ToString(dtSubjectCount.Rows[j]["external_mark"]));
                                    string tot = loadmarkat(Convert.ToString(dtSubjectCount.Rows[j]["total"]));


                                    if (subSem.Trim() == cusem.Trim())
                                    {
                                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                        sno++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Course;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = deptname;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regNo;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = subjectCode;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = SubName;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = CIA;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = EXt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = tot;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                                    }
                                }
                            }
                        }
                        FpSpread1.Visible = true;
                        FpSpread1.SaveChanges();
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        Label1.Visible = true;
                        TextBox1.Visible = true;
                        Button1.Visible = true;
                        Button2.Visible = true;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Record Found";
                        divPopAlert.Visible = true;
                        Label1.Visible = false;
                        TextBox1.Visible = false;
                        Button1.Visible = false;
                        Button2.Visible = false;
                        //lblerrormsg.Text = "No Records Found";

                    }
                }
                else
                {
                    FpSpread1.Visible = false;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Max.Arrear Count Not valid!";
                    divPopAlert.Visible = true;
                    Label1.Visible = false;
                    TextBox1.Visible = false;
                    Button1.Visible = false;
                    Button2.Visible = false;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Record Found";
                divPopAlert.Visible = true;
                Label1.Visible = false;
                TextBox1.Visible = false;
                Button1.Visible = false;
                Button2.Visible = false;
                //lblerrormsg.Text = "No Records Found";
               
            }
        }
        catch (Exception ex)
        {
            
        }
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
    
    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox1.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Pls Enter Report Name!";
                divPopAlert.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string rename = "Supplymentary Exam Eligible Students Report";
           
            //string strsubcode = da.GetFunction("Select Subject_code from subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'");
            //string degreedetails = "Office of the Controller of Examinations $" + rename + "" + '@' + "        " + "Batch: " + ddlbatch.SelectedItem.ToString() + "        " + "Degree: " + ddldegree.SelectedItem.ToString() + "        " + "Branch: " + ddldept.SelectedItem.ToString() + "        " + "Semester: " + ddlsem.SelectedItem.ToString() + "        " + "Subject Code: " + strsubcode;
            string pagename = "Supplymentary Exam Eligible Students Report.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, rename);
            Printcontrol.Visible = true;
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



}