using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Collections.Generic;
using System.Configuration;
public partial class Certificate_setting_master : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
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

            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            setLabelText();
            if (!IsPostBack)
            {
                bindCollege();
                bindbatch();
                bindedu();
                binddegree();
                bindbranch();
                bindsem();
                bindheader();
                bindledger();
                bindfinanceyear();
                bindcertificate();
            }
        }
        catch (Exception ex) { }
    }
    #region Multiple check box and dropdownlist event
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        bindedu();
        binddegree();
        bindbranch();
        bindsem();
        bindheader();
        bindledger();
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
    public void cb_Header_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_Header, cb_Header, txt_Header, "Header");
        bindledger();
    }
    public void cbl_Header_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_Header, cb_Header, txt_Header, "Header");
        bindledger();
    }
    public void cb_Ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_Ledger, cb_Ledger, txt_Ledger, "Ledger");
    }
    public void cbl_Ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_Ledger, cb_Ledger, txt_Ledger, "Ledger");
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
    void bindheader()
    {
        try
        {
            txt_Header.Text = "--Select--";
            cb_Header.Checked = false;
            cbl_Header.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + Session["usercode"].ToString() + " AND H.CollegeCode = " + Convert.ToString(ddlcollege.SelectedItem.Value) + "   ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Header.DataSource = ds;
                cbl_Header.DataTextField = "HeaderName";
                cbl_Header.DataValueField = "HeaderPK";
                cbl_Header.DataBind();
                for (int i = 0; i < cbl_Header.Items.Count; i++)
                {
                    cbl_Header.Items[i].Selected = true;
                }
                txt_Header.Text = "Header(" + cbl_Header.Items.Count + ")";
                cb_Header.Checked = true;
            }
        }
        catch (Exception ex) { }
    }
    void bindledger()
    {
        try
        {
            txt_Ledger.Text = "--Select--";
            cb_Ledger.Checked = false;
            string headerFK = rs.GetSelectedItemsValueAsString(cbl_Header);
            cbl_Ledger.Items.Clear();
            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + Session["usercode"].ToString() + " AND L.CollegeCode = " + Convert.ToString(ddlcollege.SelectedItem.Value) + " and L.HeaderFK in ('" + headerFK + "')";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Ledger.DataSource = ds;
                cbl_Ledger.DataTextField = "LedgerName";
                cbl_Ledger.DataValueField = "LedgerPK";
                cbl_Ledger.DataBind();
                for (int i = 0; i < cbl_Ledger.Items.Count; i++)
                {
                    cbl_Ledger.Items[i].Selected = true;
                }
                txt_Ledger.Text = "Ledger(" + cbl_Ledger.Items.Count + ")";
                cb_Ledger.Checked = true;
            }
        }
        catch (Exception ex) { }
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
    #endregion
    #region Button events
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        string group = Convert.ToString(txt_addgroup.Text);
        group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
        if (txt_addgroup.Text != "")
        {
            string college = ddlcollege.SelectedItem.Value;
            string grouporuserval = string.Empty;
            string grouporusercol = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporuserval = Convert.ToString(Session["group_code"]).Trim();
                grouporusercol = " group_code";

            }
            else if (Session["usercode"] != null)
            {
                grouporuserval = Convert.ToString(Session["usercode"]).Trim();
                grouporusercol = " usercode";
            }
            //string sql = "if exists ( select * from CertificateNameDetail where CertificateName='" + group + "' and Collegecode='" + college + "' and " + grouporusercol + "='" + grouporuserval + "') Update CertificateNameDetail set CertificateName='" + group + "' where CertificateName='" + group + "' and CollegeCode='" + college + "' and " + grouporusercol + "='" + grouporuserval + "' else insert into CertificateNameDetail (CertificateName,CollegeCode," + grouporusercol + ") values('" + group + "','" + college + "','" + grouporuserval + "')";
            string sql = "if exists ( select * from CertificateNameDetail where CertificateName='" + group + "' and Collegecode='" + college + "') Update CertificateNameDetail set CertificateName='" + group + "' where CertificateName='" + group + "' and CollegeCode='" + college + "'  else insert into CertificateNameDetail (CertificateName,CollegeCode) values('" + group + "','" + college + "')";
            int insert = d2.update_method_wo_parameter(sql, "Text");
            if (insert != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully";
                txt_addgroup.Text = "";
                plusdiv.Visible = false;
                panel_addgroup.Visible = false;
            }
            bindcertificate();
            txt_addgroup.Text = "";
        }
        else
        {
            plusdiv.Visible = true;
            lblerror.Visible = true;
            lblerror.Text = "Enter the CertificateName";
        }
    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }
    protected void btnplus1_Click(object sender, EventArgs e)
    {
        lbl_addgroup.Text = "Certificate Name";
        txt_addgroup.Attributes.Add("maxlength", "150");
        txt_addgroup.Attributes.Add("placeholder", "Enter the Certificate Name");
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lblerror.Visible = false;
    }
    protected void btnminus1_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbl_certificate.Items.Count > 0)
            {
                string certificateid = rs.GetSelectedItemsValueAsString(cbl_certificate);
                if (certificateid.Trim() != "")
                {
                    query = "delete  from CertificateNameDetail where Certificate_ID in('" + certificateid + "') and Collegecode='" + ddlcollege.SelectedValue + "';";
                    query += "delete from Master_Settings where settings='CertificateFormatSetting' and value like '"+certificateid+"$%'";
                    int delete = d2.update_method_wo_parameter(query, "Text");
                    if (delete != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Deleted Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Selected";
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }
        }
        catch
        {
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_error.Visible = false;
            string headerfk = rs.GetSelectedItemsValueAsString(cbl_Header);
            string leaderfk = rs.GetSelectedItemsValueAsString(cbl_Ledger);
            if (headerfk.Trim() != "" && leaderfk.Trim() != "")
            {
                query = "select H.HeaderName,h.HeaderPK,l.LedgerName,l.LedgerPK from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK=h.HeaderPK and l.CollegeCode=h.CollegeCode and l.LedgerPK in('" + leaderfk + "') and l.HeaderFK in('" + headerfk + "') order by h.HeaderName,l.LedgerName";
                ds.Clear();
                ds = da.selectDataSet(query);
                if (ds.Tables.Count > 0 && ds.Tables != null)
                {
                    DataTable dt = new DataTable();
                    DataRow dr;
                    dt.Columns.Add("headername");
                    dt.Columns.Add("leadername");
                    dt.Columns.Add("headerfk");
                    dt.Columns.Add("ledgerfk");
                    dt.Columns.Add("amount");
                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(dr1["HeaderName"]);
                        dr[1] = Convert.ToString(dr1["LedgerName"]);
                        dr[2] = Convert.ToString(dr1["HeaderPK"]);
                        dr[3] = Convert.ToString(dr1["LedgerPK"]);
                        dr[4] = "";
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        headerledgeramtGrid.DataSource = dt;
                        headerledgeramtGrid.DataBind();
                        headerledgeramtGrid.Visible = true;
                        btn_viewfees.Visible = true;
                    }
                    else
                    {
                        headerledgeramtGrid.Visible = false;
                        btn_viewfees.Visible = false;
                    }
                }
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Header and Ledger";
                headerledgeramtGrid.Visible = false;
                btn_viewfees.Visible = false;
                btn_div.Visible = false;
                certificatedetails_grid.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            headerledgeramtGrid.Visible = false;
        }
    }
    protected void btn_viewfees_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbl_certificate.Items.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("certificatename");
                dt.Columns.Add("degree");
                dt.Columns.Add("headername");
                dt.Columns.Add("headerfk");
                dt.Columns.Add("ledgername");
                dt.Columns.Add("ledgerfk");
                dt.Columns.Add("amount");
                dt.Columns.Add("degreevalue");
                dt.Columns.Add("certificatenameid");
                string HeaderFK = rs.GetSelectedItemsValueAsString(cbl_Header);
                string ledgerFK = rs.GetSelectedItemsValueAsString(cbl_Ledger);
                string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                string certificateId = rs.GetSelectedItemsValueAsString(cbl_certificate);
                string educationlevel = rs.GetSelectedItemsValueAsString(cbl_edu);
                string courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                if (!string.IsNullOrEmpty(HeaderFK) && !string.IsNullOrEmpty(ledgerFK) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(certificateId) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(degreecode) && ddl_finyear.Items.Count > 0 && ddl_feecatagory.Items.Count > 0 && ddlcollege.Items.Count > 0)
                {
                    string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    string FinyearFk = Convert.ToString(ddl_finyear.SelectedItem.Value);
                    //string semester = Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[0] + " ";
                    //string feecatagory = getFeecategory(collegecode, usercode, semester, ref linkName);
                    string feecatagory = Convert.ToString(ddl_feecatagory.SelectedItem.Value);
                    query = "select certificate_amount,m.certificate_id,m.batchyear,m.edulevel,m.courseid,m.degreecode,m.financialyear, m.collegecode,m.Feecategory,d.headerfk,d.ledgerfk from Certificate_settingDet d,Certificate_settingmaster m where m.certificatepk=d.certificatefk and m.certificate_id in('" + certificateId + "') and m.batchyear in('" + batchyear + "') and m.edulevel in('" + educationlevel + "') and m.courseid in('" + courseid + "') and m.degreecode in('" + degreecode + "') and m.financialyear='" + FinyearFk + "' and m.collegecode='" + collegecode + "' and m.Feecategory='" + feecatagory + "' and d.headerfk in('" + HeaderFK + "') and d.ledgerfk in('" + ledgerFK + "')";
                    ds.Clear();
                    ds.Dispose();
                    ds = da.selectDataSet(query);
                    for (int c = 0; c < cbl_certificate.Items.Count; c++)
                    {
                        if (cbl_certificate.Items[c].Selected)
                        {
                            for (int b = 0; b < cbl_batch.Items.Count; b++)
                            {
                                if (cbl_batch.Items[b].Selected)
                                {
                                    for (int ed = 0; ed < cbl_edu.Items.Count; ed++)
                                    {
                                        if (cbl_edu.Items[ed].Selected)
                                        {
                                            for (int d = 0; d < cbl_degree.Items.Count; d++)
                                            {
                                                if (cbl_degree.Items[d].Selected)
                                                {
                                                    for (int br = 0; br < cbl_branch.Items.Count; br++)
                                                    {
                                                        if (cbl_branch.Items[br].Selected)
                                                        {
                                                            foreach (GridViewRow gridrow in headerledgeramtGrid.Rows)
                                                            {
                                                                string Headername = Convert.ToString((gridrow.FindControl("lbl_headername") as Label).Text);
                                                                string ledgername = Convert.ToString((gridrow.FindControl("lbl_ledgername") as Label).Text);
                                                                HeaderFK = Convert.ToString((gridrow.FindControl("lbl_headerfk") as Label).Text);
                                                                ledgerFK = Convert.ToString((gridrow.FindControl("lbl_ledgerfk") as Label).Text);
                                                                string amount = Convert.ToString((gridrow.FindControl("txt_amount") as TextBox).Text);
                                                                dr = dt.NewRow();
                                                                string degree = Convert.ToString(cbl_batch.Items[b].Text) + " - " + Convert.ToString(cbl_edu.Items[ed].Text) + " - " + Convert.ToString(cbl_degree.Items[d].Text) + " - " + Convert.ToString(cbl_branch.Items[br].Text).Split('-')[1];
                                                                string degreevalue = Convert.ToString(cbl_batch.Items[b].Value) + "-" + Convert.ToString(cbl_edu.Items[ed].Value) + "-" + Convert.ToString(cbl_degree.Items[d].Value) + "-" + Convert.ToString(cbl_branch.Items[br].Value);
                                                                certificateId = Convert.ToString(cbl_certificate.Items[c].Value);
                                                                batchyear = Convert.ToString(cbl_batch.Items[b].Text);
                                                                educationlevel = Convert.ToString(cbl_edu.Items[ed].Value);
                                                                courseid = Convert.ToString(cbl_degree.Items[d].Value);
                                                                degreecode = Convert.ToString(cbl_branch.Items[br].Value);
                                                                if (ds.Tables[0].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[0].DefaultView.RowFilter = " certificate_id='" + certificateId + "' and batchyear='" + batchyear + "' and edulevel='" + educationlevel + "'and courseid='" + courseid + "'  and degreecode='" + degreecode + "' and financialyear='" + FinyearFk + "' and collegecode='" + collegecode + "' and Feecategory='" + feecatagory + "' and headerfk='" + HeaderFK + "' and ledgerfk='" + ledgerFK + "'";
                                                                    DataView feeamount_view = ds.Tables[0].DefaultView;
                                                                    //if (string.IsNullOrEmpty(amount))
                                                                    //{
                                                                    if (feeamount_view.Count > 0)
                                                                    {
                                                                        amount = Convert.ToString(feeamount_view[0]["certificate_amount"]);
                                                                    }
                                                                    //}
                                                                }
                                                                dr[0] = Convert.ToString(cbl_certificate.Items[c].Text);
                                                                dr[1] = degree;
                                                                dr[2] = Convert.ToString(Headername);
                                                                dr[3] = Convert.ToString(HeaderFK);
                                                                dr[4] = Convert.ToString(ledgername);
                                                                dr[5] = Convert.ToString(ledgerFK);
                                                                dr[6] = amount;
                                                                dr[7] = degreevalue;
                                                                dr[8] = Convert.ToString(cbl_certificate.Items[c].Value);
                                                                dt.Rows.Add(dr);
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
                    if (dt.Rows.Count > 0)
                    {
                        certificatedetails_grid.DataSource = dt;
                        certificatedetails_grid.DataBind();
                        certificatedetails_grid.Visible = true;
                        btn_div.Visible = true;
                    }
                    else
                    {
                        certificatedetails_grid.Visible = false;
                        btn_div.Visible = false;
                    }
                }
                else
                {
                    btn_viewfees.Visible = false;
                    btn_div.Visible = false;
                    certificatedetails_grid.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Fields";
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            headerledgeramtGrid.Visible = false;
        }
    }
    protected void btn_onlinefees_Click(object sender, EventArgs e)
    {
        OnlineClickflag = true;
        CertificateSave();
    }
    protected void btn_offlinefees_Click(object sender, EventArgs e)
    {
        OnlineClickflag = false;
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
            string librarydue = "0";
            string feedue = "0";
            bool insert_check = false;
            int feeallotins = 0;
            if (ddlcollege.Items.Count > 0 && ddl_feecatagory.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                semester = Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[0];
                if (Convert.ToString(ViewState["linkName"]).ToUpper() == "TERM")
                {
                    semester = "1";//Convert.ToString(ddl_feecatagory.SelectedItem.Text).Split(' ')[1] + " ";
                }
                batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                FinyearFk = Convert.ToString(ddl_finyear.SelectedItem.Value);
                feecatagory = Convert.ToString(ddl_feecatagory.SelectedItem.Value);//getFeecategory(collegecode, usercode, semester, ref linkName);
                if (cb_librarydue.Checked)
                    librarydue = "1";
                if (cb_Feesdue.Checked)
                    feedue = "1";
                if (!string.IsNullOrEmpty(batchyear.Trim()) && !string.IsNullOrEmpty(degreecode.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(feecatagory) && !string.IsNullOrEmpty(FinyearFk))
                {
                    ds.Clear();
                    ds = da.selectDataSet("select App_No,degree_code,Batch_Year from Registration r where r.CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and  degree_code in('" + degreecode + "') and Batch_Year in('" + batchyear + "') and r.current_semester='" + semester + "' and r.college_code='" + collegecode + "'");
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables != null)
                    {
                        foreach (GridViewRow gr in certificatedetails_grid.Rows)
                        {
                            string certificateId = Convert.ToString((gr.FindControl("lbl_certificatenameId") as Label).Text);
                            string HeaderFK = Convert.ToString((gr.FindControl("lbl_headerfk") as Label).Text);
                            string ledgerFK = Convert.ToString((gr.FindControl("lbl_ledgerfk") as Label).Text);
                            string Amount = Convert.ToString((gr.FindControl("txt_amount") as TextBox).Text);
                            string[] degreedet = Convert.ToString((gr.FindControl("lbl_degreevalue") as Label).Text).Split('-');
                            string duplicateIdentity = Convert.ToString((gr.FindControl("lbl_degreevalue") as Label).Text);

                            if (degreedet.Length == 4)
                            {
                                batchyear = Convert.ToString(degreedet[0]);
                                educationlevel = Convert.ToString(degreedet[1]);
                                courseid = Convert.ToString(degreedet[2]);
                                degreecode = Convert.ToString(degreedet[3]);
                            }
                            if (!string.IsNullOrEmpty(batchyear.Trim()) && !string.IsNullOrEmpty(degreecode.Trim()) && !string.IsNullOrEmpty(courseid.Trim()) && !string.IsNullOrEmpty(HeaderFK) && !string.IsNullOrEmpty(ledgerFK))
                            {
                                #region certificate Insert
                                string certificatequery = "if exists(select*from Certificate_settingmaster where certificate_id='" + certificateId + "' and batchyear='" + batchyear + "' and edulevel='" + educationlevel + "' and courseid='" + courseid + "' and degreecode='" + degreecode + "' and financialyear='" + FinyearFk + "' and collegecode='" + collegecode + "' and Feecategory='" + feecatagory + "')update  Certificate_settingmaster set isfeedue='" + feedue + "',IsLibarydue='" + librarydue + "' where certificate_id='" + certificateId + "' and batchyear='" + batchyear + "' and edulevel='" + educationlevel + "' and courseid='" + courseid + "' and degreecode='" + degreecode + "' and financialyear='" + FinyearFk + "' and collegecode='" + collegecode + "' and Feecategory='" + feecatagory + "' else insert into Certificate_settingmaster (Certificate_ID,Batchyear,EduLevel,CourseID,Degreecode,FinancialYear,Feecategory,IsFeedue, IsLibarydue, Collegecode)values('" + certificateId + "','" + batchyear + "','" + educationlevel + "','" + courseid + "','" + degreecode + "','" + FinyearFk + "','" + feecatagory + "','" + feedue + "','" + librarydue + "','" + collegecode + "')";
                                int setMasterins = da.insertData(certificatequery);
                                string certificate_fk = d2.GetFunction("select CertificatePK from Certificate_settingmaster where certificate_id='" + certificateId + "' and batchyear='" + batchyear + "' and edulevel='" + educationlevel + "' and courseid='" + courseid + "' and degreecode='" + degreecode + "' and financialyear='" + FinyearFk + "' and collegecode='" + collegecode + "' and feecategory='" + feecatagory + "' order by CertificatePK desc");
                                if (certificate_fk.Trim() != "0")
                                {
                                    string certificatedetquery = "if exists (select*from Certificate_settingDet where certificatefk='" + certificate_fk + "' and headerfk='" + HeaderFK + "' and ledgerfk='" + ledgerFK + "')update Certificate_settingDet set certificate_amount='" + Amount + "' where certificatefk='" + certificate_fk + "' and headerfk='" + HeaderFK + "' and ledgerfk='" + ledgerFK + "' else insert into Certificate_settingDet (certificatefk,headerfk, ledgerfk, certificate_amount)values('" + certificate_fk + "','" + HeaderFK + "','" + ledgerFK + "','" + Amount + "')";
                                    int setMasterdetins = da.insertData(certificatedetquery);
                                }
                                #endregion
                                if (!OnlineClickflag)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "'";
                                    DataView student_view = ds.Tables[0].DefaultView;
                                    foreach (DataRowView dr in student_view)
                                    {
                                        #region Feeallot Insert Process
                                        string App_no = Convert.ToString(dr["App_No"]);
                                        if (!string.IsNullOrEmpty(Amount.Trim()) && Amount.Trim() != "0" && certificate_fk.Trim() != "0")
                                        {
                                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + HeaderFK + "') and FeeCategory in('" + feecatagory + "')  and App_No in('" + App_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + Amount + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + Amount + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + Amount + "' where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + HeaderFK + "') and FeeCategory in('" + feecatagory + "') and App_No in('" + App_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + App_no + ",'" + ledgerFK + "','" + HeaderFK + "','" + Amount + "','0','0','0','" + Amount + "','0','0','','0','" + feecatagory + "','','0','','0','0','" + Amount + "','" + FinyearFk + "')";
                                            feeallotins = da.insertData(insupdquery);
                                        }
                                        #endregion
                                        if (feeallotins != 0)
                                            insert_check = true;
                                    }
                                }
                                else
                                {
                                    if (setMasterins != 0)
                                        insert_check = true;
                                }
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
                        btn_viewfees.Visible = false;
                        btn_div.Visible = false;
                        certificatedetails_grid.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Founds";
                    }
                }
                else
                {
                    btn_viewfees.Visible = false;
                    btn_div.Visible = false;
                    certificatedetails_grid.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Fields";
                }
            }
            else
            {
                btn_viewfees.Visible = false;
                btn_div.Visible = false;
                certificatedetails_grid.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            headerledgeramtGrid.Visible = false;
        }
    }
    #endregion
    protected void certificate_databound(object sender, EventArgs e)
    {
        try
        {
            for (int i = certificatedetails_grid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = certificatedetails_grid.Rows[i];
                GridViewRow previousRow = certificatedetails_grid.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lbl_certificatename");
                    Label lnlname1 = (Label)previousRow.FindControl("lbl_certificatename");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
            for (int i = certificatedetails_grid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = certificatedetails_grid.Rows[i];
                GridViewRow previousRow = certificatedetails_grid.Rows[i - 1];
                for (int j = 2; j <= 2; j++)
                {
                    Label lnlname = (Label)row.FindControl("lbl_degree");
                    Label lnlname1 = (Label)previousRow.FindControl("lbl_degree");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
            for (int i = certificatedetails_grid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = certificatedetails_grid.Rows[i];
                GridViewRow previousRow = certificatedetails_grid.Rows[i - 1];
                for (int j = 3; j <= 3; j++)
                {
                    Label lnlname = (Label)row.FindControl("lbl_headername");
                    Label lnlname1 = (Label)previousRow.FindControl("lbl_headername");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    public string getFeecategory(string collegecode, string usercode, string currentsem, ref string linkName)
    {
        string feecatagory = string.Empty;
        try
        {
            string linkValue = string.Empty;
            string SelectQ = string.Empty;
            linkValue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
            if (!string.IsNullOrEmpty(linkValue) && linkValue != "0")
            {
                feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and (textval like '%" + currentsem + "Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");
                linkName = "SemesterandYear";
            }
            else
            {
                linkValue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                if (!string.IsNullOrEmpty(linkValue) && linkValue == "0")
                {
                    feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '%" + currentsem + "Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");
                    linkName = "Semester";
                }
                else if (!string.IsNullOrEmpty(linkValue) && linkValue == "1")
                {
                    string csem = returnYearforSem(currentsem);
                    feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '%" + csem + " Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");
                    linkName = "Year";
                }
                else if (!string.IsNullOrEmpty(linkValue) && linkValue == "2")
                {
                    feecatagory = d2.GetFunction("select distinct TextCode from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term" + currentsem + "%' and textval not like '-1%' and t.college_code ='" + collegecode + "' ");
                    //if (!string.IsNullOrEmpty(featDegreeCode))
                    //    SelectQ += "  and f.degree_code in('" + featDegreeCode + "') ";
                    //SelectQ += " order by len(textval),textval asc";
                    linkName = "Term";
                }
            }
        }
        catch { feecatagory = ""; }
        return feecatagory;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
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
        lbl.Add(lbl_sem);
        //fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}