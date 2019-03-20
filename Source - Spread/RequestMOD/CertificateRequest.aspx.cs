using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Configuration;

public partial class RequestMOD_CertificateRequest : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess da = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string query = string.Empty;
    string q1 = string.Empty;
    string linkName = string.Empty;
    bool OnlineClickflag = false;
    string staffcodesession = string.Empty;
    string LoginStaffApplid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["collegecode"] == null)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}
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
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
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

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            staffcodesession = Session["Staff_Code"].ToString();
            if (LoginStaffApplid == "")
                LoginStaffApplid = d2.GetFunction(" select appl_id from staffmaster sm,staff_appl_master ap where sm.appl_no=ap.appl_no and sm.staff_code='" + staffcodesession + "'");
            if (!IsPostBack)
            {
                setLabelText();
                bindCollege();
                bindbatch();
                bindedu();
                binddegree();
                bindbranch();
                bindsem();
                bindcertificate();
                bindfinanceyear();
                ddl_searchtype.Items.Add(new ListItem("Roll No", "0"));
                ddl_searchtype.Items.Add(new ListItem("Reg No", "1"));
                ddl_searchtype.Items.Add(new ListItem("Admission No", "2"));
                ddl_searchtype.Items.Add(new ListItem("App No", "3"));
                ddl_searchtype.Items.Add(new ListItem("Student Name", "4"));
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
            }
        }
        catch
        {
        }
    }
    #region Multiple check box and dropdownlist event
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        bindedu();
        binddegree();
        bindbranch();
        bindsem();
        bindfinanceyear();
    }
    protected void cb_certificate_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_certificate, cb_certificate, txt_certificate, "Certificate Name");
    }
    protected void cbl_certificate_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_certificate, cb_certificate, txt_certificate, "Certificate Name");
    }
    protected void cb_batch_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, lbl_batch.Text);
    }
    protected void cbl_batch_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_batch, cb_batch, txt_batch, lbl_batch.Text);
    }
    protected void cb_edu_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_edu, cb_edu, txt_edu, lbl_edulev.Text);
        binddegree();
        bindbranch();
        bindsem();
    }
    protected void cbl_edu_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_edu, cb_edu, txt_edu, lbl_edulev.Text);
        binddegree();
        bindbranch();
        bindsem();
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
        bindsem();
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
        bindsem();
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
        bindsem();
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
        bindsem();
    }
    #endregion
    #region BindMethods
    void bindCollege()
    {
        try
        {
            ds.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            Hashtable hat = new Hashtable();
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }
    void bindbatch()
    {
        cbl_batch.Items.Clear(); txt_batch.Text = "--Select--";
        q1 = " SELECT distinct batch_year FROM tbl_attendance_rights where user_id='" + usercode + "' ORDER BY batch_year DESC";
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_batch.DataSource = ds;
            cbl_batch.DataTextField = "batch_year";
            cbl_batch.DataValueField = "batch_year";
            cbl_batch.DataBind();
            cbl_batch.Items[0].Selected = true;
            txt_batch.Text = lbl_batch.Text + "(1)";
            cb_batch.Checked = true;
        }
    }
    void bindedu()
    {
        try
        {
            cbl_edu.Items.Clear();
            txt_edu.Text = "--Select--";
            if (ddlcollege.Items.Count > 0)
            {
                ds = d2.select_method_wo_parameter("select distinct Edu_Level from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' order by Edu_Level desc", "Text");
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    cbl_edu.DataSource = ds;
                    cbl_edu.DataTextField = "Edu_Level";
                    cbl_edu.DataValueField = "Edu_Level";
                    cbl_edu.DataBind();
                    //for (int i = 0; i < cbl_edu.Items.Count; i++)
                    //{
                    cbl_edu.Items[0].Selected = true;
                    txt_edu.Text = lbl_edulev.Text + "(1)";
                    cb_edu.Checked = true;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    void binddegree()
    {
        try
        {
            string query = "";
            cbl_degree.Items.Clear();
            txt_degree.Text = "--Select--";
            if (ddlcollege.Items.Count > 0)
            {
                string educationlevel = rs.GetSelectedItemsValueAsString(cbl_edu);
                if (educationlevel == "")
                {
                    query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' order by d.Course_Id";
                }
                else
                {
                    query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + educationlevel + "') order by d.Course_Id";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    //for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //{
                    cbl_degree.Items[0].Selected = true;
                    txt_degree.Text = lbl_degree.Text + "(1)";
                    cb_degree.Checked = true;
                    //}
                }
                else
                {
                    txt_degree.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            txt_branch.Text = lbl_branch.Text;
            string deg = rs.GetSelectedItemsValueAsString(cbl_degree);
            if (ddlcollege.Items.Count > 0)
            {
                if (deg != "--Select--" && deg != null && deg != "")
                {
                    ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,department.dept_name+'-'+degree.Acronym as Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + deg + "') and degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'  order by degree.degree_code", "Text");
                }
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "Acronym";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        cbl_branch.Items[i].Selected = true;
                        txt_branch.Text = lbl_branch.Text + "(" + (cbl_branch.Items.Count) + ")";
                        cb_branch.Checked = true;
                    }
                }
                else
                {
                    cbl_branch.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    void bindsem()
    {
        try
        {
            ddl_feecatagory.Items.Clear();
            ds.Clear();
            if (ddlcollege.Items.Count > 0)
            {
                string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                ds = d2.loadFeecategory(ddlcollege.SelectedItem.Value, usercode, ref linkName);
                ViewState["linkName"] = linkName;
                //string query = " select distinct  MAX( ndurations)as ndurations from ndegree n where n.Degree_code in('" + degreecode + "') and n.college_code in('" + ddlcollege.SelectedItem.Value + "') union select distinct  MAX(duration) as ndurations  from degree d where d.Degree_Code in('" + degreecode + "') and d.college_code in('" + ddlcollege.SelectedItem.Value + "')";
                //ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_feecatagory.Items.Clear();
                    //string sem = Convert.ToString(ds.Tables[0].Rows[0]["ndurations"]);
                    //for (int j = 1; j <= Convert.ToInt32(sem); j++)
                    //{
                    //    ddl_feecatagory.Items.Add(new System.Web.UI.WebControls.ListItem(j.ToString() + " Semester", j.ToString()));
                    //}
                    ddl_feecatagory.DataSource = ds;
                    ddl_feecatagory.DataTextField = "textval";
                    ddl_feecatagory.DataValueField = "TextCode";
                    ddl_feecatagory.DataBind();
                }
            }
        }
        catch { }
    }
    void bindfinanceyear()
    {
        try
        {
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + ddlcollege.SelectedItem.Value + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            ddl_finyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    ddl_finyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    void bindcertificate()
    {
        try
        {
            cbl_certificate.Items.Clear();
            txt_certificate.Text = "Certificate Name"; string text = string.Empty;
            ds.Clear();
            string sql = " select CertificateName,Certificate_ID from CertificateNameDetail where Collegecode='" + ddlcollege.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_certificate.DataSource = ds;
                cbl_certificate.DataTextField = "CertificateName";
                cbl_certificate.DataValueField = "Certificate_ID";
                cbl_certificate.DataBind();
            }
            for (int i = 0; i < cbl_certificate.Items.Count; i++)
            {
                cbl_certificate.Items[i].Selected = true;
                text = Convert.ToString(cbl_certificate.Items[i].Text);
            }
            if (cbl_certificate.Items.Count == 1)
                txt_certificate.Text = "" + text + "";
            else
                txt_certificate.Text = "Certificate Name(" + (cbl_certificate.Items.Count) + ")";
            cb_certificate.Checked = true;
        }
        catch { }
    }
    void loadsearchtype()
    {
        switch (Convert.ToUInt32(ddl_searchtype.SelectedItem.Value))
        {
            case 0:
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
                break;
            case 1:
                txt_searchappno.Attributes.Add("placeholder", "Reg No");
                break;
            case 2:
                txt_searchappno.Attributes.Add("placeholder", "Admission No");
                break;
            case 3:
                txt_searchappno.Attributes.Add("placeholder", "App No");
                break;
            case 4:
                txt_searchappno.Attributes.Add("placeholder", "Student Name");
                break;
        }
    }
    #endregion
    #region Button events
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void ddl_searchtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadsearchtype();
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected string flitertype()
    {
        string type = string.Empty;
        switch (Convert.ToUInt32(ddl_searchtype.SelectedValue))
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
            case 4:
                type = "r.stud_name";
                break;
        }
        return type;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections from applyn a,Registration r, degree d,Department dt,Course C where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' ";
            string type = flitertype();
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string Batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
            string sem = Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[0];
            if (Convert.ToString(ViewState["linkName"]).ToUpper() == "TERM")
            {
                sem = "1";//Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[1] + " ";
            }
            string CertificateId = rs.GetSelectedItemsValueAsString(cbl_certificate);
            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(Batchyear) && !string.IsNullOrEmpty(CertificateId))
            {
                if (txt_searchappno.Text.Trim() != "")
                    q1 += " and " + type + "='" + txt_searchappno.Text.Trim() + "'";
                else
                    q1 += " and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + Batchyear + "') and c.Course_Id in('" + deptcode + "') and  r.Current_Semester in('" + sem + "') and r.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rs.Fpreadheaderbindmethod("S.No-50/Select-50/Student Name-210/Roll No-150/Reg No-150/Admission No-150/Gender-100/Section-70/" + lbl_degree.Text + "-100/" + lbl_branch.Text + "-300/" + lbl_semT.Text + "-100", FpSpread1, "FALSE");

                    FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    cb.AutoPostBack = false;
                    cball.AutoPostBack = true;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cball;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cb;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["App_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[4].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[5].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["stud_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["roll_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Reg_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["Roll_Admit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["sex"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["sections"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["Course_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dr["Dept_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dr["Current_Semester"]);
                        #region Fpspread Alignment and style
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Locked = true;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        #endregion
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    lbl_error.Visible = false;
                    btn_request.Visible = true;
                }
                else
                {
                    btn_request.Visible = false;
                    FpSpread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Founds";
                }
            }
            else
            {
                btn_request.Visible = false;
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            FpSpread1.Visible = false;
            btn_request.Visible = false;
        }
    }
    protected void btn_Request_Click(object sender, EventArgs e)
    {
        CertificateSave();
    }
    protected void CertificateSave()
    {
        try
        {
            string degreecode = string.Empty;
            string batchyear = string.Empty;
            string courseid = string.Empty;
            string educationlevel = string.Empty;
            string semester = string.Empty;
            string FinyearFk = string.Empty;
            string feecatagory = string.Empty;
            string collegecode = string.Empty;
            string CertificateId = string.Empty;
            string edulevel = string.Empty;
            string value = string.Empty;
            bool insert_check = false;
            int feeallotins = 0; Requestcode();
            if (ddlcollege.Items.Count > 0 && ddl_feecatagory.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                semester = Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[0];
                if (Convert.ToString(ViewState["linkName"]).ToUpper() == "TERM")
                    semester = "1";//Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[1] + " ";
                batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                FinyearFk = Convert.ToString(ddl_finyear.SelectedItem.Value);
                feecatagory = Convert.ToString(ddl_feecatagory.SelectedItem.Value);
                CertificateId = rs.GetSelectedItemsValueAsString(cbl_certificate);
                courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                edulevel = rs.GetSelectedItemsValueAsString(cbl_edu);
                if (!string.IsNullOrEmpty(batchyear.Trim()) && !string.IsNullOrEmpty(degreecode.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(feecatagory) && !string.IsNullOrEmpty(FinyearFk))
                {
                    ds.Clear();
                    string certQ = " select App_No,r.degree_code,Batch_Year,c.Edu_Level,c.Course_Id from Registration r, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + degreecode + "') and Batch_Year in('" + batchyear + "') and r.current_semester='" + semester + "' and r.college_code='" + collegecode + "'";
                    certQ += " select certificate_amount,m.certificate_id,c.CertificateName,m.batchyear,m.edulevel,m.courseid, m.degreecode,m.financialyear, m.collegecode,m.Feecategory,d.headerfk,d.ledgerfk,h.HeaderName,l.LedgerName from Certificate_settingDet d,Certificate_settingmaster m,CertificateNameDetail c,FM_HeaderMaster h,FM_LedgerMaster l where h.CollegeCode=l.CollegeCode and m.Collegecode=l.CollegeCode and h.HeaderPK=l.HeaderFK and l.HeaderFK=d.HeaderFK and l.LedgerPK=d.LedgerFK  and c.Certificate_ID=m.Certificate_ID and m.certificatepk=d.certificatefk and m.certificate_id in('" + CertificateId + "') and m.batchyear in('" + batchyear + "') and m.edulevel in('" + edulevel + "') and m.courseid in('" + courseid + "') and m.degreecode in('" + degreecode + "') and m.financialyear='" + FinyearFk + "' and m.collegecode='" + collegecode + "' and m.Feecategory='" + feecatagory + "'";
                    ds = da.selectDataSet(certQ);
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                    {
                        FpSpread1.SaveChanges();
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            value = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (value == "1")
                            {
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].GetTag(i, 1));
                                DataView StudentDet = new DataView();
                                ds.Tables[0].DefaultView.RowFilter = " App_No='" + AppNo + "'";
                                StudentDet = ds.Tables[0].DefaultView;
                                foreach (DataRowView dr in StudentDet)
                                {
                                    degreecode = Convert.ToString(dr["degree_code"]);
                                    batchyear = Convert.ToString(dr["Batch_Year"]);
                                    edulevel = Convert.ToString(dr["Edu_Level"]);
                                    courseid = Convert.ToString(dr["Course_Id"]);
                                }
                                DataView StudentCertFeeDet = new DataView();
                                string insupdQ = string.Empty;
                                for (int C = 0; C < cbl_certificate.Items.Count; C++)
                                {
                                    if (cbl_certificate.Items[C].Selected)
                                    {
                                        CertificateId = Convert.ToString(cbl_certificate.Items[C].Value);
                                        ds.Tables[1].DefaultView.RowFilter = " certificate_id='" + CertificateId + "' and batchyear='" + batchyear + "' and edulevel='" + edulevel + "' and courseid='" + courseid + "' and  degreecode in('" + degreecode + "') and financialyear='" + FinyearFk + "' and  collegecode='" + collegecode + "' and  Feecategory='" + feecatagory + "'";
                                        StudentCertFeeDet = ds.Tables[1].DefaultView;

                                        #region Request Insert Process
                                        string requestcode = Convert.ToString(ViewState["requestcode"]).Trim();
                                        if (!string.IsNullOrEmpty(requestcode))
                                        {
                                            insupdQ = " if not exists( select ReqAppNo from RQ_Requisition where RequestType='11' and ReqAppNo='" + AppNo + "' and MemType='1' and RequestDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and CertReqType='2')insert into RQ_Requisition (RequestType,RequestCode,RequestDate,MemType,ReqApproveStage,ReqAppStatus,ReqAppNo,college_code, CertReqType, ReqStaffAppNo)values('11','" + requestcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','1','1','1','" + AppNo + "','" + collegecode + "','2','" + LoginStaffApplid + "')";
                                            insupdQ += " update registration set tcapply='1' where app_no='" + AppNo + "'";
                                            int insertQ = da.insertData(insupdQ);
                                            string RequisitionFK = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequestType='11' and ReqAppNo='" + AppNo + "' and MemType='1' and CertReqType='2'");
                                            insupdQ = "if not exists(select RequisitionFK from RQ_RequisitionDet where RequisitionFK='" + RequisitionFK + "' and ReqCertificateID='" + CertificateId + "') insert into RQ_RequisitionDet(RequisitionFK,ReqCertificateID)values('" + RequisitionFK + "','" + CertificateId + "')";
                                            da.insertData(insupdQ);
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Please Set Code Settings";
                                            return;
                                        }
                                        #endregion

                                        #region Feeallot Insert Process
                                        foreach (DataRowView dr in StudentCertFeeDet)
                                        {
                                            string certificateId = Convert.ToString(dr["certificate_id"]);
                                            string HeaderFK = Convert.ToString(dr["headerfk"]);
                                            string ledgerFK = Convert.ToString(dr["ledgerfk"]);
                                            string Amount = Convert.ToString(dr["certificate_amount"]);
                                            if (!string.IsNullOrEmpty(Amount.Trim()) && Amount.Trim() != "0" && !string.IsNullOrEmpty(certificateId.Trim()))
                                            {
                                                insupdQ = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + HeaderFK + "') and FeeCategory in('" + feecatagory + "')  and App_No in('" + AppNo + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + Amount + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + Amount + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + Amount + "' where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + HeaderFK + "') and FeeCategory in('" + feecatagory + "') and App_No in('" + AppNo + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + AppNo + ",'" + ledgerFK + "','" + HeaderFK + "','" + Amount + "','0','0','0','" + Amount + "','0','0','','0','" + feecatagory + "','','0','','0','0','" + Amount + "','" + FinyearFk + "')";
                                                feeallotins = da.insertData(insupdQ);
                                            }
                                            if (feeallotins != 0)
                                                insert_check = true;
                                        }
                                        #endregion
                                    }
                                }
                                Requestcode();
                            }
                        }
                        if (insert_check)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                        }
                    }
                    else
                    {
                        btn_request.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Founds";
                    }
                }
                else
                {
                    btn_request.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Fields";
                }
            }
            else
            {
                btn_request.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }

    #endregion
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //"Roll No", "0"
        //"Reg No", "1"
        //"Admission No", "2"
        //"App No", "3"
        int SEARCHTYPE = 0;
        int.TryParse(contextKey, out SEARCHTYPE);
        string type = "";
        switch (SEARCHTYPE)
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
            case 4:
                type = "r.stud_name";
                break;
        }
        string query = " select " + type + "  from applyn a,Registration r where a.app_no=r.App_No  and " + type + " like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    public void Requestcode()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select ReqAcr,ReqSize,ReqStNo  from IM_CodeSettings order by StartDate desc";
            DataSet reqcodeDs = d2.select_method_wo_parameter(selectquery, "Text");
            if (reqcodeDs.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(reqcodeDs.Tables[0].Rows[0]["ReqAcr"]);
                string itemstarno = Convert.ToString(reqcodeDs.Tables[0].Rows[0]["ReqStNo"]);
                string itemsize = Convert.ToString(reqcodeDs.Tables[0].Rows[0]["ReqSize"]);
                selectquery = "select distinct top (1)  RequestCode,RequisitionPK   from RQ_Requisition where RequestCode like '" + Convert.ToString(itemacronym) + "%' order by RequisitionPK desc";
                reqcodeDs.Clear();
                reqcodeDs = d2.select_method_wo_parameter(selectquery, "Text");
                if (reqcodeDs.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(reqcodeDs.Tables[0].Rows[0]["RequestCode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    len1 = Convert.ToInt32(itemsize) - len;
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
                    }
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + newnumber;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + newnumber;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(newnumber);
                    }
                    if (newitemcode.Trim() != "")
                    {
                        newitemcode = itemacr + "" + newitemcode;
                    }
                }
                else
                {
                    string itemacr = Convert.ToString(itemstarno);
                    int len = itemacr.Length;
                    string items = Convert.ToString(itemsize);
                    int len1 = Convert.ToInt32(items);
                    int size = len1 - len;
                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + itemstarno;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                }
                ViewState["requestcode"] = Convert.ToString(newitemcode);
            }
        }
        catch
        { }
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        //lbl.Add(lbl_clgT);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_semT);
        //fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}