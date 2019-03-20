using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Text;

public partial class MarkMod_InternalMarkReport : System.Web.UI.Page
{
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    SqlCommand cmd;
    string userCode = string.Empty;
    string collegecode = string.Empty;
    string groupUserCode = string.Empty;
    static string grouporusercode = string.Empty;
    bool isBasedOnBatchRights = false;
    static ArrayList arrListVisibleColumn = new ArrayList();
    string grouporusercode1 = string.Empty;
    DataTable data = new DataTable();
    DataRow drow;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        userCode = Session["usercode"].ToString();
        groupUserCode = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                grouporusercode1 = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";

            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                grouporusercode1 = " user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";

            }


            isBasedOnBatchRights = false;
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string batchYearSettings = da.GetFunction("select value from Master_Settings where settings='CAM Entry Based On Batch And Section Rights' and " + grouporusercode + "");
                if (batchYearSettings.Trim() == "1")
                    isBasedOnBatchRights = true;
            }

            Session["StaffSelector"] = "0";
            string check_Stu_Staff_selector = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            if (check_Stu_Staff_selector.Trim() == "1")
            {
                Session["StaffSelector"] = "1";
            }

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Admisionflag"] = "0";
            Session["Appflag"] = "0";

            string masterQry = string.Empty;
            masterQry = "select * from Master_Settings where " + grouporusercode + "";
            DataSet dsMasterSetting = da.select_method_wo_parameter(masterQry, "Text");
            if (dsMasterSetting.Tables.Count > 0 && dsMasterSetting.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsMasterSetting.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Roll No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Register No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Student_Type" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Admission No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                    {
                        Session["Admisionflag"] = "1";
                    }
                    if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Application No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                    {
                        Session["Appflag"] = "1";
                    }
                }
            }
            Showgrid.Visible = false;
            lblnorec.Visible = false;
            tr_printReport.Visible = false;
            lblNote.Visible = false;
            loadCollege();
            loadBatch();
            loadDegree();
            loadDept();
            loadSem();
            loadSec();
            loadTest();
            loadSubject();

        }
    }
    public void loadCollege()
    {
        try
        {
            DataSet dsCollege = new DataSet();
            ddlCollege.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + grouporusercode1 + " and cp.college_code=cf.college_code";  //modified by Mullai
            dsCollege = da.select_method_wo_parameter(selectQuery, "Text");
            if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsCollege;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch
        {
            ddlCollege.Items.Clear();
        }
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
            }

            loadBatch();
            loadDegree();
            loadDept();
            loadSem();
            loadSec();
            loadTest();
            loadSubject();
        }
        catch
        {
        }
    }
    public void loadBatch()
    {
        try
        {
            ds.Clear();
            ddlBatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlBatch.SelectedValue = max_bat.ToString();
                con.Close();
            }
        }
        catch { }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            loadDegree();
            loadDept();
            loadSem();
            loadSec();
            loadTest();
            loadSubject();
            if (ddlDegree.Text != "")
            {
            }
            else
            {
                lblnorec.Text = "Give degree rights to the staff";
                lblnorec.Visible = true;
            }

        }
        catch { }
    }
    public void loadDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            string usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch { }
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlBranch.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string course_id = ddlDegree.SelectedValue.ToString();
            loadDept();
            loadSem();
            loadSec();
            loadTest();
            loadSubject();
        }
        catch { }

    }
    public void loadDept()
    {
        try
        {
            ddlBranch.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch { }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSem();
            loadSec();
            loadTest();
            loadSubject();
        }
        catch (Exception ex)
        {

        }
    }
    public void loadSem()
    {
        try
        {
            ddlSemYr.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            con.Close();
            con.Open();
            SqlDataReader dr;
            cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr[1].ToString());
                duration = Convert.ToInt16(dr[0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                dr.Close();
                SqlDataReader dr1;
                cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
                ddlSemYr.Items.Clear();
                dr1 = cmd.ExecuteReader();
                dr1.Read();
                if (dr1.HasRows == true)
                {
                    first_year = Convert.ToBoolean(dr1[1].ToString());
                    duration = Convert.ToInt16(dr1[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }
                    }
                }
                dr1.Close();
            }

            con.Close();
        }
        catch { }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSec();
            loadTest();
            loadSubject();
        }
        catch { }

    }
    public void loadSec()
    {
        try
        {
            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlBranch.SelectedValue);
            ds = da.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }

        }
        catch { }
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadTest();
            loadSubject();
        }
        catch { }
    }
    public void loadTest()
    {
        try
        {
            ddlTest.Items.Clear();
            string degCode = Convert.ToString(ddlBranch.SelectedValue);
            string batchYear = Convert.ToString(ddlBatch.SelectedValue);
            string sem = Convert.ToString(ddlSemYr.SelectedValue);
            string sec = Convert.ToString(ddlSec.SelectedValue);
            string strSec = "";

            if (sec.Trim().ToLower() == "all" || sec.Trim() == "" || sec == "-1" || sec == null)
            {
                strSec = string.Empty;
            }
            else
            {
                strSec = " and sections='" + sec + "'";
            }
            string qry = "select distinct c.criteria_no,criteria from criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and c.syll_code in(select distinct syll_code from syllabus_master where degree_code='" + degCode + "' and batch_year='" + batchYear + "' and semester='" + sem + "')" + strSec + " order by criteria";

            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "criteria_no";
                ddlTest.DataBind();
            }
        }
        catch { }
    }
    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSubject();
        }
        catch { }
    }
    public void loadSubject()
    {
        try
        {
            cblSub.Items.Clear();
            string degCode = Convert.ToString(ddlBranch.SelectedValue);
            string batchYear = Convert.ToString(ddlBatch.SelectedValue);
            string sem = Convert.ToString(ddlSemYr.SelectedValue);
            string sec = Convert.ToString(ddlSec.SelectedValue);
            string criteriaNo = Convert.ToString(ddlTest.SelectedValue);
            string strSec = "";
            string subDetailsQry = "";

            if (sec.Trim().ToLower() == "all" || sec.Trim() == "" || sec == "-1" || sec == null)
            {
                strSec = string.Empty;
            }
            else
            {
                if (Convert.ToString(Session["Staff_Code"]) != "")
                    strSec = " and ss.sections='" + sec + "'";
                else
                    strSec = " and sections='" + sec + "'";
            }
            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear(degCode, batchYear, sem);
            //if (!cbIsLab.Checked)
            //{
            //    subDetailsQry = " select distinct s.subject_no,s.Subject_name from Subject s,exam_type e,sub_sem ss where ss.subtype_no=s.subtype_no and s.subject_no=e.subject_no and (ss.subject_type not like '%practical%' or isnull(ss.lab,0)<>1) and s.subject_name not like '%lab%' and s.syll_code in(select distinct syll_code from syllabus_master where degree_code='" + degCode + "' and batch_year='" + batchYear + "' and semester='" + sem + "') " + strSec + " and e.criteria_no='" + criteriaNo + "'";
            //}
            //else
            if (Syllabus_year != "-1")
            {
                if (Convert.ToString(Session["Staff_Code"]) == "")
                {
                    subDetailsQry = "select distinct s.subject_no,s.Subject_name from Subject s,exam_type e where s.subject_no=e.subject_no and s.syll_code in(select distinct syll_code from syllabus_master where degree_code='" + degCode + "' and batch_year='" + batchYear + "' and semester='" + sem + "')" + strSec + " and e.criteria_no='" + criteriaNo + "'";
                }
                else
                {
                    subDetailsQry = " select distinct subject.subtype_no,subject_type,subject.subject_no,subject_name,subject_code from subject,sub_sem,staff_selector ss,exam_type e   where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code='" + degCode + "' and semester='" + sem + "' and syllabus_year = '" + Syllabus_year + "' and batch_year = '" + batchYear + "') and subject.subject_no=ss.subject_no and  subject.subject_no=e.subject_no and ss.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strSec + " and e.criteria_no='" + criteriaNo + "' order by subject.subtype_no,subject.subject_no";
                }
            }

            DataSet dsSubDetails = da.select_method_wo_parameter(subDetailsQry, "Text");
            if (dsSubDetails.Tables.Count > 0 && dsSubDetails.Tables[0].Rows.Count > 0)
            {
                cblSub.DataSource = dsSubDetails;
                cblSub.DataTextField = "subject_name";
                cblSub.DataValueField = "subject_no";
                cblSub.DataBind();
            }
            for (int i = 0; i < cblSub.Items.Count; i++)
            {
                cblSub.Items[i].Selected = true;
            }
            txtSub.Text = "Subject " + "(" + cblSub.Items.Count + ")";
            if (cblSub.Items.Count > 0)
                cbSub.Checked = true;
            else
                cbSub.Checked = false;
        }
        catch { }
    }
    protected void cbSub_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbSub, cblSub, txtSub, Label2.Text, "--Select--");
    }
    protected void cblSub_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbSub, cblSub, txtSub, Label2.Text, "--Select--");

    }

    //protected void btnGo_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        loadColumns();
    //        #region Spread design
    //        spreadReport.Sheets[0].ColumnHeader.RowCount = 2;
    //        spreadReport.Sheets[0].ColumnCount = 6;
    //        spreadReport.Sheets[0].RowCount = 0;
    //        spreadReport.CommandBar.Visible = false;
    //        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //        style.Font.Size = 10;
    //        style.Font.Bold = true;
    //        style.Font.Name = "Book Antiqua";
    //        style.HorizontalAlign = HorizontalAlign.Center;
    //        style.ForeColor = Color.Black;
    //        style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //        spreadReport.Sheets[0].ColumnHeader.DefaultStyle = style;
    //        spreadReport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        spreadReport.Sheets[0].AllowTableCorner = true;
    //        spreadReport.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
    //        spreadReport.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No.";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No.";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Type";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Application No.";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No.";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
    //        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
    //        spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
    //        if (!arrListVisibleColumn.Contains("1"))
    //            spreadReport.Sheets[0].Columns[0].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[0].Visible = true;
    //        if (!arrListVisibleColumn.Contains("2"))
    //            spreadReport.Sheets[0].Columns[1].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[1].Visible = true;
    //        if (!arrListVisibleColumn.Contains("3"))
    //            spreadReport.Sheets[0].Columns[2].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[2].Visible = true;
    //        if (!arrListVisibleColumn.Contains("4"))
    //            spreadReport.Sheets[0].Columns[3].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[3].Visible = true;
    //        if (!arrListVisibleColumn.Contains("5"))
    //            spreadReport.Sheets[0].Columns[4].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[4].Visible = true;
    //        if (!arrListVisibleColumn.Contains("6"))
    //            spreadReport.Sheets[0].Columns[5].Visible = false;
    //        else
    //            spreadReport.Sheets[0].Columns[5].Visible = true;

    //        spreadReport.Sheets[0].Columns[0].Width = 120;
    //        spreadReport.Sheets[0].Columns[1].Width = 120;
    //        spreadReport.Sheets[0].Columns[2].Width = 200;
    //        spreadReport.Sheets[0].Columns[3].Width = 130;
    //        spreadReport.Sheets[0].Columns[4].Width = 130;
    //        spreadReport.Sheets[0].Columns[5].Width = 180;

    //        spreadReport.Columns[0].Locked = true;
    //        spreadReport.Columns[1].Locked = true;
    //        spreadReport.Columns[2].Locked = true;
    //        spreadReport.Columns[3].Locked = true;
    //        spreadReport.Columns[4].Locked = true;
    //        spreadReport.Columns[5].Locked = true;

    //        spreadReport.Sheets[0].Columns[0].ForeColor = Color.Black;
    //        spreadReport.Sheets[0].Columns[1].ForeColor = Color.Black;
    //        spreadReport.Sheets[0].Columns[2].ForeColor = Color.Black;
    //        spreadReport.Sheets[0].Columns[3].ForeColor = Color.Black;
    //        spreadReport.Sheets[0].Columns[4].ForeColor = Color.Black;
    //        spreadReport.Sheets[0].Columns[5].ForeColor = Color.Black;

    //        #endregion

    //        if (Session["Rollflag"].ToString() == "0")
    //        {
    //            spreadReport.Sheets[0].ColumnHeader.Columns[0].Visible = false;
    //        }
    //        if (Session["Regflag"].ToString() == "0")
    //        {
    //            spreadReport.Sheets[0].ColumnHeader.Columns[1].Visible = false;
    //        }
    //        if (Session["Studflag"].ToString() == "0")
    //        {
    //            spreadReport.Sheets[0].ColumnHeader.Columns[2].Visible = false;
    //        }
    //        if (Session["Admisionflag"].ToString() == "0")
    //        {
    //            spreadReport.Sheets[0].ColumnHeader.Columns[4].Visible = false;
    //        }
    //        if (Session["Appflag"].ToString() == "0")
    //        {
    //            spreadReport.Sheets[0].ColumnHeader.Columns[3].Visible = false;
    //        }

    //        string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
    //        if (strorderby == "")
    //        {
    //            strorderby = string.Empty;
    //        }
    //        else
    //        {
    //            if (strorderby == "0")
    //            {
    //                strorderby = "ORDER BY registration.Roll_No";
    //            }
    //            else if (strorderby == "1")
    //            {
    //                strorderby = "ORDER BY registration.Reg_No";
    //            }
    //            else if (strorderby == "2")
    //            {
    //                strorderby = "ORDER BY Registration.Stud_Name";
    //            }
    //            else if (strorderby == "0,1,2")
    //            {
    //                strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
    //            }
    //            else if (strorderby == "0,1")
    //            {
    //                strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
    //            }
    //            else if (strorderby == "1,2")
    //            {
    //                strorderby = "ORDER BY registration.Reg_No,Registration.Stud_Name";
    //            }
    //            else if (strorderby == "0,2")
    //            {
    //                strorderby = "ORDER BY registration.Roll_No,Registration.Stud_Name";
    //            }
    //        }

    //        string degCode = Convert.ToString(ddlBranch.SelectedValue);
    //        string batchYear = Convert.ToString(ddlBatch.SelectedValue);
    //        string sem = Convert.ToString(ddlSemYr.SelectedValue);
    //        string sec = Convert.ToString(ddlSec.SelectedValue);
    //        string criteriaNo = Convert.ToString(ddlTest.SelectedValue);
    //        string subNo = Convert.ToString(getCblSelectedValue(cblSub));
    //        string strSec = "";

    //        if (sec.Trim().ToLower() == "all" || sec.Trim() == "" || sec == "-1" || sec == null)
    //        {
    //            strSec = string.Empty;
    //        }
    //        else
    //        {
    //            strSec = " and sections='" + sec + "'";
    //        }

    //        string strstaffselector = string.Empty;
    //        Session["StaffSelector"] = "0";
    //        strstaffselector = string.Empty;
    //        string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
    //        string[] splitminimumabsentsms = staffbatchyear.Split('-');
    //        if (splitminimumabsentsms.Length == 2)
    //        {
    //            int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
    //            if (splitminimumabsentsms[0].ToString() == "1")
    //            {
    //                if (Convert.ToInt32(batchYear) >= batchyearsetting)
    //                {
    //                    Session["StaffSelector"] = "1";
    //                }
    //            }
    //        }
    //        if (Session["StaffSelector"].ToString() == "1")
    //        {
    //            if (Session["Staff_Code"] != null)
    //            {
    //                if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
    //                {
    //                    strstaffselector = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
    //                }
    //            }
    //        }

    //        string qry = "Select distinct registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,ap.app_formno as ApplicationNumber,registration.roll_admit as AdmissionNo,im.subject_no,Q1Mark,Q2Mark,Q3Mark,Q4Mark,DescTotal,QuizMark,AssignmentMark,RecordMark,ObservationMark,LabInternal from registration ,SubjectChooser  ,applyn ap,InternalMarkEntry im where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degCode + "' and Semester = '" + sem + "' and registration.Batch_Year = '" + batchYear + "' and SubjectChooser.Subject_No = im.subject_no " + strSec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  and im.subject_no in ('" + subNo + "') and criteria_no='" + criteriaNo + "' and ap.app_no=im.app_no " + strstaffselector + " " + strorderby + "";

    //        DataSet dsDetails = da.select_method_wo_parameter(qry, "Text");

    //        if (dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
    //        {
    //            string[] arraySubject = subNo.Split(new string[] { "','" }, StringSplitOptions.None);
    //            for (int i = 0; i < arraySubject.Length; i++)
    //            {
    //                string isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(arraySubject[i]) + "')");
    //                string subName = da.GetFunction("select subject_name from subject where subject_no='" + Convert.ToString(arraySubject[i]) + "'");
    //                if (isLab.ToLower().Trim() == "false")
    //                {
    //                    int colCnt = spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnCount - 1;

    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Q1 Mark";
    //                    if (!arrListVisibleColumn.Contains("7"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Q2 Mark";
    //                    if (!arrListVisibleColumn.Contains("8"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Q3 Mark";
    //                    if (!arrListVisibleColumn.Contains("9"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Q4 Mark";
    //                    if (!arrListVisibleColumn.Contains("10"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Descriptive Total";

    //                    if (!arrListVisibleColumn.Contains("11"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Quiz Mark";
    //                    if (!arrListVisibleColumn.Contains("12"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Assignment Mark";
    //                    if (!arrListVisibleColumn.Contains("13"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Grand Total";
    //                    if (!arrListVisibleColumn.Contains("14"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;

    //                    spreadReport.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = subName;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[0, colCnt].Tag = Convert.ToString(arraySubject[i]);
    //                    spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, colCnt, 1, spreadReport.Sheets[0].ColumnCount - colCnt);
    //                }
    //                else
    //                {
    //                    int colCnt = spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnCount - 1;


    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Record Mark";
    //                    if (!arrListVisibleColumn.Contains("15"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Observation Mark";
    //                    if (!arrListVisibleColumn.Contains("16"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Total";
    //                    if (!arrListVisibleColumn.Contains("17"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Internal Mark";
    //                    if (!arrListVisibleColumn.Contains("18"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;
    //                    spreadReport.Sheets[0].ColumnCount++;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[1, spreadReport.Sheets[0].ColumnCount - 1].Text = "Grand Total";
    //                    if (!arrListVisibleColumn.Contains("19"))
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = false;
    //                    else
    //                        spreadReport.Sheets[0].Columns[spreadReport.Sheets[0].ColumnCount - 1].Visible = true;

    //                    spreadReport.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = subName;
    //                    spreadReport.Sheets[0].ColumnHeader.Cells[0, colCnt].Tag = Convert.ToString(arraySubject[i]);
    //                    spreadReport.Sheets[0].ColumnHeaderSpanModel.Add(0, colCnt, 1, spreadReport.Sheets[0].ColumnCount - colCnt);
    //                }
    //            }

    //            Hashtable ht = new Hashtable();
    //            float grandTotal = 0;
    //            int spreadRow = -1;
    //            for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
    //            {
    //                if (!ht.ContainsKey(Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"])))
    //                {
    //                    spreadRow++;
    //                    spreadReport.Sheets[0].RowCount++;
    //                    ht.Add(Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"]), "");
    //                    dsDetails.Tables[0].DefaultView.RowFilter = "app_no='" + Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"]) + "'";
    //                    DataTable dtStuMarks = dsDetails.Tables[0].DefaultView.ToTable();

    //                    string q1Mark = "";
    //                    string q2Mark = "";
    //                    string q3Mark = "";
    //                    string q4Mark = "";
    //                    string descMark = "";
    //                    string quizMark = "";
    //                    string assignmentMark = "";
    //                    string recordMark = "";
    //                    string observationMark = "";
    //                    string internalMark = "";
    //                    float descMarks = 0;
    //                    float quizMarks = 0;
    //                    float assignmentMarks = 0;
    //                    float recordMarks = 0;
    //                    float observationMarks = 0;
    //                    float internalMarks = 0;
    //                    string isLab = "";
    //                    int cnt = 0;
    //                    for (int row = 0; row < dtStuMarks.Rows.Count; row++)
    //                    {

    //                        isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(dtStuMarks.Rows[row]["subject_no"]) + "')");
    //                        if (isLab.ToLower().Trim() == "false")
    //                        {
    //                            q1Mark = Convert.ToString(dtStuMarks.Rows[row]["Q1Mark"]);
    //                            q2Mark = Convert.ToString(dtStuMarks.Rows[row]["Q2Mark"]);
    //                            q3Mark = Convert.ToString(dtStuMarks.Rows[row]["Q3Mark"]);
    //                            q4Mark = Convert.ToString(dtStuMarks.Rows[row]["Q4Mark"]);
    //                            descMark = Convert.ToString(dtStuMarks.Rows[row]["DescTotal"]);
    //                            quizMark = Convert.ToString(dtStuMarks.Rows[row]["QuizMark"]);
    //                            assignmentMark = Convert.ToString(dtStuMarks.Rows[row]["AssignmentMark"]);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["DescTotal"]), out descMarks);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["QuizMark"]), out quizMarks);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["AssignmentMark"]), out assignmentMarks);
    //                        }
    //                        else
    //                        {
    //                            recordMark = Convert.ToString(dtStuMarks.Rows[row]["RecordMark"]);
    //                            observationMark = Convert.ToString(dtStuMarks.Rows[row]["ObservationMark"]);
    //                            internalMark = Convert.ToString(dtStuMarks.Rows[row]["LabInternal"]);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["RecordMark"]).Trim(), out recordMarks);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["ObservationMark"]).Trim(), out observationMarks);
    //                            float.TryParse(Convert.ToString(dtStuMarks.Rows[row]["LabInternal"]).Trim(), out internalMarks);
    //                        }

    //                        for (int m = 6; m < spreadReport.Sheets[0].ColumnCount; m++)
    //                        {
    //                            string sub_no = Convert.ToString(spreadReport.Sheets[0].ColumnHeader.Cells[0, m].Tag);
    //                            if (Convert.ToString(dtStuMarks.Rows[row]["subject_no"]) == sub_no)
    //                            {
    //                                isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(dtStuMarks.Rows[row]["subject_no"]) + "')");
    //                                if (isLab.ToLower().Trim() == "false")
    //                                {
    //                                    grandTotal = descMarks + quizMarks + assignmentMarks;
    //                                    cnt = m;

    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = q1Mark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = q2Mark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = q3Mark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = q4Mark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = descMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = quizMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = assignmentMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = Convert.ToString(grandTotal);
    //                                }
    //                                else
    //                                {
    //                                    float tot = recordMarks + observationMarks;
    //                                    grandTotal = recordMarks + observationMarks + internalMarks;
    //                                    cnt = m;

    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = recordMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = observationMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = Convert.ToString(tot);
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = internalMark;
    //                                    cnt++;
    //                                    spreadReport.Sheets[0].Cells[spreadRow, cnt].Text = Convert.ToString(grandTotal);
    //                                }

    //                            }
    //                        }

    //                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 1].CellType = tt;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 0].CellType = tt;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 0].Text = dtStuMarks.Rows[row]["RollNumber"].ToString();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 1].Text = dtStuMarks.Rows[row]["RegistrationNumber"].ToString();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 2].Text = dtStuMarks.Rows[row]["StudentType"].ToString();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 3].Text = dtStuMarks.Rows[row]["ApplicationNumber"].ToString();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 4].Text = dtStuMarks.Rows[row]["AdmissionNo"].ToString();
    //                        spreadReport.Sheets[0].Cells[spreadRow, 5].Text = dtStuMarks.Rows[row]["Student_Name"].ToString();

    //                        spreadReport.Sheets[0].Cells[spreadRow, 0].HorizontalAlign = HorizontalAlign.Center;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 1].HorizontalAlign = HorizontalAlign.Center;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 2].HorizontalAlign = HorizontalAlign.Left;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 3].HorizontalAlign = HorizontalAlign.Center;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 4].HorizontalAlign = HorizontalAlign.Center;
    //                        spreadReport.Sheets[0].Cells[spreadRow, 5].HorizontalAlign = HorizontalAlign.Left;

    //                    }


    //                }
    //            }
    //            for (int k = 6; k < spreadReport.Sheets[0].ColumnCount; k++)
    //            {
    //                spreadReport.Columns[k].Locked = true;
    //                spreadReport.Sheets[0].Columns[k].ForeColor = Color.Black;

    //            }
    //            spreadReport.Sheets[0].PageSize = spreadReport.Sheets[0].RowCount;
    //            spreadReport.SaveChanges();
    //            spreadReport.Visible = true;
    //            lblnorec.Visible = false;
    //        }
    //        else
    //        {
    //            lblnorec.Visible = true;
    //            lblnorec.Text = "No Record(s) found";
    //            spreadReport.Visible = false;
    //        }

    //    }
    //    catch { }
    //}

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            loadColumnOrder();
            loadColumns();

            Dictionary<int, string> dicsubcode = new Dictionary<int, string>();
            #region Grid design

            ArrayList arrColHdrNames1 = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();
            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");
            data.Columns.Add("SNo", typeof(string));

            data.Columns.Add("Roll No", typeof(string));
            arrColHdrNames1.Add("Roll No");
            arrColHdrNames2.Add("Roll No");

            data.Columns.Add("Reg No", typeof(string));
            arrColHdrNames1.Add("Reg No");
            arrColHdrNames2.Add("Reg No");

            data.Columns.Add("Student Type", typeof(string));
            arrColHdrNames1.Add("Student Type");
            arrColHdrNames2.Add("Student Type");

            data.Columns.Add("Application No", typeof(string));
            arrColHdrNames1.Add("Application No");
            arrColHdrNames2.Add("Application No");

            data.Columns.Add("Admission No", typeof(string));
            arrColHdrNames1.Add("Admission No");
            arrColHdrNames2.Add("Admission No");

            data.Columns.Add("Student Name", typeof(string));
            arrColHdrNames1.Add("Student Name");
            arrColHdrNames2.Add("Student Name");



            #endregion


            string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY registration.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY registration.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY Registration.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,Registration.Stud_Name";
                }
            }

            string degCode = Convert.ToString(ddlBranch.SelectedValue);
            string batchYear = Convert.ToString(ddlBatch.SelectedValue);
            string sem = Convert.ToString(ddlSemYr.SelectedValue);
            string sec = Convert.ToString(ddlSec.SelectedValue);
            string criteriaNo = Convert.ToString(ddlTest.SelectedValue);
            string subNo = Convert.ToString(getCblSelectedValue(cblSub));
            string strSec = "";
            string sec11 = "";
            if (sec.Trim().ToLower() == "all" || sec.Trim() == "" || sec == "-1" || sec == null)
            {
                strSec = string.Empty;
            }
            else
            {
                strSec = " and sections='" + sec + "'";
                sec11 = " and r.sections='" + sec + "'";
            }

            string strstaffselector = string.Empty;
            Session["StaffSelector"] = "0";
            strstaffselector = string.Empty;
            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            string[] splitminimumabsentsms = staffbatchyear.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear) >= batchyearsetting)
                    {
                        Session["StaffSelector"] = "1";
                    }
                }
            }
            if (Session["StaffSelector"].ToString() == "1")
            {
                if (Session["Staff_Code"] != null)
                {
                    if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                    {
                        strstaffselector = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
                    }
                }
            }


            if (arrListVisibleColumn.Count > 0)
            {
                //string qryStuDetails = "select distinct r.roll_no,r.reg_no ,r.app_no ,r.stud_name ,r.Stud_Type ,a.app_formno,r.roll_admit,q.PartNo,q.NO_Ques, q.QNo,m.Marks,re.marks_obtained,q.CriteriaNo,q.subjectNo,m.desctotal from NewInternalMarkEntry m,CAQuesSettingsParent q,registration r,Result re,applyn a     where r.App_No=m.app_no   and r.App_No=a.app_no  and  q.MasterID=m.MasterID and re.roll_no=r.Roll_No and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and q.subjectNo in('" + subNo + "') and q.CriteriaNo='" + criteriaNo + "' and r.Batch_Year='" + batchYear + "' and r.degree_code='" + degCode + "' and r.Current_Semester='" + sem + "'" + strSec + "   ORDER BY r.Reg_No";//and re.exam_code='38789' and re.exam_code='" + examCodeValue + "'
                string qryStuDetails = "select distinct r.roll_no,r.reg_no ,r.app_no ,r.stud_name ,r.Stud_Type ,a.app_formno,r.roll_admit,q.PartNo,q.NO_Ques, q.QNo,m.Marks,re.marks_obtained,q.CriteriaNo,q.subjectNo,m.desctotal from NewInternalMarkEntry m,CAQuesSettingsParent q,registration r,Result re,applyn a,exam_type et    where m.examcode=et.exam_code  and et.criteria_no=q.CriteriaNo and q.subjectNo=et.subject_no  and     Convert(nvarchar,r.App_No)=Convert(nvarchar,m.app_no)   and r.App_No=a.app_no  and  q.MasterID=m.MasterID and re.roll_no=r.Roll_No and isnull(ltrim(rtrim(r.sections)),'')=isnull(ltrim(rtrim(et.sections)),'')  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and et.subject_No in('" + subNo + "') and et.Criteria_No='" + criteriaNo + "' and r.Batch_Year='" + batchYear + "' and r.degree_code='" + degCode + "' and r.Current_Semester='" + sem + "' " + sec11 + "  and et.exam_code=re.exam_code and m.examcode=et.exam_code  ORDER BY r.Reg_No";
                //string qry = "Select distinct registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,ap.app_formno as ApplicationNumber,registration.roll_admit as AdmissionNo,im.subject_no,Q1Mark,Q2Mark,Q3Mark,Q4Mark,DescTotal,QuizMark,AssignmentMark,RecordMark,ObservationMark,LabInternal from registration ,SubjectChooser  ,applyn ap,InternalMarkEntry im where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degCode + "' and Semester = '" + sem + "' and registration.Batch_Year = '" + batchYear + "' and SubjectChooser.Subject_No = im.subject_no " + strSec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  and im.subject_no in ('" + subNo + "') and criteria_no='" + criteriaNo + "' and ap.app_no=im.app_no " + strstaffselector + " " + strorderby + "";

                DataSet dsDetails = da.select_method_wo_parameter(qryStuDetails, "Text");

                if (dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                {
                    string[] arraySubject = subNo.Split(new string[] { "','" }, StringSplitOptions.None);
                    int colcnt1 = 6;
                    for (int i = 0; i < arraySubject.Length; i++)
                    {
                        string isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(arraySubject[i]) + "')");
                        string subName = da.GetFunction("select subject_name from subject where subject_no='" + Convert.ToString(arraySubject[i]) + "'");



                        if (isLab.ToLower().Trim() == "false")
                        {

                            int colCount = 0;
                            for (int j = 0; j < arrListVisibleColumn.Count; j++)
                            {
                                int x = Convert.ToInt32(arrListVisibleColumn[j]);
                                if (x > 5 && x < 14)
                                {
                                    string colHeaderName = cblColumnOrder.Items[x].Text;
                                    System.Text.StringBuilder colname = new System.Text.StringBuilder(colHeaderName);

                                    AddTableColumn(data, colname);


                                    arrColHdrNames1.Add(subName);
                                    arrColHdrNames2.Add(colHeaderName);



                                    colCount++;
                                }
                            }
                            int colCnt = data.Columns.Count;
                            colCnt = colCnt - colCount;
                            dicsubcode.Add(colCnt - 1, Convert.ToString(arraySubject[i]));

                        }
                        else
                        {

                            int colCount = 0;
                            for (int j = 0; j < arrListVisibleColumn.Count; j++)
                            {
                                int x = Convert.ToInt32(arrListVisibleColumn[j]);
                                if (x >= 14)
                                {
                                    string colHeaderName = cblColumnOrder.Items[x].Text;

                                    System.Text.StringBuilder colname = new System.Text.StringBuilder(colHeaderName);

                                    AddTableColumn(data, colname);


                                    arrColHdrNames1.Add(subName);
                                    arrColHdrNames2.Add(colHeaderName);


                                    colCount++;
                                }
                            }

                            int colCnt = data.Columns.Count;
                            colCnt = colCnt - colCount;
                            dicsubcode.Add(colCnt - 1, Convert.ToString(arraySubject[i]));
                        }
                    }


                    DataRow drHdr1 = data.NewRow();
                    DataRow drHdr2 = data.NewRow();

                    for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames1[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];

                    }

                    data.Rows.Add(drHdr1);
                    data.Rows.Add(drHdr2);
                    Hashtable ht = new Hashtable();
                    float grandTotal = 0;
                    int spreadRow = -1;
                    for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
                    {
                        if (!ht.ContainsKey(Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"])))
                        {
                            spreadRow++;
                            drow = data.NewRow();
                            data.Rows.Add(drow);
                            ht.Add(Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"]), "");
                            dsDetails.Tables[0].DefaultView.RowFilter = "app_no='" + Convert.ToString(dsDetails.Tables[0].Rows[i]["app_no"]) + "'";
                            DataTable dtStuMarks = dsDetails.Tables[0].DefaultView.ToTable();

                            string q1Mark = "";
                            string q2Mark = "";
                            string q3Mark = "";
                            string q4Mark = "";
                            string descMark = "";
                            string quizMark = "";
                            string assignmentMark = "";
                            string recordMark = "";
                            string observationMark = "";
                            string internalMark = "";
                            float descMarks = 0;
                            float quizMarks = 0;
                            float assignmentMarks = 0;
                            float recordMarks = 0;
                            float observationMarks = 0;
                            float internalMarks = 0;
                            string isLab = "";
                            int cnt = 0;
                            double grandTotal1 = 0;
                            string grandtot = string.Empty;
                            for (int row = 0; row < dtStuMarks.Rows.Count; row++)
                            {
                                isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(dtStuMarks.Rows[row]["subjectNo"]) + "')");
                                dtStuMarks.DefaultView.RowFilter = "subjectNo='" + Convert.ToString(dtStuMarks.Rows[row]["subjectNo"]) + "'";
                                DataTable dtMarks = dtStuMarks.DefaultView.ToTable();
                                if (dtMarks.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtMarks.Rows)
                                    {
                                        string Qno = Convert.ToString(dr["QNo"]);
                                        if (isLab.ToLower().Trim() == "false" || isLab == "0")
                                        {
                                            if (Qno == "1")
                                                q1Mark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "2")
                                                q2Mark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "3")
                                                q3Mark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "4")
                                                q4Mark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "5")
                                                quizMark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "6")
                                                assignmentMark = Convert.ToString(dr["Marks"]);

                                            descMark = Convert.ToString(dr["DescTotal"]);

                                            float.TryParse(Convert.ToString(dr["DescTotal"]), out descMarks);
                                            if (Qno == "5")
                                                float.TryParse(Convert.ToString(dr["Marks"]), out quizMarks);
                                            if (Qno == "6")
                                                float.TryParse(Convert.ToString(dr["Marks"]), out assignmentMarks);

                                            grandtot = Convert.ToString(dr["marks_obtained"]);
                                        }
                                        else
                                        {
                                            if (Qno == "1")
                                                recordMark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "2")
                                                observationMark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "3")
                                                internalMark = Convert.ToString(dr["Marks"]);
                                            if (Qno == "1")
                                                float.TryParse(Convert.ToString(dr["Marks"]).Trim(), out recordMarks);
                                            if (Qno == "2")
                                                float.TryParse(Convert.ToString(dr["Marks"]).Trim(), out observationMarks);
                                            if (Qno == "3")
                                                float.TryParse(Convert.ToString(dr["Marks"]).Trim(), out internalMarks);

                                            grandtot = Convert.ToString(dr["marks_obtained"]);
                                        }
                                    }
                                }

                                for (int m = 6; m < data.Columns.Count; m++)
                                {
                                    if (dicsubcode.ContainsKey(m))
                                    {
                                        string sub_no = dicsubcode[m];
                                        if (Convert.ToString(dtStuMarks.Rows[row]["subjectNo"]) == sub_no)
                                        {
                                            isLab = da.GetFunction("select lab from sub_sem where subType_no=(select subtype_no from subject where subject_no='" + Convert.ToString(dtStuMarks.Rows[row]["subjectNo"]) + "')");
                                            if (isLab.ToLower().Trim() == "false")
                                            {

                                                if (descMarks == -1 && quizMarks == -1 && assignmentMarks == -1)
                                                    grandTotal = -1;
                                                else
                                                {
                                                    descMarks = checkMark(descMarks);
                                                    quizMarks = checkMark(quizMarks);
                                                    assignmentMarks = checkMark(assignmentMarks);
                                                    grandTotal = descMarks + quizMarks + assignmentMarks;
                                                    grandTotal1 = Math.Round(grandTotal, 0, MidpointRounding.AwayFromZero);
                                                }
                                                if (grandtot.Trim() == "-1" && grandTotal1 == 0)
                                                {
                                                    grandTotal1 = Convert.ToDouble(grandtot);
                                                }


                                                cnt = m;

                                                for (int j = 0; j < arrListVisibleColumn.Count; j++)
                                                {
                                                    int x = Convert.ToInt32(arrListVisibleColumn[j]);
                                                    if (x > 5 && x < 14)
                                                    {

                                                        switch (x)
                                                        {
                                                            case 6:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(q1Mark);

                                                                }
                                                                break;
                                                            case 7:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(q2Mark);
                                                                }
                                                                break;
                                                            case 8:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(q3Mark);
                                                                } break;
                                                            case 9:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(q4Mark);
                                                                } break;
                                                            case 10:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(descMark);
                                                                } break;
                                                            case 11:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(quizMark);
                                                                } break;

                                                            case 12:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(assignmentMark);
                                                                } break;
                                                            case 13:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(Convert.ToString(grandTotal1));
                                                                } break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                float tot = 0;

                                                if (recordMarks == -1 && observationMarks == -1 && internalMarks == -1)
                                                {
                                                    tot = -1;
                                                    grandTotal = -1;
                                                }
                                                else if (recordMarks == -1 && observationMarks == -1 && internalMarks != -1)
                                                {
                                                    tot = -1;
                                                    grandTotal = internalMarks;
                                                    grandTotal1 = Math.Round(grandTotal, 0, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    recordMarks = checkMark(recordMarks);
                                                    observationMarks = checkMark(observationMarks);
                                                    internalMarks = checkMark(internalMarks);

                                                    tot = recordMarks + observationMarks;
                                                    grandTotal = tot + internalMarks;
                                                    grandTotal1 = Math.Round(grandTotal, 0, MidpointRounding.AwayFromZero);
                                                }

                                                cnt = m;

                                                for (int j = 0; j < arrListVisibleColumn.Count; j++)
                                                {

                                                    int x = Convert.ToInt32(arrListVisibleColumn[j]);
                                                    if (x >= 14)
                                                    {

                                                        switch (x)
                                                        {
                                                            case 14:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(recordMark);
                                                                }
                                                                break;
                                                            case 15:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(observationMark);
                                                                } break;
                                                            case 16:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(Convert.ToString(tot));
                                                                } break;
                                                            case 17:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(internalMark);
                                                                } break;
                                                            case 18:
                                                                {
                                                                    cnt++;
                                                                    data.Rows[data.Rows.Count - 1][cnt] = checkStatus(Convert.ToString(grandTotal1));
                                                                } break;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }

                                data.Rows[data.Rows.Count - 1][0] = data.Rows.Count - 2;
                                data.Rows[data.Rows.Count - 1][1] = dtStuMarks.Rows[row]["Roll_no"].ToString();
                                data.Rows[data.Rows.Count - 1][2] = dtStuMarks.Rows[row]["Reg_no"].ToString();
                                data.Rows[data.Rows.Count - 1][3] = dtStuMarks.Rows[row]["Stud_Type"].ToString();
                                data.Rows[data.Rows.Count - 1][4] = dtStuMarks.Rows[row]["app_formno"].ToString();
                                data.Rows[data.Rows.Count - 1][5] = dtStuMarks.Rows[row]["roll_admit"].ToString();
                                data.Rows[data.Rows.Count - 1][6] = dtStuMarks.Rows[row]["stud_name"].ToString();


                            }
                        }
                    }
                    //for (int k = 6; k < spreadReport.Sheets[0].ColumnCount; k++)
                    //{
                    //    spreadReport.Columns[k].Locked = true;
                    //    spreadReport.Sheets[0].Columns[k].ForeColor = Color.Black;

                    //}

                    if (data.Columns.Count > 0 && data.Rows.Count > 2)
                    {
                        Showgrid.DataSource = data;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        tr_printReport.Visible = true;
                        lblnorec.Visible = false;
                        lblNote.Visible = true;


                        int rct = Showgrid.Rows.Count - 2;
                        //Rowspan
                        GridViewRow row = Showgrid.Rows[0];
                        GridViewRow previousRow = Showgrid.Rows[1];
                        Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[0].Font.Bold = true;
                        Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[1].Font.Bold = true;
                        Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                        for (int i = 0; i < data.Columns.Count; i++)
                        {
                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }
                        }

                        //ColumnSpan

                        for (int cell = Showgrid.Rows[0].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = Showgrid.Rows[0].Cells[cell];
                            TableCell previouscol = Showgrid.Rows[0].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;

                                }
                            }
                        }

                    }

                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Record(s) found";
                    Showgrid.Visible = false;
                    lblNote.Visible = false;
                    tr_printReport.Visible = false;
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Select Column Order Settings";
                Showgrid.Visible = false;
                lblNote.Visible = false;
                tr_printReport.Visible = false;
            }

        }
        catch { }
    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            if (!arrListVisibleColumn.Contains("0"))
                e.Row.Cells[0].Visible = false;
            if (Session["Rollflag"].ToString() == "0" || !arrListVisibleColumn.Contains("1"))
                e.Row.Cells[1].Visible = false;
            if (Session["Regflag"].ToString() == "0" || !arrListVisibleColumn.Contains("2"))
                e.Row.Cells[2].Visible = false;
            if (Session["Studflag"].ToString() == "0" || !arrListVisibleColumn.Contains("3"))
                e.Row.Cells[3].Visible = false;
            if (Session["Appflag"].ToString() == "0" || !arrListVisibleColumn.Contains("4"))
                e.Row.Cells[4].Visible = false;
            if (Session["Admisionflag"].ToString() == "0" || !arrListVisibleColumn.Contains("5"))
                e.Row.Cells[5].Visible = false;


            for (int j = 7; j < data.Columns.Count; j++)
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;

        }

    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
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
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
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
            string name = "";
            cb.Checked = false;
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

    #endregion

    //protected void cbIsLab_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        loadTest();
    //        loadSubject();
    //    }
    //    catch { }
    //}

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        try
        {
            string syl_year = string.Empty;
            con2a.Close();
            con2a.Open();
            SqlCommand cmd2a;
            SqlDataReader get_syl_year;
            cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + degree_code + " and semester =" + sem + " and batch_year=" + batch_year + " ", con2a);
            get_syl_year = cmd2a.ExecuteReader();
            get_syl_year.Read();
            if (get_syl_year.HasRows == true)
            {
                if (get_syl_year[0].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = get_syl_year[0].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
            return syl_year;

        }
        catch
        {
            return string.Empty;
        }
    }
    protected float checkMark(float mark)
    {
        try
        {
            if (mark == -1)
                return 0;
            else if (mark == -20)
                return 0;
            else
                return mark;
        }
        catch
        {
            return 0;
        }
    }
    protected string checkStatus(string mark)
    {
        try
        {
            if (mark == "-1")
                return "AAA";
            else if (mark == "-20")
                return " ";
            else
                return mark;
        }
        catch
        {
            return null;
        }
    }

    #region colorder
    protected void lnkBtnColOrder_OnClick(object sender, EventArgs e)
    {
        loadColumnOrder();
        loadColumns();
        divColOrder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        divColOrder.Visible = true;
    }
    protected void loadColumnOrder()
    {
        try
        {
            cblColumnOrder.Items.Clear();
            cblColumnOrder.Items.Add(new ListItem("Roll No", "0"));
            cblColumnOrder.Items.Add(new ListItem("Reg No", "1"));
            cblColumnOrder.Items.Add(new ListItem("Student Type", "2"));
            cblColumnOrder.Items.Add(new ListItem("Admission No", "3"));
            cblColumnOrder.Items.Add(new ListItem("Application No", "4"));
            cblColumnOrder.Items.Add(new ListItem("Student Name", "5"));
            //if (cbIsLab.Checked)
            //{
            cblColumnOrder.Items.Add(new ListItem("Q1 Mark", "6"));
            cblColumnOrder.Items.Add(new ListItem("Q2 Mark", "7"));
            cblColumnOrder.Items.Add(new ListItem("Q3 Mark", "8"));
            cblColumnOrder.Items.Add(new ListItem("Q4 Mark", "9"));
            cblColumnOrder.Items.Add(new ListItem("Total (Best Of Two)", "10"));
            cblColumnOrder.Items.Add(new ListItem("Quiz(10 M)", "11"));
            cblColumnOrder.Items.Add(new ListItem("Assignment(5 M)", "12"));
            cblColumnOrder.Items.Add(new ListItem("Theory Grand Total(25 M)", "13"));
            cblColumnOrder.Items.Add(new ListItem("Record Mark", "14"));
            cblColumnOrder.Items.Add(new ListItem("Observation Mark", "15"));
            cblColumnOrder.Items.Add(new ListItem("Total", "16"));
            cblColumnOrder.Items.Add(new ListItem("Internal Mark", "17"));
            cblColumnOrder.Items.Add(new ListItem("Lab Grand Total", "18"));
            //}
            //else
            //{
            //    cblColumnOrder.Items.Add(new ListItem("Q1 Mark", "7"));
            //    cblColumnOrder.Items.Add(new ListItem("Q2 Mark", "8"));
            //    cblColumnOrder.Items.Add(new ListItem("Q3 Mark", "9"));
            //    cblColumnOrder.Items.Add(new ListItem("Q4 Mark", "10"));
            //    cblColumnOrder.Items.Add(new ListItem("Descriptive Total", "11"));
            //    cblColumnOrder.Items.Add(new ListItem("Quiz Mark", "12"));
            //    cblColumnOrder.Items.Add(new ListItem("Assignment Mark", "13"));
            //    cblColumnOrder.Items.Add(new ListItem("Grand Total", "14"));

            //  }
        }
        catch { }
    }
    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }
    public void loadColumns()
    {
        try
        {
            string linkname = "Internal Mark Report column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            userCode = Session["usercode"].ToString();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + userCode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "' ";
            dscol.Clear();
            dscol = da.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblColumnOrder.Items.Count > 0)
                {
                    arrListVisibleColumn.Clear();
                    for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                    {
                        if (cblColumnOrder.Items[i].Selected == true)
                        {
                            arrListVisibleColumn.Add(Convert.ToString(cblColumnOrder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblColumnOrder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblColumnOrder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    arrListVisibleColumn.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                arrListVisibleColumn.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }

            }
            else
            {
                arrListVisibleColumn.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    cblColumnOrder.Items[i].Selected = true;
                    arrListVisibleColumn.Add(Convert.ToString(cblColumnOrder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblColumnOrder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblColumnOrder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + userCode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + userCode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + userCode + "','" + Convert.ToString(Session["collegecode"]) + "')";
                clsupdate = da.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + userCode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = da.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblColumnOrder.Items.Count; k++)
                                {
                                    if (val == cblColumnOrder.Items[k].Value)
                                    {
                                        cblColumnOrder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblColumnOrder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void btnColOrderOK_OnClick(object sender, EventArgs e)
    {
        divColOrder.Visible = false;
        loadColumns();
    }
    #endregion

    protected void btnExcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;

            if (reportname.ToString().Trim() != "")
            {
                lblnorec.Visible = false;
                da.printexcelreportgrid(Showgrid, reportname);

            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
            txtxl.Text = "";
            reportname = "";
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnprintmasterr_Click(object sender, EventArgs e)
    {
        string filter = "";
        //Session["column_header_row_count"] = spreadReport.Sheets[0].ColumnHeader.RowCount;
        string batch = string.Empty;
        string deg = string.Empty;
        string brnch = string.Empty;
        string subname = string.Empty;
        string subno = string.Empty;
        string dep = da.GetFunction("select acronym from degree where degree_code='" + ddlBranch.SelectedValue.ToString() + "'");
        string staffname = Convert.ToString(Session["Staff_code"]);
        if (!string.IsNullOrEmpty(staffname))
        {
            staffname = da.GetFunction("select staff_name from staffmaster where staff_code='" + staffname + "'");
        }
        else
        {
            string batchYear = Convert.ToString(ddlBatch.SelectedValue);
            string sec = Convert.ToString(ddlSec.SelectedValue);
            string strSec = "";
            if (sec.Trim().ToLower() == "all" || sec.Trim() == "" || sec == "-1" || sec == null)
            {
                strSec = string.Empty;
            }
            else
            {
                if (Convert.ToString(Session["Staff_Code"]) != "")
                    strSec = " and ss.sections='" + sec + "'";
                else
                    strSec = " and sections='" + sec + "'";
            }
            for (int k = 0; k < cblSub.Items.Count; k++)
            {
                if (cblSub.Items[k].Selected == true)
                {
                    if (string.IsNullOrEmpty(subname))
                        subname = cblSub.Items[k].Text;
                    else
                        subname = subname + "," + cblSub.Items[k].Text;

                    if (string.IsNullOrEmpty(subno))
                        subno = "'" + cblSub.Items[k].Value + "'";
                    else
                        subno = subno + ",'" + cblSub.Items[k].Value + "'";

                }
            }
            string q = "select ss.staff_code,staff_name from staff_selector ss,staffmaster st where subject_no in(" + subno + ") " + strSec + " and ss.staff_code=st.staff_code ";
            ds = da.select_method_wo_parameter(q, "Text");
            string staffs = string.Empty;
            for (int k = 0; k < ds.Tables.Count; k++)
            {
                staffs = Convert.ToString(ds.Tables[0].Rows[k]["staff_name"]);
                if (staffname == "")
                    staffname = staffs;
                else

                    staffname += "," + staffs;
            }
            //staffname = da.GetFunction("select staff_code from staff_selector where subject_no in(" + subno + ") " + strSec + " and batch_year='" + batchYear + "'");
            // staffname = da.GetFunction("select staff_name from staffmaster where staff_code='" + staffname + "'");

        }
        subname = string.Empty;
        for (int k = 0; k < cblSub.Items.Count; k++)
        {
            if (cblSub.Items[k].Selected == true)
            {
                if (string.IsNullOrEmpty(subname))
                    subname = cblSub.Items[k].Text;
                else
                    subname = subname + "," + cblSub.Items[k].Text;

                if (string.IsNullOrEmpty(subno))
                    subno = "'" + cblSub.Items[k].Value + "'";
                else
                    subno = subno + ",'" + cblSub.Items[k].Value + "'";

            }
        }


        string sem = ddlSemYr.SelectedItem.Text.ToString();
        string sem1 = string.Empty;
        string sem2 = string.Empty;
        if (sem == "1")
        {
            sem1 = "I";
            sem2 = "I";
        }
        else if (sem == "2")
        {
            sem1 = "II";
            sem2 = "I";
        }
        else if (sem == "3")
        {
            sem1 = "III";
            sem2 = "II";
        }
        else if (sem == "4")
        {
            sem1 = "IV";
            sem2 = "II";
        }
        else if (sem == "5")
        {
            sem1 = "V";
            sem2 = "III";
        }
        else if (sem == "6")
        {
            sem1 = "VI";
            sem2 = "III";
        }
        else if (sem == "7")
        {
            sem1 = "VII";
            sem2 = "IV";
        }
        else if (sem == "8")
        {
            sem1 = "VIII";
            sem2 = "IV";
        }
        string tst = ddlTest.SelectedItem.Text.ToString();
        string[] tst1 = null;
        if (tst.Contains('-'))
            tst1 = tst.Split('-');
        string tstname = string.Empty;
        if (tst1.Length > 0)
            tstname = tst1[1].ToString();
        string subcode = string.Empty;
        string subcode1 = "select distinct subject_code from subject where subject_no in (" + subno + ")";
        DataSet dstd = new DataSet();
        dstd = da.select_method_wo_parameter(subcode1, "text");
        if (dstd.Tables.Count > 0 && dstd.Tables[0].Rows.Count > 0)
        {
            for (int l = 0; l < dstd.Tables[0].Rows.Count; l++)
            {
                if (string.IsNullOrEmpty(subcode))
                    subcode = dstd.Tables[0].Rows[l]["subject_code"].ToString();
                else
                    subcode = subcode + "," + dstd.Tables[0].Rows[l]["subject_code"].ToString();
            }
        }


        string degreedetails = string.Empty;
        degreedetails = "" + ddlDegree.SelectedItem.Text + " " + ddlTest.SelectedItem.Text + " @Name of the Faculty : " + staffname + "@MID : " + tstname + "  @Subject : " + subname + "@Department : " + dep + " - " + ddlSec.SelectedItem.Text.ToString() + "@Subject Code : " + subcode + " @Year/Semester : " + sem2 + " - " + sem1 + "";

        //degreedetails = "Internal Mark Report @" + "Batch: " + ddlBatch.SelectedItem.Text + "@Degree :" + ddlDegree.SelectedItem.Text + "@Branch :" + ddlBranch.SelectedItem.Text + "@Semester :" + ddlSemYr.SelectedItem.Text + "@Section :" + ddlSec.SelectedItem.Text + filter;

        string pagename = "InternalMarkReport.aspx";
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;

        lblnorec.Visible = false;
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Internal Mark Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}