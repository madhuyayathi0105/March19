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

public partial class CoeMod_ThirdvaluationReportNew : System.Web.UI.Page
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
                bindSubject();
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
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1  " + qryCollege + qryBatch + " order by r.Batch_Year desc";//and r.cc='0' and delflag='0' and exam_flag<>'debar'
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
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'
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
            SelSem = "select distinct current_semester from Registration where Batch_Year in('" + valBatch + "')  order by Current_Semester";//and degree_code in('" + valDegree + "')
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
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code  AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";//AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'
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

    public void bindSubject()
    {
        try
        {
            cblsubject.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if(cbl_sem.Items.Count>0)
                sem = rs.GetSelectedItemsValueAsString(cbl_sem);

            string sql = string.Empty;

            if ((!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree)) && !string.IsNullOrEmpty(sem))
            {
                //sql = "SELECT distinct Subject_No,Subject_Code,Subject_Name FROM Subject S,Syllabus_Master Y,Exam_Details D where s.syll_code = y.syll_code and y.degree_code = d.degree_code and y.Batch_Year = d.batch_year and y.semester = d.current_semester and d.degree_code in('" + valDegree + "') and d.batch_year in('" + valBatch + "') and d.current_semester in('" + sem + "') and d.exam_code = (select exam_code from Exam_Details where degree_code in('" + valDegree + "') and batch_year in('" + valBatch + "')  and current_semester in('" + sem + "') )";

                sql = "SELECT distinct Subject_Code,Subject_Name,(Subject_Name+'-'+Subject_Code) as subjectDet FROM Subject S,Syllabus_Master Y,Exam_Details D where s.syll_code = y.syll_code and y.degree_code = d.degree_code and y.Batch_Year = d.batch_year and y.semester = d.current_semester and d.degree_code in('" + valDegree + "') and d.batch_year in('" + valBatch + "') and d.current_semester in('" + sem + "') and d.exam_code in(select exam_code from Exam_Details where degree_code in('" + valDegree + "') and batch_year in('" + valBatch + "')  and current_semester in('" + sem + "') )";

                //sql = "SELECT distinct ed.degree_code,ed.batch_year,ed.current_semester,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code in('" + valDegree + "')  and ed.batch_year in('" + valBatch + "') and ed.current_semester in('" + sem + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' order by ed.batch_year,ed.degree_code,ed.current_semester,ead.subject_no ";

                sql = "SELECT distinct s.subject_name,s.Subject_Code,(Subject_Name+'-'+Subject_Code) as subjectDet  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code in('" + valDegree + "')  and ed.batch_year in('" + valBatch + "') and ed.current_semester in('" + sem + "') and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' order by s.subject_name,s.Subject_Code,(Subject_Name+'-'+Subject_Code) ";

                ds = da.select_method_wo_parameter(sql, "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    cblsubject.DataSource = ds;
                    cblsubject.DataTextField = "subjectDet";
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

    public void FoilCard()
    {
        try
        {
            int g = 1;
            string collgr = string.Empty;
            string affilitied = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
          
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = "", hroll = "", bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            string valSubject = string.Empty;
            string valDegree = string.Empty;
            string valBatch = string.Empty;
            string valSem = string.Empty;
            DataSet ds1 = new DataSet();
            string Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
            Double difrak = 0;
            if (Mark_Difference != "")
            {
                difrak = Convert.ToInt16(Mark_Difference);
            }
            if (cblsubject.Items.Count > 0)
                valSubject = rs.GetSelectedItemsValueAsString(cblsubject);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblBranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if (cbl_sem.Items.Count > 0)
                valSem = rs.GetSelectedItemsValueAsString(cbl_sem);
            DataSet dsdisplay = new DataSet();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Font fontCoverNo = new Font("Book Antique", 22, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = da.select_method_wo_parameter(strquery, "Text");
            string sml = da.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            sml = "20";
            if (sml.Trim() == "" || sml.Trim() != "0")
            {
                if (Convert.ToInt32(sml) > 15)
                {
                    tablepadding = 3;
                }
                else
                {
                    tablepadding = 10;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        district = ds.Tables[0].Rows[0]["district"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        string[] aff = affilitied.Split(',');
                        affilitied = aff[0].ToString();
                        int u = 0;
                        string query1 = string.Empty;
                        query1 = "select distinct r.batch_year,r.degree_code,ed.current_semester,s.subject_name,s.subject_code,s.subject_no,r.Reg_No,r.Roll_No,r.Stud_Name,m.evaluation1,m.evaluation2,m.evaluation3,sy.semester,CASE WHEN evaluation1 > evaluation2 THEN evaluation1-evaluation2 WHEN evaluation2 >= evaluation1 THEN evaluation2-evaluation1 END as diff,es.bundle_no from Exam_Details ed,Registration r,mark_entry m,subject s,exam_seating es,syllabus_master sy where s.syll_code=sy.syll_code and ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and r.Roll_No=m.roll_no and ed.exam_code=m.exam_code and m.subject_no=s.subject_no and s.subject_no=es.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and r.Batch_Year in('" + valBatch + "') and r.degree_code in('" + valDegree + "') and ed.current_semester in('" + valSem + "') and s.subject_code in('" + valSubject + "') and r.Reg_No=es.regno and datepart(year,edate)='" + ddlyear.SelectedValue.ToString() + "' and datepart(month,edate)='" + ddlmonth.SelectedValue.ToString() + "' order by r.batch_year,r.degree_code,ed.current_semester,s.subject_name,s.subject_code,s.subject_no,r.Reg_No,r.Roll_No,r.Stud_Name,m.evaluation1,m.evaluation2,m.evaluation3,CASE WHEN evaluation1 > evaluation2 THEN evaluation1-evaluation2 WHEN evaluation2 >= evaluation1 THEN evaluation2-evaluation1 END,es.bundle_no";// and m.subject_no='" + ddlsubject.SelectedValue + "'
                        ds1 = da.select_method_wo_parameter(query1,"Text");
                      
                            isval = 1;
                            u = u + 1;
                            if (ds1.Tables[0].Rows.Count>0)
                            {
                                y = y + 1;
                                chkgenflag = true;
                             
                                PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                collgr = Session["collegecode"].ToString();
                                DataTable BatchList = new DataTable();
                                BatchList = ds1.Tables[0].DefaultView.ToTable(true, "batch_year", "degree_code", "current_semester", "subject_no", "semester");
                                if (BatchList.Rows.Count > 0)
                                {
                                    for (int i = 0; i < BatchList.Rows.Count; i++)
                                    {
                                        string subSem = Convert.ToString(BatchList.Rows[i]["semester"]);
                                        string dicBatch=Convert.ToString(BatchList.Rows[i]["batch_year"]);
                                        string dicDegree=Convert.ToString(BatchList.Rows[i]["degree_code"]);
                                        string dicSem=Convert.ToString(BatchList.Rows[i]["current_semester"]);
                                        string subject_no = Convert.ToString(BatchList.Rows[i]["subject_no"]);
                                        string deg = dirAcc.selectScalarString("select (c.Course_Name+'-'+de.dept_name) as deginfo from Degree d,course c,department de where c.Course_Id=d.Course_Id and d.dept_code=de.dept_code and d.Degree_Code='" + dicDegree + "'");
                                        string subjectCode = dirAcc.selectScalarString("select subject_Code from subject where subject_no='" + subject_no + "'");
                                        string subjectName = dirAcc.selectScalarString("select subject_name from subject where subject_no='" + subject_no + "'");
                                        //string subSem = dirAcc.selectScalarString("select subject_name from syllabus_master where subject_no='" + subject_no + "'");

                                        DataTable dtBundleList = new DataTable();
                                        ds1.Tables[0].DefaultView.RowFilter = "batch_year='" + dicBatch + "' and degree_code='" + dicDegree + "' and current_semester='" + dicSem + "' and subject_no='" + subject_no + "' and diff>='"+difrak+"'";
                                        dtBundleList = ds1.Tables[0].DefaultView.ToTable();
                                        //table1.Columns[4].SetWidth(60);
                                        int row = 0;
                                        int modcount = 1;
                                        if (dtBundleList.Rows.Count > 0)
                                        {

                                            for (ji = 0; ji < dtBundleList.Rows.Count; ji++)
                                            {
                                                coltop = 10;
                                                PdfTextArea ptc;

                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                {
                                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                    mypdfpage.Add(LogoImage, 35, 25, 700);
                                                }
                                                ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                           new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 15;
                                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Office of the Controller of Examinations").ToUpper());
                                                mypdfpage.Add(ptc);

                                                coltop = coltop + 15;
                                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "THIRD VALUATION FOIL CARD FOR THE END SEMESTER EXAMINATIONS" + "-" + Convert.ToString(ddlmonth.SelectedItem.Text).ToUpper() + " " + ddlyear.SelectedItem.Text + "");
                                                mypdfpage.Add(ptc);//FOIL CARD FOR THE END OF SEMESTER EXAMINATIONS-
                                                coltop = coltop + 10;
                                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                mypdfpage.Add(ptc);
                                                int tblcount = 0;
                                                if (Convert.ToInt32(dtBundleList.Rows.Count) > 17)
                                                    tblcount = 16;
                                                else
                                                    tblcount = Convert.ToInt32(dtBundleList.Rows.Count) + 1;
                                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, tblcount, 5, 10);
                                                table1.VisibleHeaders = false;
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetContent("S.No");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetFont(Fontbold);
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetContent("Register Number");
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetFont(Fontbold);

                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetContent("Bundle No");
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetFont(Fontbold);

                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetContent("Marks In Figures");
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetFont(Fontbold);

                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetContent("Marks In Words");
                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetFont(Fontbold);

                                                table1.Columns[0].SetWidth(20);
                                                table1.Columns[1].SetWidth(55);
                                                table1.Columns[1].SetWidth(40);
                                                table1.Columns[3].SetWidth(80);
                                                table1.Columns[4].SetWidth(115);

                                                coltop = coltop + 35;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + deg);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Batch Year");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + dicBatch);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + subjectCode);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + subSem);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, subjectName);
                                                mypdfpage.Add(ptc);

                                                Gios.Pdf.PdfTablePage newpdftabpage1;
                                                //Gios.Pdf.PdfTablePage newpdftabpage2 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 750));
                                                //mypdfpage.Add(newpdftabpage2);
                                              bool newpage=false;
                                                for (ji = 1; ji <= dtBundleList.Rows.Count; ji++)
                                                {
                                                    if (ji % 16 == 0)
                                                    {
                                                        coltop = 10;
                                                        PdfArea pa4 = new PdfArea(mydocument, 14, 12, 566, 821);
                                                        PdfRectangle pr5 = new PdfRectangle(mydocument, pa4, Color.Black);
                                                        mypdfpage.Add(pr5);
                                                        newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 750));
                                                        mypdfpage.Add(newpdftabpage1);
                                                        mypdfpage.SaveToDocument();
                                                        mypdfpage = mydocument.NewPage();
                                                        if (Convert.ToInt32(dtBundleList.Rows.Count) + 2 - (modcount * 16) > 17)
                                                            tblcount = 18;
                                                        else
                                                            tblcount = Convert.ToInt32(dtBundleList.Rows.Count) + 2 - (modcount * 16);
                                                        table1 = mydocument.NewTable(Fontbold, tblcount, 5, 10);
                                                        table1.VisibleHeaders = false;
                                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetContent("S.No");
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetFont(Fontbold);
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetContent("Register Number");
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetFont(Fontbold);

                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetContent("Bundle No");
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetFont(Fontbold);

                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetContent("Marks In Figures");
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetFont(Fontbold);

                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetContent("Marks In Words");
                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetFont(Fontbold);

                                                        table1.Columns[0].SetWidth(20);
                                                        table1.Columns[1].SetWidth(55);
                                                        table1.Columns[1].SetWidth(40);
                                                        table1.Columns[3].SetWidth(80);
                                                        table1.Columns[4].SetWidth(115);
                                                        row = 0;
                                                        modcount++;
                                                        newpage = true;

                                                    }
                                                        string regno = dtBundleList.Rows[ji - 1]["Reg_No"].ToString();
                                                        string name = dtBundleList.Rows[ji - 1]["Stud_Name"].ToString();
                                                        string strbundleNo = dtBundleList.Rows[ji - 1]["bundle_no"].ToString();
                                                        //string roomno = dtBundleList.Rows[ji - 1]["roomno"].ToString();
                                                        //string seatno = dtBundleList.Rows[ji - 1]["seat_no"].ToString();
                                                        table1.Cell(row+1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(row + 1, 0).SetContent(g.ToString());
                                                        table1.Cell(row + 1, 0).SetFont(Fontnormal);
                                                        table1.Cell(row + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(row + 1, 1).SetContent(regno.ToString());
                                                        table1.Cell(row + 1, 1).SetFont(Fontnormal);

                                                        table1.Cell(row + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(row + 1, 2).SetContent(strbundleNo);
                                                        table1.Cell(row + 1, 2).SetFont(Fontnormal);
                                                        g = g + 1;
                                                        row++;
                                                    
                                                }
                                                if (newpage)
                                                {
                                                    newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 60, 550, 750));
                                                    mypdfpage.Add(newpdftabpage1);
                                                }
                                                else
                                                {
                                                    newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 750));
                                                    mypdfpage.Add(newpdftabpage1);
                                                }
                                                mypdfpage.Add(pr1);
                                                PdfTextArea pdfSignExaminer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 750, 200, 50), ContentAlignment.MiddleLeft, "Signature of the Examiner");
                                                mypdfpage.Add(pdfSignExaminer);
                                                PdfTextArea pdfDate = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 775, 200, 50), ContentAlignment.MiddleLeft, "Date\t\t:\t\t");
                                                mypdfpage.Add(pdfDate);
                                                PdfTextArea pdfSignChairman = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 350, 750, 200, 50), ContentAlignment.MiddleRight, "Signature of the Chairman");
                                                mypdfpage.Add(pdfSignChairman);
                                                g = 1;
                                                if (yq >= 180)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = mydocument.NewPage();
                                                    yq = 180;
                                                }
                                            }
                                            string appPath = HttpContext.Current.Server.MapPath("~");
                                            if (appPath != "")
                                            {
                                                string szPath = appPath + "/Report/";
                                                string szFile = "FoilCardSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                                mydocument.SaveToFile(szPath + szFile);
                                                Response.ClearHeaders();
                                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                                Response.ContentType = "application/pdf";
                                                Response.WriteFile(szPath + szFile);
                                            }
                                        }
                                    }
                            }
                                else
                                {
                                    //lblerror1.Visible = true;
                                    //lblerror1.Text = "No Records Found";
                                }
                            }
                        //}
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                if (chkgenflag == false)
                {
                    //lblerror1.Visible = true;
                    //lblerror1.Text = "Please Select Any One Record";
                }
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
          
            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            loadSem();
            bindSubject();
            
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
            string valSubject=string.Empty;
            string valDegree = string.Empty;
            string valBatch = string.Empty;
            string valSem = string.Empty;
            DataSet ds1 = new DataSet();
           if (cblsubject.Items.Count > 0)
               valSubject = rs.GetSelectedItemsValueAsString(cblsubject);
           if (cblBatch.Items.Count > 0)
                  valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
           if (cblBranch.Items.Count > 0)
                  valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
           if (cbl_sem.Items.Count > 0)
                  valSem = rs.GetSelectedItemsValueAsString(cbl_sem);

           string SelectQ = string.Empty;
           DataTable dtOverAll = new DataTable();
           string Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
           Double difrak = 0;
           if (Mark_Difference != "")
           {
             difrak = Convert.ToInt16(Mark_Difference);
           }
            string query1=string.Empty;
            query1 = "select distinct r.batch_year,r.degree_code,ed.current_semester,s.subject_name,s.subject_code,s.subject_no,r.Reg_No,r.Roll_No,r.Stud_Name,m.evaluation1,m.evaluation2,m.evaluation3 from Exam_Details ed,Registration r,mark_entry m,subject s,exam_seating es where ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and r.Roll_No=m.roll_no and ed.exam_code=m.exam_code and m.subject_no=s.subject_no and s.subject_no=es.subject_no and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and r.Batch_Year in('" + valBatch + "') and r.degree_code in('" + valDegree + "') and ed.current_semester in('" + valSem + "') and s.subject_code in('" + valSubject + "') ";// and m.subject_no='" + ddlsubject.SelectedValue + "'//,es.bundle_no

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
                FpSpread1.Sheets[0].ColumnCount = 8;

                //string strsubcode = da.GetFunction("Select Subject_code from subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'");
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject Code : " + strsubcode;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "ExtMark1";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "ExtMark2";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ExtMark3";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Difference";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Remarks";


                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "1";
                if (Session["Rollflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                }

                if (Session["Regflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }

                FpSpread1.Sheets[0].Columns[0].Width = 70;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 150;
                DataTable dtBatchDegree = new DataTable();
                DataTable dicSubject = new DataTable();
                Dictionary<string, string> dicBatchDegree = new Dictionary<string, string>();
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Boolean reportfalg = false;
                dtBatchDegree = ds1.Tables[0].DefaultView.ToTable(true, "batch_year", "degree_code", "current_semester", "subject_no");
                if (dtBatchDegree.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBatchDegree.Rows)
                    {
                        string BatchYear = Convert.ToString(dr["batch_year"]).Trim();
                        string DegreeCode = Convert.ToString(dr["degree_code"]).Trim();
                        string Sems = Convert.ToString(dr["current_semester"]).Trim();
                        string subjectNo = Convert.ToString(dr["subject_no"]).Trim();
                        string subjectName = dirAcc.selectScalarString("select subject_name from subject where subject_no='" + subjectNo + "'");
                        string deg = dirAcc.selectScalarString("select (c.Course_Name+'-'+de.dept_name) as deginfo from Degree d,course c,department de where c.Course_Id=d.Course_Id and d.dept_code=de.dept_code and d.Degree_Code='" + DegreeCode + "'");
                        FpSpread1.Sheets[0].RowCount= FpSpread1.Sheets[0].RowCount+1;
                        string arrangedate =  BatchYear + '/' + deg;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = arrangedate + " " + Sems + " " + "Sem" + " " + subjectName;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);

                int sno = 0;
                DataTable dicSubjectWise=new DataTable();
                ds1.Tables[0].DefaultView.RowFilter = "batch_year='" + BatchYear + "' and degree_code='" + DegreeCode + "' and current_semester='" + Sems + "' and subject_no='" + subjectNo+ "'";
                dicSubjectWise = ds1.Tables[0].DefaultView.ToTable();
                bool rowflag = false;
                if (dicSubjectWise.Rows.Count > 0)
                {
                    rowflag= false;
                    for (int i = 0; i < dicSubjectWise.Rows.Count; i++)
                    {
                        string rollno = dicSubjectWise.Rows[i]["Roll_No"].ToString();
                        string eval1 = dicSubjectWise.Rows[i]["evaluation1"].ToString();
                        string eval2 = dicSubjectWise.Rows[i]["evaluation2"].ToString();
                        string eval3 = dicSubjectWise.Rows[i]["evaluation3"].ToString();

                        //string valu1 = loadmarkat(eval2);
                        //string valu2 = loadmarkat(eval2);

                        Double studiffmark = 0;

                        if (eval1.Trim() != "" && eval2.Trim() != "")
                        {
                            //Double deval = Convert.ToDouble(loadmarkat(eval1));
                            //Double deva2 = Convert.ToDouble(loadmarkat(eval2));

                            Double deval = Convert.ToDouble(eval1);
                            Double deva2 = Convert.ToDouble(eval2);

                            if (deval >= 0 && deva2 >= 0)
                            {
                                if (deval > deva2)
                                {
                                    studiffmark = deval - deva2;
                                }
                                else
                                {
                                    studiffmark = deva2 - deval;
                                }
                            }

                            if (difrak <= studiffmark)
                            {
                                rowflag = true;
                                reportfalg = true;
                                sno++;
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dicSubjectWise.Rows[i]["Reg_No"].ToString();
                                if (deval > 0 && deva2 > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dicSubjectWise.Rows[i]["evaluation1"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dicSubjectWise.Rows[i]["evaluation2"].ToString();
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = loadmarkat(dicSubjectWise.Rows[i]["evaluation1"].ToString());
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = loadmarkat(dicSubjectWise.Rows[i]["evaluation2"].ToString());
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreptype.SelectedItem.Text == "After Evaluation")
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = loadmarkat(dicSubjectWise.Rows[i]["evaluation3"].ToString());
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = studiffmark.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                if (rowflag==false)
                {
                    FpSpread1.Sheets[0].Rows.Remove(FpSpread1.Sheets[0].RowCount - 1, 1);
                }
           }
        }
                if (reportfalg == true)
                {
                    FpSpread1.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    g1btnprint.Visible = true;
                    g1btnexcel.Visible = true;
                    Printcontrol.Visible = false;
                    txtexcelname.Visible = true;
                    //lblexportxl.Visible = true;
                }
                else
                {
                    //FpSpread1.Visible = false;
                    //lblerrormsg.Text = "No Records Found";

                }
                //FpSpread1.Sheets[0].RowCount = 20;
                FpSpread1.Width = 750;
                FpSpread1.Height = 900;
            }
            else
            {
                FpSpread1.Visible = false;
                //lblerrormsg.Text = "No Records Found";
               
            }
        }
        catch (Exception ex)
        {
            
        }
    }
    protected void btnFoilCard_click(object sender, EventArgs e)
    {
        try
        {
            FoilCard();
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
    protected void lblrepttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlreptype.SelectedItem.Text == "Before Evaluation")
        {
            //lblerrormsg.Visible = false;
            //FpSpread1.Visible = false;
            //g1btnexcel.Visible = false;
            //g1btnprint.Visible = false;
            //txtexcelname.Visible = false;
            //lblexportxl.Visible = false;
        }
        else if (ddlreptype.SelectedItem.Text == "After Evaluation")
        {
            //lblerrormsg.Visible = false;
            //FpSpread1.Visible = false;
            //g1btnexcel.Visible = false;
            //g1btnprint.Visible = false;
            //txtexcelname.Visible = false;
            //lblexportxl.Visible = false;
        }

    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                //lblerrormsg.Text = "Please Enter Your Report Name";
                //lblerrormsg.Visible = true;
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
            string rename = "Report Befor III Evaluation";
            if (ddlreptype.SelectedItem.Text == "After Evaluation")
            {
                rename = "Report After III Evaluation";
            }
            //string strsubcode = da.GetFunction("Select Subject_code from subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'");
            //string degreedetails = "Office of the Controller of Examinations $" + rename + "" + '@' + "        " + "Batch: " + ddlbatch.SelectedItem.ToString() + "        " + "Degree: " + ddldegree.SelectedItem.ToString() + "        " + "Branch: " + ddldept.SelectedItem.ToString() + "        " + "Semester: " + ddlsem.SelectedItem.ToString() + "        " + "Subject Code: " + strsubcode;
            string pagename = "ThirdvaluationReportNew.aspx";
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