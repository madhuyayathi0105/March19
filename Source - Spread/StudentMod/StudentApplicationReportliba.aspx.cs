using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using System.Configuration;

public partial class StudentApplicationReportliba : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string Collinfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    Collinfo = collegecode;
                }

                loadstrm();
                bindbatch();
                binddeg();
                binddept();
                txt_fromdate.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                txt_todate.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                Collinfo = collegecode;
            }
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                Collinfo = collegecode;
            }

            loadstrm();
            bindbatch();
            binddeg();
            binddept();
        }
        catch
        {
        }
    }
    #endregion

    #region grauation

    public void loadstrm()
    {
        try
        {
            ddlgrad.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgrad.DataSource = ds;
                ddlgrad.DataTextField = "type";
                ddlgrad.DataValueField = "type";
                ddlgrad.DataBind();
                ddlgrad.Enabled = true;
            }
            else
            {
                ddlgrad.Enabled = false;
            }
            // binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string stream = ddlgrad.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + clgvalue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }

            txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
            // binddept();
        }
        catch { }
    }
    #endregion

    #region batch
    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region degree

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            int i = 0;
            string stream = "";
            if (ddlgrad.Items.Count > 0)
            {
                if (ddlgrad.SelectedItem.Text != "")
                {
                    stream = ddlgrad.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
            binddept();
        }
        catch { }
    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_branch.Items.Clear();
            cb_branch.Checked = false;
            txt_branch.Text = "---Select---";
            int i = 0;
            string batch2 = "";
            if (ddl_batch.Items.Count > 0)
            {
                batch2 = Convert.ToString(ddl_batch.SelectedItem.Value);
            }

            string degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            string collegecode = ddlcollege.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = lbl_branch.Text + "(" + cbl_branch.Items.Count + ")";
                        cb_branch.Checked = true;
                    }
                }
            }

        }
        catch { }
    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
        }
        catch { }
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");

        }
        catch { }
    }
    #endregion

    #region auto search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select stud_name from applyn where isnull( isconfirm,'0') ='1' and ISNULL(admission_status,'0')=0 and stud_name like '" + prefixText + "%' and College_code='" + Collinfo + "'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getmob(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select Student_Mobile from applyn where isnull( isconfirm,'0') ='1' and ISNULL(admission_status,'0')=0 and Student_Mobile like '" + prefixText + "%' and College_code='" + Collinfo + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select app_formno from applyn where isnull( isconfirm,'0') ='1' and ISNULL(admission_status,'0')=0 and app_formno like '" + prefixText + "%' and College_code='" + Collinfo + "'";
        name = ws.Getname(query);
        return name;
    }
    #endregion



    #region button search

    protected DataSet loaddataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get value

            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            string grad = Convert.ToString(ddlgrad.SelectedItem.Value);
            string batch = Convert.ToString(ddl_batch.SelectedItem.Value);
            string deptcode = Convert.ToString(getCblSelectedValue(cbl_branch));

            string studname = Convert.ToString(txt_searchstudname.Text);
            string applNo = Convert.ToString(txt_searchappno.Text);
            string studmblNo = Convert.ToString(txt_searchmobno.Text);
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            #endregion

            string query = "";
            if (!string.IsNullOrEmpty(studname))
            {
                //student name
                #region student name
                query = "select a.remarks,a.sex,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, seattype,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name, convert(varchar(10),a.date_applied,103) as date_applied,a.stuper_id,convert(varchar(10),a.ApplBankRefDate,103) as date_paid,a.applbankrefnumber  from degree d,Department dt,Course C ,applyn a Where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isnull( isconfirm,'0') ='1'  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and a.stud_name like '%" + txt_searchstudname.Text + "%'   ";
                if (ddlreportTye.SelectedIndex == 1)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='1'";
                }
                else if (ddlreportTye.SelectedIndex == 2)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1'";
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(studname))
            {
                //studnet application no
                #region student applno
                query = "select a.remarks,a.sex,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, seattype,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name, convert(varchar(10),a.date_applied,103) as date_applied,a.stuper_id,convert(varchar(10),a.ApplBankRefDate,103) as date_paid,a.applbankrefnumber  from degree d,Department dt,Course C ,applyn a Where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isnull( isconfirm,'0') ='1'  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and a.app_formno='" + txt_searchappno.Text + "'   ";
                if (ddlreportTye.SelectedIndex == 1)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='1'";
                }
                else if (ddlreportTye.SelectedIndex == 2)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1'";
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(studname))
            {
                //student mbl no
                #region studentmbl no
                query = "select a.remarks,a.sex,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, seattype,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name, convert(varchar(10),a.date_applied,103) as date_applied,a.stuper_id,convert(varchar(10),a.ApplBankRefDate,103) as date_paid,a.applbankrefnumber  from degree d,Department dt,Course C ,applyn a Where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isnull( isconfirm,'0') ='1'  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and a.Student_Mobile='" + txt_searchmobno.Text + "' ";
                if (ddlreportTye.SelectedIndex == 1)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='1'";
                }
                else if (ddlreportTye.SelectedIndex == 2)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1'";
                }
                #endregion
            }
            else
            {
                //applied
                #region applied

                string paidcondition = "";
                if (rdb_paid.Checked == true)
                    paidcondition = " and isnull( isconfirm,'0') ='1'";
                if (rdb_notpaid.Checked == true)
                    paidcondition = " and isnull( isconfirm,'0')='0'";

                query = "select a.remarks,a.sex,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, seattype,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name, convert(varchar(10),a.date_applied,103) as date_applied,a.stuper_id,convert(varchar(10),a.ApplBankRefDate,103) as date_paid,a.applbankrefnumber  from degree d,Department dt,Course C ,applyn a Where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + paidcondition + " and a.degree_code in('" + deptcode + "')and a.Batch_Year in('" + batch + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "'  and a.college_code=d.college_code";

                if (ddlreportTye.SelectedIndex == 3)
                {
                    query = "";
                    query = " select a.remarks,a.sex,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, seattype,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name, convert(varchar(10),a.date_applied,103) as date_applied,a.stuper_id,convert(varchar(10),a.ApplBankRefDate,103) as date_paid,a.applbankrefnumber  from degree d,Department dt,Course C ,applyn a,registration r Where r.app_no=a.app_no and  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isnull( isconfirm,'0') ='1' and r.degree_code in('" + deptcode + "')and r.Batch_Year in('" + batch + "')  and r.college_code='" + ddlcollege.SelectedItem.Value + "'and r.college_code=d.college_code ";

                }
                if (ddlreportTye.SelectedIndex == 0)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='0'";
                    if (cbl_datewise.Checked == true)//if (fromdate != "" && todate != "")                    
                        query = query + " and  A.date_applied between '" + fromdate + "' and '" + todate + "'";
                }
                else if (ddlreportTye.SelectedIndex == 1)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='1'";
                    if (cbl_datewise.Checked == true)//if (fromdate != "" && todate != "")                    
                        query = query + " and  A.AdmitedDate between '" + fromdate + "' and '" + todate + "'";
                }
                else if (ddlreportTye.SelectedIndex == 2)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1' and a.app_no not in( select app_no from Registration a where  a.degree_code in('" + deptcode + "')and a.Batch_Year in('" + batch + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "')";
                    if (cbl_datewise.Checked == true)//if (fromdate != "" && todate != "")                    
                        query = query + " and  A.AdmitedDate between '" + fromdate + "' and '" + todate + "'";

                }
                else if (ddlreportTye.SelectedIndex == 3)
                {
                    query = query + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1' ";
                    if (cbl_datewise.Checked == true)
                        query = query + " and  A.AdmitedDate between '" + fromdate + "' and '" + todate + "'";
                }
                #endregion
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(query, "text");
        }
        catch { }
        return dsload;
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loaddataset();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadDetails();
            }
            else
            {
                lblalert.Text = "No Record Found!";
                lblalert.Visible = true;
                imgAlert.Visible = true;
                btnExport.Visible = false;
                gridDetail.Visible = false;
            }
        }
        catch { }
    }

    protected void loadDetails()
    {
        try
        {
            double amount = 0;
            double fnlamt = 0;
            string gender = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("Date Applied");
            dt.Columns.Add("Application No");
            dt.Columns.Add("Applicant Name");
            dt.Columns.Add("Gender");
            dt.Columns.Add("Batch");
            dt.Columns.Add(lbl_degree.Text);
            dt.Columns.Add(lbl_branch.Text + " (Option1)");
            //dt.Columns.Add(lbl_branch.Text+" (Option2)");
            dt.Columns.Add("Mobile No");
            //  dt.Columns.Add("Religion");
            // dt.Columns.Add("Community");
            // dt.Columns.Add("Remarks");
            // dt.Columns.Add("Quota");
            dt.Columns.Add("Email ID");
            dt.Columns.Add("Date of Payment");
            dt.Columns.Add("Transaction Number");
            if (rdb_paid.Checked)
                dt.Columns.Add("Amount");

            DataRow dr;
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                dr = dt.NewRow();
                dr["Sno"] = Convert.ToString(sel + 1);
                dr["Date Applied"] = Convert.ToString(ds.Tables[0].Rows[sel]["date_applied"]);
                dr["Application No"] = Convert.ToString(ds.Tables[0].Rows[sel]["app_formno"]);
                dr["Applicant Name"] = Convert.ToString(ds.Tables[0].Rows[sel]["stud_name"]);
                string gen = Convert.ToString(ds.Tables[0].Rows[sel]["sex"]);
                if (gen == "0")
                    gender = "Male";
                else if (gen == "1")
                    gender = "Female";
                else
                    gender = "Transgender";
                dr["Gender"] = gender;
                dr["Batch"] = Convert.ToString(ds.Tables[0].Rows[sel]["Batch_Year"]);
                dr[lbl_degree.Text] = Convert.ToString(ds.Tables[0].Rows[sel]["Course_Name"]);
                dr[lbl_branch.Text + " (Option1)"] = Convert.ToString(ds.Tables[0].Rows[sel]["Dept_Name"]);
                // dr[lbl_branch.Text+" (Option2)"] = Convert.ToString(ds.Tables[0].Rows[sel]["Alternativedegree_code"]);
                dr["Mobile No"] = Convert.ToString(ds.Tables[0].Rows[sel]["Student_Mobile"]);
                //  dr["Religion"] = Convert.ToString(ds.Tables[0].Rows[sel]["religion"]);
                // dr["Community"] = Convert.ToString(ds.Tables[0].Rows[sel]["community"]);
                // dr["Remarks"] = Convert.ToString(ds.Tables[0].Rows[sel]["remarks"]);
                // dr["Quota"] = Convert.ToString(ds.Tables[0].Rows[sel]["seattype"]);
                dr["Email ID"] = Convert.ToString(ds.Tables[0].Rows[sel]["stuper_id"]);
                dr["Date of Payment"] = Convert.ToString(ds.Tables[0].Rows[sel]["date_paid"]);
                dr["Transaction Number"] = Convert.ToString(ds.Tables[0].Rows[sel]["applbankrefnumber"]);
                if (rdb_paid.Checked)
                {
                    amount = 500;
                    dr["Amount"] = Convert.ToString(amount);
                    fnlamt += amount;
                }
                dt.Rows.Add(dr);
            }

            if (dt.Rows.Count > 0)
            {
                if (rdb_paid.Checked)
                {
                    dr = dt.NewRow();
                    dr["Amount"] = Convert.ToString(fnlamt);
                    dt.Rows.Add(dr);
                }
                gridDetail.DataSource = dt;
                gridDetail.DataBind();
                gridDetail.HeaderRow.Style.Add("background-color", "#0CA6CA");
                for (int i = 0; i < gridDetail.HeaderRow.Cells.Count; i++)
                {
                    gridDetail.HeaderRow.Cells[i].Style.Add("background-color", "#0CA6CA");
                    gridDetail.HeaderRow.Cells[i].Style.Add("font-weight", "Bold");
                    gridDetail.HeaderRow.Cells[i].Style.Add("font-size", "18px");
                    gridDetail.HeaderRow.Cells[i].Style.Add("color", "White");
                }

                btnExport.Visible = true;
                gridDetail.Visible = true;
            }
        }
        catch { }

    }

    //excel
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "StudentDetails.xls"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gridDetail.AllowPaging = false;
        //Change the Header Row back to white color
        gridDetail.HeaderRow.Style.Add("background-color", "#0CA6CA");
        //Applying stlye to gridview header cells
        for (int i = 0; i < gridDetail.HeaderRow.Cells.Count; i++)
        {
            gridDetail.HeaderRow.Cells[i].Style.Add("background-color", "#0CA6CA");
        }
        gridDetail.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }





    #endregion

    protected void ddlreportTye_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rdb_paid.Visible = false;
            rdb_notpaid.Visible = false;
            btnExport.Visible = false;
            gridDetail.Visible = false;
            if (ddlreportTye.SelectedIndex == 0)
            {
                gridDetail.Visible = false;
                rdb_paid.Visible = true;
                rdb_notpaid.Visible = true;
            }
            if (ddlreportTye.SelectedIndex == 1)
            {
                gridDetail.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void gridDetail_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            if (rdb_paid.Checked)
            {
                if (gridDetail.Rows.Count > 0)
                {
                    gridDetail.Rows[gridDetail.Rows.Count - 1].Cells[1].Text = "Total";
                    gridDetail.Rows[gridDetail.Rows.Count - 1].Cells[1].ColumnSpan = 5;
                    gridDetail.Rows[gridDetail.Rows.Count - 1].Style.Add("background-color", "Green");
                    for (int i = 2; i < 6; i++)
                    {
                        gridDetail.Rows[gridDetail.Rows.Count - 1].Cells.RemoveAt(i);
                    }
                }
            }
        }
        catch { }
    }
    protected void gridDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Assumes the Price column is at index 4
        int col = gridDetail.Columns.Count - 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            if (rdb_paid.Checked)
                e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    #region text change event

    public void txt_searchstudname_TextChanged(object sender, EventArgs e)
    {
        if (txt_searchstudname.Text != "")
        {
            txt_searchmobno.Text = "";
            txt_searchappno.Text = "";
            btn_go_OnClick(sender, e);
        }
    }

    public void txt_searchappno_TextChanged(object sender, EventArgs e)
    {
        if (txt_searchappno.Text != "")
        {
            txt_searchmobno.Text = "";
            txt_searchstudname.Text = "";
            btn_go_OnClick(sender, e);
        }
    }

    public void txt_searchmobno_TextChanged(object sender, EventArgs e)
    {
        if (txt_searchmobno.Text != "")
        {
            txt_searchappno.Text = "";
            txt_searchstudname.Text = "";
            btn_go_OnClick(sender, e);
        }
    }

    #endregion

    protected void cb_selectedchange_Click(object sender, EventArgs e)
    {
        if (cbl_datewise.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        else
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
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
        }
        catch { }
    }

    #endregion
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
        lbl.Add(lbl_clgname);
        fields.Add(0);
        lbl.Add(lbl_graduation);
        fields.Add(1);
        lbl.Add(lbl_degree);
        fields.Add(2);
        lbl.Add(lbl_branch);
        fields.Add(3);
        //lbl.Add(lbl_org_sem);
        //fields.Add(4);



        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}