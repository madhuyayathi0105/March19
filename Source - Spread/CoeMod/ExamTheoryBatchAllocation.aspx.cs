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
using System.Data.SqlClient;
using System.Configuration;

public partial class ExamTheoryBatchAllocation : System.Web.UI.Page
{
    DAccess2 obi_access = new DAccess2();
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
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
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string error = "";
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    string norow = string.Empty;
    string nocol = string.Empty;
    string allotseat = string.Empty;
    int height = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
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
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

                //bindbranch();
            }
            if (!IsPostBack)
            {

                Bindcollege();
                BindCollege();
                examYear();
                examMonth();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                bindSem();

                BindSubject();

            }

        }
        catch (Exception ex)
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
    public void BindCollege()
    {
        // con.Open();
        string cmd = "select collname,college_code from collinfo";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = obi_access.select_method_wo_parameter(cmd, "Text");
        //da.Fill(ds);
        DropDownList1.DataSource = ds;
        DropDownList1.DataTextField = "collname";
        DropDownList1.DataValueField = "college_code";
        DropDownList1.DataBind();
        bindstaffcata(Convert.ToString(DropDownList1.SelectedValue));
        loadstaffdep(Convert.ToString(DropDownList1.SelectedValue));
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
            ddlbatch.Items.Clear();
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
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_Year";
                    ddlbatch.DataValueField = "Batch_Year";
                    ddlbatch.DataBind();
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
            ddldegree1.Items.Clear();
            string batchCode = string.Empty;

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
                columnfield = " and dp.group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;

            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree1.DataSource = ds;
                ddldegree1.DataTextField = "course_name";
                ddldegree1.DataValueField = "course_id";
                ddldegree1.DataBind();
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
            ds.Clear();
            cblDegree.Items.Clear();
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
                columnfield = " and dp.group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);

            if (ddldegree1.Items.Count > 0)
                valDegree = Convert.ToString(ddldegree1.SelectedValue);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "dept_name";
                cblDegree.DataValueField = "degree_code";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void BindSubject()
    {
        try
        {
            string degreecode = string.Empty;
            ds.Clear();
            ddlSubject.Items.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);

            if (cblDegree.Items.Count > 0)
                valDegree = rs.getCblSelectedValue(cblDegree);
            string year = Convert.ToString(ddlExamYear.SelectedValue);
            string month = Convert.ToString(ddlExamMonth.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "select distinct s.subject_code,s.subject_name from subject s,syllabus_master sy,exam_details e,exam_application ea,exam_appl_details ead where s.syll_code=sy.syll_code and s.subject_no=ead.subject_no and e.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and sy.batch_year=e.batch_year and sy.degree_code=e.degree_code   and e.batch_year in('" + valBatch + "') and e.degree_code in('" + valDegree + "') and sy.semester='" + sem + "' and e.exam_month='" + month + "' and e.exam_year='" + year + "' order by   s.subject_code,s.subject_name";//and sy.semester=e.current_semester

                ds = da.select_method_wo_parameter(selBranch, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubject.DataSource = ds;
                    ddlSubject.DataTextField = "subject_name";
                    ddlSubject.DataValueField = "subject_code";
                    ddlSubject.DataBind();

                }
            }

        }
        catch
        {

        }
    }
    public void bindSem()
    {
        string selBranch = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        string subtype = string.Empty;
        ddlsem.ClearSelection();
        ddlsem.Items.Clear();

        if (ddlbatch.Items.Count > 0)
            valBatch = Convert.ToString(ddlbatch.SelectedValue);
        if (cblDegree.Items.Count > 0)
            valDegree = rs.getCblSelectedValue(cblDegree);

        int i = 0;
        Hashtable hat = new Hashtable();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        else
        {
            group_user = Session["group_code"].ToString();
        }
        string strSem =da.GetFunction("select MAX(NDurations) from Ndegree where Degree_code in('" + valDegree + "') and college_code='" + collegecode + "'");
        int currentsem = 0;
        Int32.TryParse(strSem, out currentsem);  //added by prabha on feb 22 2018
        for (i = 1; i <= currentsem; i++)
        {
            ddlsem.Items.Add(i.ToString());
        }
        ddlsem.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));

    }
    public void examMonth()
    {
        try
        {
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string year1 = Convert.ToString(ddlExamYear.SelectedValue);
            ddlExamMonth.Items.Clear();
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'  order by Exam_month desc";
            DataTable dsss = dirAcc.selectDataTable(strsql);
            if (dsss.Rows.Count > 0)
            {
                ddlExamMonth.DataSource = dsss;
                ddlExamMonth.DataTextField = "monthName";
                ddlExamMonth.DataValueField = "Exam_month";
                ddlExamMonth.DataBind();
                ddlExamMonth.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }
    public void examYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            DataTable dsss = dirAcc.selectDataTable(" select distinct Exam_year from exam_details order by Exam_year desc");
            if (dsss.Rows.Count > 0)
            {
                ddlExamYear.DataSource = dsss;
                ddlExamYear.DataTextField = "Exam_year";
                ddlExamYear.DataValueField = "Exam_year";
                ddlExamYear.DataBind();

            }
        }
        catch
        {
        }
    }
    public void SubSubject()
    {
        try
        {
            ddlSubSubject.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            valBatch = Convert.ToString(ddlbatch.SelectedValue);
            string subject = Convert.ToString(ddlSubject.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.getCblSelectedValue(cblDegree);
            string year = Convert.ToString(ddlExamYear.SelectedValue);
            string month = Convert.ToString(ddlExamMonth.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(subject))
            {
                string SubSubjectQ = " select ss.SubPart,ss.SubSubjectID from COESubSubjectPartSettings ss,COESubSubjectPartMater sm where ss.id=sm.id and sm.DegreeCode in('" + valDegree + "') and sm.ExamMonth='" + month + "' and sm.ExamYear='" + year + "' and sm.Semester='" + sem + "' and ss.SubCode='" + subject + "'";

                DataTable dtSubSubject = dirAcc.selectDataTable(SubSubjectQ);
                if (dtSubSubject.Rows.Count > 0)
                {
                    ddlSubSubject.DataSource = dtSubSubject;
                    ddlSubSubject.DataTextField = "SubPart";
                    ddlSubSubject.DataValueField = "SubSubjectID";
                    ddlSubSubject.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        examMonth();
        BindSubject();

    }
    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubject();
        SubSubject();
    }
    protected void ddlSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubject();
        SubSubject();
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            BindSubject();
            SubSubject();
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindSem();
            BindSubject();
            SubSubject();

        }
        catch (Exception ex)
        {
        }
    }
    protected void ddldegree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindSem();
            ddlSubject.Items.Clear();
            BindSubject();
            SubSubject();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindSem();
            ddlSubject.Items.Clear();
            BindSubject();
            SubSubject();
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlSubject.Items.Clear();
            BindSubject();
            SubSubject();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        SubSubject();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GridView1.Visible = false;
            btnSave.Visible = false;
            GridPart.Visible = false;
            Button1.Visible = false;
            GridView4.Visible = false;
            divRange.Visible = false;
            DataTable dtPartAlloc = new DataTable();
            dtPartAlloc.Columns.Add("Date");
            dtPartAlloc.Columns.Add("PartName");
            dtPartAlloc.Columns.Add("Session");
            int noPart = 0;
            DataRow dr = null;
            ddlBatchSel.Items.Clear();
            if (!string.IsNullOrEmpty(txtNoPart.Text))
            {
                int.TryParse(txtNoPart.Text, out noPart);
                if (noPart > 0)
                {
                    for (int i = 1; i <= noPart; i++)
                    {
                        dr = dtPartAlloc.NewRow();
                        dr["Date"] = "";
                        dr["PartName"] = getPartText(i.ToString());
                        dr["Session"] = "";
                        dtPartAlloc.Rows.Add(dr);
                        ddlBatchSel.Items.Insert(i - 1, getPartText(i.ToString()));
                    }
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter No.of batch";
                divPopAlert.Visible = true;
            }
            if (dtPartAlloc.Rows.Count > 0)
            {
                GridPart.DataSource = dtPartAlloc;
                GridPart.DataBind();
                GridPart.Visible = true;
                Button1.Visible = true;

                foreach (GridViewRow gr in GridPart.Rows)
                {
                    DropDownList ddlSession = (gr.FindControl("ddlSession") as DropDownList);
                    TextBox txtDate = (gr.FindControl("txtappldate") as TextBox);
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    ddlSession.Items.Insert(0, "F.N");
                    ddlSession.Items.Insert(1, "A.N");
                    //txtDate.Text=DateTime.
                }
            }
        }
        catch
        {
        }
    }
    private string getPartText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "B1";
                    break;
                case "2":
                    mark = "B2";
                    break;
                case "3":
                    mark = "B3";
                    break;
                case "4":
                    mark = "B4";
                    break;
                case "5":
                    mark = "B5";
                    break;
                case "6":
                    mark = "B6";
                    break;
                case "7":
                    mark = "B7";
                    break;
                case "8":
                    mark = "B8";
                    break;
                case "9":
                    mark = "B9";
                    break;
                case "10":
                    mark = "B10";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        GridView1.Visible = false;
        btnSave.Visible = false;
        GridView4.Visible = false;
        divRange.Visible = false;
        string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
        string examYear = Convert.ToString(ddlExamYear.SelectedValue);
        string examMonth = Convert.ToString(ddlExamMonth.SelectedValue);
        string batchYear = Convert.ToString(ddlbatch.SelectedValue);
        string degreeCode = string.Empty;
        string Sem = Convert.ToString(ddlsem.SelectedValue);
        string subCode = Convert.ToString(ddlSubject.SelectedValue);
        if (cblDegree.Items.Count > 0)
            degreeCode = rs.getCblSelectedValue(cblDegree);

        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(Sem))
        {
            string SelectQ = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.App_No,e.exam_code,s.subject_code,s.subject_name, isnull('','') as Batch from Registration r,Exam_Details e,exam_application ea,exam_appl_details ead,subject s where s.subject_no=ead.subject_no and ea.appl_no=ead.appl_no and r.Roll_No=ea.roll_no and e.exam_code=ea.exam_code and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and Exam_Month='" + examMonth + "' and Exam_year='" + examYear + "'  and r.Batch_Year='" + batchYear + "' and r.degree_code in('" + degreeCode + "') and s.subject_code='" + subCode + "'  order by r.Reg_No";//and e.current_semester='" + Sem + "'
            DataTable dtStudent = dirAcc.selectDataTable(SelectQ);
            if (dtStudent.Rows.Count > 0)
            {
                GridView4.DataSource = dtStudent;
                GridView4.DataBind();
                GridView4.Visible = true;
                divRange.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Student(s) were fount";
                divPopAlert.Visible = true;
            }
        }
        else
        {
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Pls Select All Details";
            divPopAlert.Visible = true;
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
    protected void Btn_range_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_frange.Text == "" || txt_trange.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
                return;
            }

            if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
                return;
            }
            foreach (GridViewRow grid in GridView4.Rows)
            {
                string lblSno = (grid.FindControl("lblQNo") as Label).Text;

                if (lblSno != "")
                {
                    if (Convert.ToInt32(lblSno) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(lblSno) <= Convert.ToInt32(txt_trange.Text))
                    {
                        TextBox txtBatch = (grid.FindControl("txtgMarks") as TextBox);
                        txtBatch.Text = Convert.ToString(ddlBatchSel.SelectedItem.Text);
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GridView1.Visible = false;
            Hashtable hat = new Hashtable();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.getCblSelectedValue(cblDegree);
            string year = Convert.ToString(ddlExamYear.SelectedValue);
            string month = Convert.ToString(ddlExamMonth.SelectedValue);
            string subjectCode = string.Empty;
            if (ddlSubject.Items.Count > 0)
                subjectCode = Convert.ToString(ddlSubject.SelectedValue);
            bool isSaved = false;
            hat.Clear();
            foreach (GridViewRow gr in GridPart.Rows)
            {
                string batch = (gr.FindControl("lblPartNo") as Label).Text;
                string Date = (gr.FindControl("txtappldate") as TextBox).Text;
                string session = (gr.FindControl("ddlSession") as DropDownList).SelectedItem.Text;
                string time = (gr.FindControl("txtTime") as TextBox).Text;
                string inter = (gr.FindControl("txtInternal") as TextBox).Text;
                string exter = (gr.FindControl("txtExternal") as TextBox).Text;
                string lab = (gr.FindControl("txtLabAss") as TextBox).Text;
                string skill = (gr.FindControl("txtSkillAss") as TextBox).Text;


                if (!hat.ContainsKey(batch))
                {
                    hat.Add(batch, Date + "$" + session + "$" + time + "$" + inter + "$" + exter + "$" + skill + "$" + lab);
                }
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "select ea.roll_no, s.subject_code,s.subject_name,ead.subject_no,ea.exam_code from subject s,syllabus_master sy,exam_details e,exam_application ea,exam_appl_details ead where s.syll_code=sy.syll_code and s.subject_no=ead.subject_no and e.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and sy.batch_year=e.batch_year and sy.degree_code=e.degree_code   and e.batch_year in('" + valBatch + "') and e.degree_code in('" + valDegree + "') and e.current_semester='" + sem + "' and e.exam_month='" + month + "' and e.exam_year='" + year + "' and s.subject_code in('" + subjectCode + "') order by   s.subject_code,s.subject_name";//and sy.semester=e.current_semester
                DataTable dicStudent = dirAcc.selectDataTable(selBranch);
                string subSub = Convert.ToString(ddlSubSubject.SelectedValue);
                if (GridView4.Rows.Count > 0)
                {
                    foreach (GridViewRow grid in GridView4.Rows)
                    {
                        string rollNo = (grid.FindControl("lblRollNo") as Label).Text;
                        string AppNo = (grid.FindControl("lblAppNo") as Label).Text;
                        string ExamCode = (grid.FindControl("lblExamCode") as Label).Text;
                        string batch = (grid.FindControl("txtgMarks") as TextBox).Text;
                        dicStudent.DefaultView.RowFilter = "roll_no='" + rollNo + "' and exam_code='" + ExamCode + "'";
                        DataTable dicSubject = dicStudent.DefaultView.ToTable();
                        if (!string.IsNullOrEmpty(batch))
                        {
                            if (dicSubject.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dicSubject.Rows)
                                {
                                    string subNo = Convert.ToString(dr["subject_no"]);
                                    string val = Convert.ToString(hat[batch]);
                                    string Date = string.Empty;
                                    string session = string.Empty;
                                    string timett = string.Empty;
                                    string inte = string.Empty;
                                    string ext = string.Empty;
                                    string lab = string.Empty;
                                    string skill = string.Empty;
                                    string dttime = string.Empty;
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        Date = Convert.ToString(val.Split('$')[0]);
                                        string[] dt = Date.Split('/');
                                        if (dt[0].Length > 1)
                                        {
                                            dt[0] = "0" + dt[0];
                                        }
                                        if (dt[1].Length == 1)
                                        {
                                            dt[1] = "0" + dt[1];
                                        }
                                        dttime = dt[1] + "/" + dt[0] + "/" + dt[2];
                                        session = Convert.ToString(val.Split('$')[1]);
                                        timett = Convert.ToString(val.Split('$')[2]);
                                        inte = Convert.ToString(val.Split('$')[3]);
                                        ext = Convert.ToString(val.Split('$')[4]);
                                        skill = Convert.ToString(val.Split('$')[5]);
                                        lab = Convert.ToString(val.Split('$')[6]);
                                    }
                                    string InsQ = "if exists (select * from examtheorybatch where appno='" + AppNo + "' and examcode='" + ExamCode + "' and subNo='" + subNo + "' and SubSubjectID='" + subSub + "') update examtheorybatch SET Batch='" + batch + "',ExamDate='" + dttime + "',ExamSession='" + session + "',Examtime='" + timett + "',InternalCode='" + inte + "',ExternalCode='" + ext + "',LabAss='" + lab + "',SkillAss='" + skill + "'  where appno='" + AppNo + "' and examcode='" + ExamCode + "' and subNo='" + subNo + "' and SubSubjectID='" + subSub + "' else insert into examtheorybatch(ExamCode,AppNo,SubNo,Batch,ExamDate,ExamSession,Examtime,InternalCode,ExternalCode,LabAss,SkillAss,SubSubjectID) values ('" + ExamCode + "','" + AppNo + "','" + subNo + "','" + batch + "','" + dttime + "','" + session + "','" + timett + "','" + inte + "','" + ext + "','" + lab + "','" + skill + "','" + subSub + "')";
                                    int ExQ = da.update_method_wo_parameter(InsQ, "text");
                                    if (ExQ != 0)
                                        isSaved = true;
                                }
                            }
                        }
                    }
                    if (isSaved)
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Saved !";
                        divPopAlert.Visible = true;
                    }
                    else
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Not Saved !";
                        divPopAlert.Visible = true;
                    }

                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Student(s) were Found !";
                    divPopAlert.Visible = true;
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
            GridView1.Visible = false;
            btnSave.Visible = false;
            GridPart.Visible = false;
            Button1.Visible = false;
            GridView4.Visible = false;
            divRange.Visible = false;
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.getCblSelectedValue(cblDegree);
            string year = Convert.ToString(ddlExamYear.SelectedValue);
            string month = Convert.ToString(ddlExamMonth.SelectedValue);
            string subjectCode = string.Empty;
            if (ddlSubject.Items.Count > 0)
                subjectCode = Convert.ToString(ddlSubject.SelectedValue);
            string subtypeCode = string.Empty;
            string subSub = Convert.ToString(ddlSubSubject.SelectedValue);
            if (!string.IsNullOrEmpty(subSub) && subSub != "0")
                subtypeCode = "   and SubSubjectID='" + subSub + "'";

            string selectQ = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.App_No,e.exam_code,eb.Batch,convert(nvarchar(15),eb.ExamDate,103) as edate,eb.ExamSession,s.subject_code,s.subject_name from Registration r,Exam_Details e,exam_application ea,examtheorybatch eb,subject s where s.subject_no=eb.SubNo and eb.examCode=e.exam_code and r.App_No=eb.appno and r.Roll_No=ea.roll_no and e.exam_code=ea.exam_code and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and Exam_Month='" + month + "' and Exam_year='" + year + "' and e.current_semester='" + sem + "' and r.Batch_Year='" + valBatch + "' and r.degree_code in('" + valDegree + "')  and s.subject_code in('" + subjectCode + "') " + subtypeCode + "  order by r.Reg_No";

            DataTable dtReport = dirAcc.selectDataTable(selectQ);
            if (dtReport.Rows.Count > 0)
            {
                GridView1.DataSource = dtReport;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No record Found!";
                divPopAlert.Visible = true;
            }

        }
        catch
        {
        }
    }
    protected void btnLabl_click(object sender, EventArgs e)
    {
        chkExtOnly.Checked = false;
        rbInternal.Checked = true;
        gviewstaff.Visible = false;
        panel3.Visible = true;
        Button lnkSelected = (Button)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        Session["Row"] = rowIndx;

    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindstaffcata(Convert.ToString(DropDownList1.SelectedValue));
        loadfsstaff();
    }
    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }
    protected void cb_Category_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Category.Checked == true)
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = true;
                txt_Category.Text = "Category(" + (cbl_Category.Items.Count) + ")";
            }
            panel_Category.Focus();
        }
        else
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = false;
                txt_Category.Text = "---Select---";
            }
        }
    }
    protected void cbl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Category.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Category.Items.Count; i++)
        {
            if (cbl_Category.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_Category.Text = "Category(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_Category.Text = "---Select---";
        }
        cb_Category.Checked = false;
    }
    protected void BtnCategory_Click(object sender, EventArgs e)
    {
        try
        {
            //fsstaff.Sheets[0].RowCount = 0; 
            loadfsstaff();
        }
        catch
        {
        }
    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadfsstaff();
    }
    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }
    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            //gviewstaff.Visible = false;
            string staff = string.Empty;
            for (int k = 0; k < gviewstaff.Rows.Count; k++)
            {
                CheckBox chk = gviewstaff.Rows[k].FindControl("selectchk1") as CheckBox;
                if (chk.Checked == true)
                {
                    Label code = (Label)gviewstaff.Rows[k].FindControl("lblstaff");
                    string stafcode = code.Text;
                    Label name = (Label)gviewstaff.Rows[k].FindControl("lblname");
                    string stafname = name.Text;
                    if (string.IsNullOrEmpty(staff))
                        staff = stafcode + "-" + stafname;
                    else
                        staff = staff + ";" + stafcode + "-" + stafname;
                }
            }
            string rno = Convert.ToString(Session["Row"]);
            int Row = 0;
            int.TryParse(rno, out Row);
            if (rbInternal.Checked)
            {
                TextBox txt = (GridPart.Rows[Row].FindControl("txtInternal") as TextBox);
                txt.Text = staff;
            }
            if (rbExternal.Checked)
            {
                TextBox txt = (GridPart.Rows[Row].FindControl("txtExternal") as TextBox);
                txt.Text = staff;
            }
            if (rblSkillAss.Checked)
            {
                TextBox txt = (GridPart.Rows[Row].FindControl("txtSkillAss") as TextBox);
                txt.Text = staff;
            }
            if (rbLabAss.Checked)
            {
                TextBox txt = (GridPart.Rows[Row].FindControl("txtLabAss") as TextBox);
                txt.Text = staff;
                //txt.Enabled = false;
            }

        }
        catch
        {
        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        Session["Row"] = null;
    }
    public void bindstaffcata(string college)
    {
        try
        {

            DataSet ds = new DataSet();
            txt_Category.Text = "---Select---";
            cb_Category.Checked = false;
            string collvalue = college;
            cbl_Category.Items.Clear();
            if (collvalue == "---Select---")
            {
                collvalue = Session["collegecode"].ToString();
            }
            height = 0;
            cbl_Category.Items.Clear();
            ds.Clear();
            ds = obi_access.loadcategory(collvalue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Category.DataSource = ds;
                cbl_Category.DataTextField = "category_name";
                cbl_Category.DataValueField = "Category_Code";
                cbl_Category.DataBind();
                for (int i = 0; i < cbl_Category.Items.Count; i++)
                {
                    cbl_Category.Items[i].Selected = true;
                    height++;
                }
                txt_Category.Text = "Category(" + cbl_Category.Items.Count + ")";
                cb_Category.Checked = true;
            }
            if (height > 10)
            {
                panel_Category.Height = 300;
            }
            else
            {
                panel_Category.Height = 150;
            }
        }
        catch (Exception)
        {
        }
    }
    protected void loadfsstaff()
    {
        DataTable dtable1 = new DataTable();
        DataRow dtrow2 = null;
        string sql = string.Empty;
        string Categorys = rs.GetSelectedItemsValueAsString(cbl_Category);

        if (chkExtOnly.Checked)
        {
            if (txt_search.Text == "")
                sql = "select staff_code,staff_name from external_staff";
            else
            {
                if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "select staff_code,staff_name from external_staff where staff_code='" + txt_search.Text + "'";
                }
                else if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "select staff_code,staff_name from external_staff where staff_name='" + txt_search.Text + "'";
                }
            }
        }
        else
        {
            if (ddldepratstaff.SelectedIndex != 0)
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + DropDownList1.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + DropDownList1.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                    }
                }
                else
                {
                    //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                    sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code  WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + DropDownList1.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";
                }
            }
            else if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + DropDownList1.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + DropDownList1.SelectedValue + "' and  (stafftrans.category_code in('" + Categorys + "')) and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
                else if (DropDownList1.SelectedIndex != -1)
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + DropDownList1.SelectedValue + "'";//Modifed By Srinath 9/5/2013
                }
                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and staffmaster.college_code='" + DropDownList1.SelectedValue + "'";//Modifed By Srinath 9/5/2013
                }
            }
            else
                if (ddldepratstaff.SelectedValue.ToString() == "All")
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster,StaffCategorizer where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + DropDownList1.SelectedValue + "' and StaffCategorizer.category_code= stafftrans.category_code and  stafftrans.category_code in ('" + Categorys + "') and StaffCategorizer.college_code=staffmaster.college_code";
                }
        }

        DataSet dsbindspread = new DataSet();
        dsbindspread = obi_access.select_method_wo_parameter(sql, "Text");

        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;

            dtable1.Columns.Add("Staff_Code");
            dtable1.Columns.Add("Staff_Name");
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                dtrow2 = dtable1.NewRow();
                dtrow2["Staff_Code"] = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();
                dtrow2["Staff_Name"] = name;
                dtable1.Rows.Add(dtrow2);
            }
            gviewstaff.DataSource = dtable1;
            gviewstaff.DataBind();
            gviewstaff.Visible = true;


        }
    }
    public void loadstaffdep(string collegecode)
    {
        //con.Open();
        string cmd = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";
        // SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = obi_access.select_method_wo_parameter(cmd, "Text");
        // da.Fill(ds);
        ddldepratstaff.DataSource = ds;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        ddldepratstaff.Items.Insert(0, "All");
        //con.Close();
        //  bindstaffcata(Convert.ToString(ddlcollege.SelectedValue));
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindSem();
            ddlSubject.Items.Clear();
            BindSubject();
            SubSubject();
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
            bindSem();
            ddlSubject.Items.Clear();
            BindSubject();
            SubSubject();
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