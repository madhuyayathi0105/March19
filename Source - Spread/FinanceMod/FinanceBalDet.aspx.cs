using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class FinanceBalDet : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    Boolean boolStudClick = false;
    static byte roll = 0;
    static int GrdRowCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Session["collegecode"] == null)
        //    Response.Redirect("~/Default.aspx");
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }

            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                edu_level();
                bindbatch();
                degree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                string headerName = string.Empty;
                bindheader(headerName);
                loadpaid();
                //  loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                //txt_fromdate.Attributes.Add("disabled", "disabled");
                //txt_todate.Attributes.Add("disabled", "disabled");
                //txt_fromdate.Enabled = false;
                //txt_todate.Enabled = false;
                getPrintSettings();
                LoadIncludeSetting();
                loadfinanceyear();
                // getFinancialYear();
                loadType();
                rblMode_Selected(sender, e);
                getAcademicYear();

                loadseat();
                loadquota();
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            popstud.Attributes.Add("Style", "display:none;");
            if (checkSchoolSetting() == 0)
            {
                txtbatch.Enabled = true;
                txt_degree.Enabled = false;
                txt_branch.Enabled = false;
                txt_sem.Enabled = false;
            }
        }
        catch
        {
        }
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
        lbl.Add(lblclg);
        // lbl.Add(lbl_str1);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_org_sem);
        fields.Add(0);
        //fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        edu_level();
        bindbatch();
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        string headerName = string.Empty;
        bindheader(headerName);
        loadpaid();
        rblMode_Selected(sender, e);
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        edu_level();
        bindbatch();
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        string headerName = string.Empty;
        bindheader(headerName);
        loadpaid();
        rblMode_Selected(sender, e);
        loadseat();
        loadquota();//abarna
    }
    #endregion

    #region degree,dept,sem
    public void bindbatch()
    {
        try
        {
            cblbatch.Items.Clear();
            cbbatch.Checked = false;
            txtbatch.Text = "--Select--";
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblbatch.DataSource = ds;
                cblbatch.DataTextField = "batch_year";
                cblbatch.DataValueField = "batch_year";
                cblbatch.DataBind();
                for (int i = 0; i < cblbatch.Items.Count; i++)
                {
                    cblbatch.Items[i].Selected = true;
                }
                cbbatch.Checked = true;
                txtbatch.Text = lblbatch.Text + "(" + cblbatch.Items.Count + ")";
                degree();
            }
            CallCheckboxChange(cbbatch, cblbatch, txtbatch, lblbatch.Text, lblbatch.Text);
        }
        catch
        {
        }
    }
    public void degree()
    {
        try
        {
            string query, edulvl = "";
            cbl_degree.Items.Clear();

            edulvl = reuse.GetSelectedItemsText(cbl_grad);


            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            string selColleges = reuse.GetSelectedItemsValueAsString(cblclg);

            query = "select distinct c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code in ('" + selColleges + "') and Edu_Level in ('" + edulvl + "')  " + rights + "";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_name";
                cbl_degree.DataBind();
                cb_degree.Checked = true;

            }
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, lbl_degree.Text);
        }
        catch (Exception ex)
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, lbl_degree.Text);
        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string collegeSel = reuse.GetSelectedItemsValueAsString(cblclg);
            string branch = reuse.GetSelectedItemsText(cbl_degree);

            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cb_branch.Checked = false;
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct department.dept_name  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and course.course_name in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code in ('" + collegeSel + "') " + rights + " ";
            }

            ds.Clear();

            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "dept_name";
                cbl_branch.DataBind();
                cb_branch.Checked = true;
            }
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, lbl_branch.Text);
        }
        catch
        {
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, lbl_branch.Text);
        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();

        string build = "";
        string build1 = "";
        string batch = "";
        string collegeSel = reuse.GetSelectedItemsValueAsString(cblclg);
        string branch = reuse.GetSelectedItemsText(cbl_branch);

        if (cblbatch.Items.Count > 0)
        {
            batch = reuse.GetSelectedItemsValueAsString(cblbatch);
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree d,department dt where dt.dept_code=d.dept_code and dept_name in ('" + branch + "') and d.college_code in ('" + collegeSel + "')";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = "Sem(" + cbl_sem.Items.Count + ")";
                }
            }
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = "";

            int i = 0;
            string collegeSel = reuse.GetSelectedItemsValueAsString(cblclg);
            string branch = reuse.GetSelectedItemsText(cbl_branch);

            if (cblbatch.Items.Count > 0)
            {
                batch = reuse.GetSelectedItemsValueAsString(cblbatch);
            }
            string sqlquery = "select distinct sections from registration r,degree d, department dt where r.degree_code=d.degree_code and dt.dept_code=d.dept_code and  batch_year in('" + batch + "') and dept_name in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sec.DataSource = ds;
                cbl_sec.DataTextField = "sections";
                cbl_sec.DataValueField = "sections";
                cbl_sec.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }
            }
            else
            {
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void edu_level()
    {
        string st = "";
        string collegeSel = reuse.GetSelectedItemsValueAsString(cblclg);

        cbl_grad.Items.Clear();
        if (collegeSel != string.Empty)
        {
            st = "select distinct edu_level,priority from course where college_code in ('" + collegeSel + "') order by priority";

            ds = d2.select_method_wo_parameter(st, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_grad.DataSource = ds;
                cbl_grad.DataTextField = "edu_level";
                cbl_grad.DataValueField = "edu_level";
                cbl_grad.DataBind();
                cb_grad.Checked = true;

            }
        }
        CallCheckboxChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, lbl_graduation.Text);
    }
    //18-05-2017
    protected void cbbatch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cbbatch, cblbatch, txtbatch, lblbatch.Text, lbl_degree.Text);

        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cblbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbbatch, cblbatch, txtbatch, lblbatch.Text, "");

        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lblbatch.Text, lblbatch.Text);

        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "");

        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cb_grad_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, lbl_graduation.Text);

        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cbl_grad_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, "");
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }

    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                //txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region header
    public void bindheader(string headerName)
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            //if (!string.IsNullOrEmpty(headerName))
            //    query += " and headername in('" + headerName + "')";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderName";
                chkl_studhed.DataBind();
                int cnt = 0;
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    if (!string.IsNullOrEmpty(headerName))
                    {
                        if (headerName.Contains(chkl_studhed.Items[i].Text))
                        {
                            chkl_studhed.Items[i].Selected = true;
                            cnt++;
                        }
                        else
                            chkl_studhed.Items[i].Selected = false;
                    }
                    else
                    {
                        chkl_studhed.Items[i].Selected = true;
                        cnt++;
                    }
                }
                txt_studhed.Text = lblheader.Text + "(" + cnt + ")";
                if (chkl_studhed.Items.Count == cnt)
                    chk_studhed.Checked = true;
                string ledgerName = string.Empty;
                bindledger(ledgerName);
            }
        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        string ledgerName = string.Empty;
        switch (rblMode.SelectedIndex)
        {
            case 0:
                bindledger(ledgerName);
                break;
            case 1:
                loadHostelLedger(ledgerName);
                break;
            case 2:
                loadTransLedger(ledgerName);
                break;
        }
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        string ledgerName = string.Empty;
        switch (rblMode.SelectedIndex)
        {
            case 0:
                bindledger(ledgerName);
                break;
            case 1:
                loadHostelLedger(ledgerName);
                break;
            case 2:
                loadTransLedger(ledgerName);
                break;
        }
    }
    #endregion

    #region Ledger
    public void bindledger(string ledgerName)
    {
        try
        {
            string headercode;

            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            headercode = Convert.ToString(getCblSelectedValue(chkl_studhed));
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            if (Convert.ToString(collegecode) != "" && Convert.ToString(headercode) != "")
            {
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P where l.HeaderFK =h.HeaderPK and  L.LedgerPK = P.LedgerFK and  l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
                //if (!string.IsNullOrEmpty(ledgerName))
                //    query += " and l.ledgername in('" + ledgerName + "')";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ds;
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataValueField = "ledgername";
                    chkl_studled.DataBind();
                    int cnt = 0;
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ledgerName))
                        {
                            if (ledgerName.Contains(chkl_studled.Items[i].Text))
                            {
                                chkl_studled.Items[i].Selected = true;
                                cnt++;
                            }
                            else
                                chkl_studled.Items[i].Selected = false;
                        }
                        else
                        {
                            chkl_studled.Items[i].Selected = true;
                            cnt++;
                        }
                    }
                    txt_studled.Text = lbl_ledger.Text + "(" + cnt + ")";
                    if (cnt == chkl_studled.Items.Count)
                        chk_studled.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }

    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }
    # endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_paid.Items.Clear();
            chk_paid.Checked = false;
            txt_paid.Text = "--Select--";
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
                chk_paid.Checked = true;
            }
        }
        catch
        {

        }

    }
    public void chk_paid_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    #endregion

    //#region financial year
    //public void loadfinanceyear()
    //{
    //    try
    //    {
    //        string fnalyr = "";
    //        // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
    //        string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by edate desc";
    //        ds.Dispose();
    //        ds.Reset();
    //        chkfyear.Checked = false;
    //        chklsfyear.Items.Clear();
    //        ds = d2.select_method_wo_parameter(getfinanceyear, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
    //                // string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
    //                ddlfinyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
    //            }

    //            //for (int i = 0; i < chklsfyear.Items.Count; i++)
    //            //{
    //            //    chklsfyear.Items[i].Selected = true;
    //            //    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
    //            //}
    //            //if (chklsfyear.Items.Count == 1)
    //            //{
    //            //    txtfyear.Text = "" + fnalyr + "";
    //            //}
    //            //else
    //            //{
    //            //    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
    //            //}
    //            //// txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
    //            //chkfyear.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            //string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    // string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                    txtfyear.Text = "" + fnalyr + "";
                else
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
                //tdlblfnl.Visible = true;
                tdfnl.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region financial year

    //protected Dictionary<string, string> loadfinanceyear()
    //{
    //    try
    //    {
    //        string fnalyr = "";
    //        //string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
    //        string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')";
    //        ds.Dispose();
    //        ds.Reset();
    //        chkfyear.Checked = false;
    //        chklsfyear.Items.Clear();
    //        ds = d2.select_method_wo_parameter(getfinanceyear, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
    //              //  string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
    //                chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));

    //            }
    //            for (int i = 0; i < chklsfyear.Items.Count; i++)
    //            {
    //                chklsfyear.Items[i].Selected = true;
    //                fnalyr = Convert.ToString(chklsfyear.Items[i].Value);
    //            }
    //            if (chklsfyear.Items.Count == 1)
    //                txtfyear.Text = "" + fnalyr + "";
    //            else
    //                txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
    //            // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
    //            chkfyear.Checked = true;
    //            //tdlblfnl.Visible = true;
    //            tdfnl.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    protected Dictionary<string, string> getFinancialYear()
    {
        Dictionary<string, string> htfinlYR = new Dictionary<string, string>();
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string selQFK = string.Empty;
            selQFK = "  select distinct (convert(nvarchar(15),FinYearStart,103)+'-'+convert(nvarchar(15),FinYearEnd,103)+'-'+convert(varchar(10),collegecode)) as finyear,finyearpk as pk from FM_FinYearMaster where CollegeCode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!htfinlYR.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        htfinlYR.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]));
                }
            }
        }
        catch { htfinlYR.Clear(); }
        return htfinlYR;
    }

    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }

    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }

    #endregion

    protected string getLedgerFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct ledgerpk from fm_ledgermaster where collegecode in('" + collegecode + "') and ledgername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["ledgerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            UserbasedRights();
            string hdText = string.Empty;
            string ldText = string.Empty;
            string payMode = string.Empty;
            string batch = string.Empty;
            string degreeCode = string.Empty;
            string sem = string.Empty;
            string sec = string.Empty;
            string finyear = string.Empty;
            string studMode = string.Empty;
            string seattype = string.Empty;
            string quota = string.Empty;
            string strInclude = string.Empty;
            seattype = Convert.ToString(getCblSelectedText(cbl_seat));
            quota = Convert.ToString(getCblSelectedText(cblQuota));
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            string SelQuery = "";
            if (!string.IsNullOrEmpty(hdText))
            {
                SelQuery = "select distinct headerpk as code from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdText + "')";
                hdText = getFilterValues(SelQuery);
            }
            if (!string.IsNullOrEmpty(ldText))
            {
                ldText = getLedgerFK(ldText, collegecode);
            }
            if (!string.IsNullOrEmpty(seattype))//abar
            {
                SelQuery = "select distinct textcode as code from TextValTable  where TextCriteria='seat' and textval in('" + seattype + "') and college_code in('" + collegecode + "')";
                seattype = getFilterValues(SelQuery);
            }
            if (!string.IsNullOrEmpty(quota))//abar
            {
                if (!string.IsNullOrEmpty(seattype))
                {
                    SelQuery = "select distinct quotaid as code from stu_quotaseetinges where settype in('" + seattype + "') and  collegecode in('" + collegecode + "') and quotaname in('" + quota + "')";
                }
                else
                {
                    SelQuery = "select distinct quotaid as code from stu_quotaseetinges where  collegecode in('" + collegecode + "') and quotaname in('" + quota + "')";
                }
                quota = getFilterValues(SelQuery);
            }
            payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            batch = Convert.ToString(getCblSelectedText(cblbatch));
            degreeCode = Convert.ToString(getCblSelectedValue(cbl_branch));
            string courseName = Convert.ToString(getCblSelectedText(cbl_degree));
            if (!string.IsNullOrEmpty(degreeCode))
            {
                SelQuery = "     select distinct d.degree_code as code from degree d,department dt,course c ,deptprivilages dp where c.course_id=d.course_id and dt.dept_code=d.dept_code and c.college_code = d.college_code and dt.college_code = d.college_code and dp.Degree_code=d.Degree_code and d.college_code in('" + collegecode + "') and c.course_name in('" + courseName + "') and dt.dept_name in('" + degreeCode + "') " + rights + "";
                // SelQuery = " select distinct d.degree_code as code from degree d,course c,department dt,deptprivilages dp where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=dp.degree_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "') " + rights + " ";
                degreeCode = getFilterValues(SelQuery);
            }
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sec));
            //seattype = Convert.ToString(getCblSelectedText(cbl_seat));
            //quota = Convert.ToString(getCblSelectedText(cblQuota));
            //finyear = Convert.ToString(ddlfinyear.SelectedItem.Value);
            //if (!string.IsNullOrEmpty(finyear))
            //{
            //    string frDate = finyear.Split('-')[0];
            //    string toDate = finyear.Split('-')[1];
            //    SelQuery = " select distinct finyearpk as code from FM_FinYearMaster where CollegeCode in('" + collegecode + "') and FinYearStart='" + frDate.Split('/')[1] + "/" + frDate.Split('/')[0] + "/" + frDate.Split('/')[2] + "' and FinYearEnd='" + toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2] + "'  ";
            //    finyear = getFilterValues(SelQuery);
            //}
            //------------------------
            //financial year
            StringBuilder sbFinlYr = new StringBuilder();
            Dictionary<string, string> htFinlYR = getFinancialYear();
            if (chklsfyear.Items.Count > 0)
            {
                for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
                {
                    if (!chklsfyear.Items[fnl].Selected)
                        continue;
                    for (int clg = 0; clg < cblclg.Items.Count; clg++)
                    {
                        if (!cblclg.Items[clg].Selected)
                            continue;
                        string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + cblclg.Items[clg].Value);//to pass value get key from dictionary 
                        sbFinlYr.Append(KeyVal + "','");
                    }

                }
                if (sbFinlYr.Length > 0)
                    sbFinlYr.Remove(sbFinlYr.Length - 3, 3);

            }




            finyear = Convert.ToString(getCblSelectedValue(chklsfyear));
            //--------------------------
            studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            //  string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";


            strInclude = getStudCategory();

            //string strInclude = getStudCategory();

            string strCondition = string.Empty;//added by sudhagar 31.07.2017
            string SelQ = string.Empty;
            if (rblMode.SelectedIndex == 0)//general
            {
                strCondition = string.Empty;
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                {

                    #region Query
                    // string hdFK = getHeaderFK(hdText, collegecode);
                    if (string.IsNullOrEmpty(seattype))
                    {
                        if (string.IsNullOrEmpty(quota))
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                            SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                            if (cbdate.Checked)
                                SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");

                        }
                        else
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and  a.quota in('" + quota + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and  a.quota in('" + quota + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                            SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r,applyn a where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and  a.quota in('" + quota + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                            if (cbdate.Checked)
                                SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");

                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(quota))
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "')  and a.seattype in('" + seattype + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and a.seattype in('" + seattype + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                            SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r,applyn a where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "')  and a.seattype in('" + seattype + "')  and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                            if (cbdate.Checked)
                                SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");


                    #endregion
                        }
                        else
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "')  and a.seattype in('" + seattype + "')  and  a.quota in('" + quota + "') and a.app_no=r.app_no and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and a.seattype in('" + seattype + "') and a.app_no=r.app_no and  a.quota in('" + quota + "')  and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                            SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r,applyn a where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "')  and a.seattype in('" + seattype + "') and a.app_no=r.app_no  and  a.quota in('" + quota + "')  and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                            if (cbdate.Checked)
                                SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        }
                    }
                }
            }
            else if (rblMode.SelectedIndex == 1)//hostel(added by abarna 26.01.2018)
            {
                if (rblhostelmode.SelectedIndex == 0)//cumulative(added by abarna 26.01.2018)
                {
                    strCondition = " and stud_type in('Hostler')";
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                    {

                        #region Query
                        // string hdFK = getHeaderFK(hdText, collegecode);

                        SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                        SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                        SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                        if (cbdate.Checked)
                            SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        #endregion
                    }
                }
                if (rblhostelmode.SelectedIndex == 1 || rblhostelmode.SelectedIndex == 2)//hostelname wise and hostel with collegewise (added by abarna 26.01.2018)
                {
                    strCondition = " and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and ISNULL(IsSuspend,0)=0 ";
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                    {

                        #region Query
                        // string hdFK = getHeaderFK(hdText, collegecode);


                        SelQ = "select COUNT(distinct h.app_no)HostelCount,r.College_Code as CollegeCode,HostelMasterFK,hm.HostelName,c.Coll_acronymn from HT_HostelRegistration h,HM_HostelMaster hm,registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and ISNULL(IsSuspend,0)=0 and h.HostelMasterFK=hm.HostelMasterPK and r.app_no=h.APP_No";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by h.HostelMasterFK,r.College_Code,c.Coll_acronymn,hm.HostelName order by h.HostelMasterFK";

                        SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,h.hostelmasterfk from ft_feeallot f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no   and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "  " + strInclude + " group by r.college_code,h.hostelmasterfk order by r.college_code";

                        SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,h.hostelmasterfk from ft_findailytransaction f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                        if (cbdate.Checked)
                            SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,h.hostelmasterfk order by r.college_code";


                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        #endregion
                    }

                }

            }
            else//transport
            {
                strCondition = " and isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>''";
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                {

                    #region Query
                    // string hdFK = getHeaderFK(hdText, collegecode);

                    SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn  from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "";

                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                    SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                    SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                    if (cbdate.Checked)
                        SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";


                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(SelQ, "Text");
                    #endregion
                }
            }
            if (cbloadMaster.Checked)
            {
                string linkName = string.Empty;
                switch (rblMode.SelectedIndex)
                {
                    case 0:
                        linkName = "43LoadMasterGeneral";
                        break;
                    case 1:
                        linkName = "43LoadMasterHostel";
                        break;
                    case 2:
                        linkName = "43LoadMasterTransport";
                        break;
                }
                string hdTexts = Convert.ToString(getCblSelectedTempText(chkl_studhed));
                string ldTexts = Convert.ToString(getCblSelectedTempText(chkl_studled));
                bool boolSel = true;
                string linkValue = loadMaster(linkName, hdTexts, ldTexts, boolSel);
            }
            #endregion


        }
        catch { }
        return dsload;
    }

    protected DataSet loadDetailsCurSem()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            UserbasedRights();
            string hdText = string.Empty;
            string ldText = string.Empty;
            string payMode = string.Empty;
            string batch = string.Empty;
            string degreeCode = string.Empty;
            string sem = string.Empty;
            string sec = string.Empty;
            string finyear = string.Empty;
            string studMode = string.Empty;
            string seattype = string.Empty;
            string quota = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            string hostel = Convert.ToString(getCblSelectedText(cbl_hostelname));
            seattype = Convert.ToString(getCblSelectedText(cbl_seat));
            quota = Convert.ToString(getCblSelectedText(cblQuota));
            string SelQuery = "";
            if (!string.IsNullOrEmpty(hdText))
            {
                SelQuery = "select distinct headerpk as code from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdText + "')";
                hdText = getFilterValues(SelQuery);
            }
            if (!string.IsNullOrEmpty(ldText))
            {
                ldText = getLedgerFK(ldText, collegecode);
            }
            if (!string.IsNullOrEmpty(seattype))//abar
            {
                SelQuery = "select distinct textcode as code from TextValTable  where TextCriteria='seat' and textval in('" + seattype + "') and college_code in('" + collegecode + "')";
                seattype = getFilterValues(SelQuery);
            }
            if (!string.IsNullOrEmpty(quota))//abar
            {
                if (!string.IsNullOrEmpty(seattype))
                {
                    SelQuery = "select distinct quotaid as code from stu_quotaseetinges where settype in('" + seattype + "') and  collegecode in('" + collegecode + "') and quotaname in('" + quota + "')";
                }
                else
                {
                    SelQuery = "select distinct quotaid as code from stu_quotaseetinges where  collegecode in('" + collegecode + "') and quotaname in('" + quota + "')";
                }
                quota = getFilterValues(SelQuery);
            }
            payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            batch = Convert.ToString(getCblSelectedText(cblbatch));
            degreeCode = Convert.ToString(getCblSelectedValue(cbl_branch));
            string courseName = Convert.ToString(getCblSelectedText(cbl_degree));
            if (!string.IsNullOrEmpty(degreeCode))
            {
                SelQuery = "     select distinct d.degree_code as code from degree d,department dt,course c ,deptprivilages dp where c.course_id=d.course_id and dt.dept_code=d.dept_code and c.college_code = d.college_code and dt.college_code = d.college_code and dp.Degree_code=d.Degree_code and d.college_code in('" + collegecode + "') and c.course_name in('" + courseName + "') and dt.dept_name in('" + degreeCode + "') " + rights + "";
                //SelQuery = " select distinct d.degree_code as code from degree d,course c,department dt,deptprivilages dp where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=dp.degree_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "') " + rights + "";
                degreeCode = getFilterValues(SelQuery);
            }
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sec));
            //finyear = Convert.ToString(ddlfinyear.SelectedItem.Value);
            //if (!string.IsNullOrEmpty(finyear))
            //{
            //    string frDate = finyear.Split('-')[0];
            //    string toDate = finyear.Split('-')[1];
            //    SelQuery = " select distinct finyearpk as code from FM_FinYearMaster where CollegeCode in('" + collegecode + "') and FinYearStart='" + frDate.Split('/')[1] + "/" + frDate.Split('/')[0] + "/" + frDate.Split('/')[2] + "' and FinYearEnd='" + toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2] + "'  ";
            //    finyear = getFilterValues(SelQuery);
            //}
            finyear = Convert.ToString(getCblSelectedValue(chklsfyear));
            studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            //  string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            string strInclude = getStudCategory();
            StringBuilder sbFinlYr = new StringBuilder();
            Dictionary<string, string> htFinlYR = getFinancialYear();
            if (chklsfyear.Items.Count > 0)
            {
                for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
                {
                    if (!chklsfyear.Items[fnl].Selected)
                        continue;
                    for (int clg = 0; clg < cblclg.Items.Count; clg++)
                    {
                        if (!cblclg.Items[clg].Selected)
                            continue;
                        string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + cblclg.Items[clg].Value);//to pass value get key from dictionary 
                        sbFinlYr.Append(KeyVal + "','");
                    }

                }
                if (sbFinlYr.Length > 0)
                    sbFinlYr.Remove(sbFinlYr.Length - 3, 3);

            }
            string strCondition = string.Empty;//added by sudhagar 31.07.2017

            if (rblMode.SelectedIndex == 0)//general
            {
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                {
                    #region Query
                    // string hdFK = getHeaderFK(hdText, collegecode);

                    string SelQ = string.Empty;
                    if (string.IsNullOrEmpty(seattype))
                    {
                        if (string.IsNullOrEmpty(quota))
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";
                            if (checkSchoolSetting() != 0)
                            {
                                SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";
                            }
                            else
                            {
                                SelQ += " select sum(isnull(debit,'0'))-sum(isnull(CREDIT,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and actualfinyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                            }
                            SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " order by r.college_code,r.batch_year";
                            //,f.feecategory
                            SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f where r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                            //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");
                    #endregion
                        }
                        else
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and a.app_no=r.app_no  and a.quota in('" + quota + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no and a.app_no=r.app_no and a.quota in('" + quota + "') and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";
                            if (checkSchoolSetting() != 0)
                            {
                                SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.quota in('" + quota + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";
                            }
                            else
                            {
                                SelQ += " select sum(isnull(debit,'0'))-sum(isnull(CREDIT,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.quota in('" + quota + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and actualfinyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                            }
                            SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c,applyn a where a.app_no=r.app_no   and a.quota in('" + quota + "') and r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " order by r.college_code,r.batch_year";
                            //,f.feecategory
                            SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f,applyn a where a.app_no=r.app_no  and a.quota in('" + quota + "') and r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                            //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(quota))
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and a.app_no=r.app_no and a.seattype in('" + seattype + "')  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no and a.app_no=r.app_no and a.seattype in('" + seattype + "') and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";
                            if (checkSchoolSetting() != 0)
                            {
                                SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";
                            }
                            else
                            {
                                SelQ += " select sum(isnull(debit,'0'))-sum(isnull(CREDIT,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and actualfinyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                            }
                            SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "')   and r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " order by r.college_code,r.batch_year";
                            //,f.feecategory
                            SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "')  and r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                            //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        }
                        else
                        {
                            SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c,applyn a where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and a.app_no=r.app_no and a.seattype in('" + seattype + "') and a.quota in('" + quota + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                            SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r,applyn a where r.app_no=f.app_no and a.app_no=r.app_no and a.seattype in('" + seattype + "')and a.quota in('" + quota + "') and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";
                            if (checkSchoolSetting() != 0)
                            {
                                SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "') and a.quota in('" + quota + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";
                            }
                            else
                            {
                                SelQ += " select sum(isnull(debit,'0'))-sum(isnull(CREDIT,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "') and a.quota in('" + quota + "') and r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and actualfinyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0'";
                                if (cbdate.Checked)
                                    SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                                if (!string.IsNullOrEmpty(studMode))
                                    SelQ += " and r.mode in('" + studMode + "')";
                                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                            }
                            SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "')  and a.quota in('" + quota + "') and r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += " order by r.college_code,r.batch_year";
                            //,f.feecategory
                            SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f,applyn a where a.app_no=r.app_no and a.seattype in('" + seattype + "') and a.quota in('" + quota + "') and r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                            if (!string.IsNullOrEmpty(studMode))
                                SelQ += " and r.mode in('" + studMode + "')";
                            SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                            //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                            dsload.Clear();
                            dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        }
                    }
                }
            }

            else if (rblMode.SelectedIndex == 1)//hostel(added by abarna 26.01.2018)
            {
                if (rblhostelmode.SelectedIndex == 0)//cumulative(added by abarna 26.01.2018)
                {
                    strCondition = " and stud_type in('Hostler')";
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                    {
                        #region Query
                        // string hdFK = getHeaderFK(hdText, collegecode);
                        string SelQ = string.Empty;
                        SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                        SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";

                        SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                        if (cbdate.Checked)
                            SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                        SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " order by r.college_code,r.batch_year";
                        //,f.feecategory
                        SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f where r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                        //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        #endregion
                    }

                }

                if (rblhostelmode.SelectedIndex == 1)//hostelname wise and hostel with collegewise (added by abarna 26.01.2018)
                {
                    strCondition = " and stud_type in('Hostler')";
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                    {

                        #region Query
                        // string hdFK = getHeaderFK(hdText, collegecode);
                        string SelQ = string.Empty;

                        SelQ = "select COUNT(distinct h.app_no)HostelCount,HostelMasterFK,hm.HostelName,c.Coll_acronymn,r.batch_year,r.college_code from HT_HostelRegistration h,HM_HostelMaster hm,registration r,collinfo c where r.college_code=c.college_code  and r.batch_year in('" + batch + "')  and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "  and h.HostelMasterFK=hm.HostelMasterPK and r.app_no=h.APP_No";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by h.HostelMasterFK,c.Coll_acronymn,hm.HostelName,r.batch_year,r.college_code order by h.HostelMasterFK,r.batch_year ";

                        SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,h.hostelmasterfk,r.batch_year,feecategory,r.college_code from ft_feeallot f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no   and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "')  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "  " + strInclude + " group by h.hostelmasterfk,r.batch_year,feecategory,r.college_code order by r.batch_year ";

                        SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,h.hostelmasterfk,r.batch_year,feecategory,r.college_code from ft_findailytransaction f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no and r.batch_year in('" + batch + "')  and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                        if (cbdate.Checked)
                            SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by h.hostelmasterfk,r.batch_year,feecategory,r.college_code order by r.batch_year";
                        SelQ += " select distinct r.batch_year,h.hostelmasterfk from registration r,collinfo c,HT_HostelRegistration h where r.college_code=c.college_code and h.app_no=r.app_no and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "')  and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + "";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " order by r.batch_year";
                        //,f.feecategory
                        SelQ += " select  count(distinct r.app_no) as studcount,hm.HostelName,h.hostelmasterfk from registration r,collinfo c,ft_feeallot f,HM_HostelMaster hm,HT_HostelRegistration h where r.college_code=c.college_code and h.HostelMasterFK=hm.HostelMasterPK and  h.app_no=r.app_no and f.app_no=r.app_no  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "')  and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by hm.HostelName,h.hostelmasterfk  order by hm.HostelName";


                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        #endregion
                    }
                }
                if (rblhostelmode.SelectedIndex == 2)// hostel with collegewise (added by abarna 26.01.2018)
                {
                    strCondition = " and stud_type in('Hostler')";
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                    {
                        #region Query
                        // string hdFK = getHeaderFK(hdText, collegecode);
                        string SelQ = string.Empty;

                        SelQ = "select COUNT(distinct h.app_no)HostelCount,HostelMasterFK,hm.HostelName,c.Coll_acronymn,r.batch_year,r.college_code from HT_HostelRegistration h,HM_HostelMaster hm,registration r,collinfo c where r.college_code=c.college_code  and r.batch_year in('" + batch + "')  and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.college_code in('" + collegecode + "') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no) " + strCondition + "  and h.HostelMasterFK=hm.HostelMasterPK and r.app_no=h.APP_No";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by h.HostelMasterFK,c.Coll_acronymn,hm.HostelName,r.batch_year,r.college_code,h.HostelMasterFK order by h.HostelMasterFK,r.batch_year ";

                        SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,h.hostelmasterfk,r.batch_year,feecategory,r.college_code from ft_feeallot f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no   and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "')  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code in('" + collegecode + "') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "  " + strInclude + " group by h.hostelmasterfk,r.batch_year,feecategory,r.college_code order by h.HostelMasterFK,r.batch_year,r.college_code";
                        //           SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + finyear + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                        //if (!string.IsNullOrEmpty(studMode))
                        //    SelQ += " and r.mode in('" + studMode + "')";
                        //SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";


                        SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,h.hostelmasterfk,r.batch_year,feecategory,r.college_code from ft_findailytransaction f,registration r,HT_HostelRegistration h where h.app_no=r.app_no and r.app_no=f.app_no and r.batch_year in('" + batch + "')  and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0' and r.college_code in('" + collegecode + "') ";
                        if (cbdate.Checked)
                            SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by h.hostelmasterfk,r.batch_year,feecategory,r.college_code order by r.batch_year";
                        SelQ += " select distinct r.batch_year,h.hostelmasterfk ,r.college_code from registration r,collinfo c,HT_HostelRegistration h where r.college_code=c.college_code and h.app_no=r.app_no and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.college_code in('" + collegecode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += " order by r.batch_year";
                        //,f.feecategory
                        SelQ += " select  count(distinct r.app_no) as studcount,hm.HostelName,h.hostelmasterfk,r.college_code,r.batch_year from registration r,collinfo c,ft_feeallot f,HM_HostelMaster hm,HT_HostelRegistration h where r.college_code=c.college_code and h.HostelMasterFK=hm.HostelMasterPK and  h.app_no=r.app_no and f.app_no=r.app_no  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and r.college_code in('" + collegecode + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                        if (!string.IsNullOrEmpty(studMode))
                            SelQ += " and r.mode in('" + studMode + "')";
                        SelQ += "" + strInclude + " group by hm.HostelName,h.hostelmasterfk,r.college_code,r.batch_year  order by hm.HostelName,r.college_code";


                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(SelQ, "Text");
                        #endregion
                    }


                }
            }
            else
            {
                strCondition = " and isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>''";
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                {
                    #region Query
                    // string hdFK = getHeaderFK(hdText, collegecode);
                    string SelQ = string.Empty;
                    SelQ = " select  count(distinct r.app_no) as studcount,r.college_code,c.Coll_acronymn from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)  " + strCondition + "";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "" + strInclude + " group by r.college_code ,c.Coll_acronymn order by r.college_code";

                    SelQ += " select distinct sum(FeeAmount) as Allot,SUM(DeductAmout) as Concession ,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code,r.batch_year,feecategory from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "')  " + strCondition + " and isnull(istransfer,'0')='0'";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "  " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code,f.feecategory ";

                    SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code,r.batch_year,feecategory from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.ledgerfk in('" + ldText + "') and f.paymode in('" + payMode + "')  and finyearfk in('" + Convert.ToString(sbFinlYr) + "') " + strCondition + " and isnull(paid_Istransfer,'0')='0'";
                    if (cbdate.Checked)
                        SelQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code,r.batch_year,feecategory order by r.college_code";

                    SelQ += " select distinct r.college_code,r.batch_year from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') " + strCondition + "";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += " order by r.college_code,r.batch_year";
                    //,f.feecategory
                    SelQ += " select  count(distinct r.app_no) as studcount,r.college_code,c.collname,r.batch_year from registration r,collinfo c,ft_feeallot f where r.college_code=c.college_code and f.app_no=r.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  " + strCondition + " and r.app_no in(select distinct app_no from ft_feeallot f where f.app_no=r.app_no)";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "" + strInclude + " group by r.college_code ,c.collname,r.batch_year order by r.college_code";
                    //  SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code"; ,f.feecategory
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(SelQ, "Text");
                    #endregion
                }
            }


            if (cbloadMaster.Checked)
            {
                string linkName = string.Empty;
                switch (rblMode.SelectedIndex)
                {
                    case 0:
                        linkName = "43LoadMasterGeneral";
                        break;
                    case 1:
                        linkName = "43LoadMasterHostel";
                        break;
                    case 2:
                        linkName = "43LoadMasterTransport";
                        break;
                }
                string hdTexts = Convert.ToString(getCblSelectedTempText(chkl_studhed));
                string ldTexts = Convert.ToString(getCblSelectedTempText(chkl_studled));
                bool boolSel = true;
                string linkValue = loadMaster(linkName, hdTexts, ldTexts, boolSel);
            }
            #endregion



        }
        catch { }
        return dsload;
    }

    protected string getFilterValues(string SelQuery)
    {
        string getValues = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            DataSet dsval = d2.select_method_wo_parameter(SelQuery, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["code"]);
                }
                getValues = string.Join("','", headerFK);
            }
        }
        catch { getValues = string.Empty; }
        return getValues;
    }

    //added by abarna 26.01.2018
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }

    protected DataTable loadDetails(DataSet ds)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("College");
            dtpaid.Columns.Add("CollegeCode");
            dtpaid.Columns.Add("HostelMasterPK");
            dtpaid.Columns.Add("Hostelname");

            dtpaid.Columns.Add("Student");
            dtpaid.Columns.Add("Allot");
            dtpaid.Columns.Add("Concession");
            dtpaid.Columns.Add("Total");
            dtpaid.Columns.Add("Receipt");
            dtpaid.Columns.Add("credit");
            dtpaid.Columns.Add("Balance");
            if (dtpaid.Columns.Count > 0)
            {
                int rowCnt = 0;
                DataRow drpaid;
                Hashtable httotal = new Hashtable();
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    #region
                    drpaid = dtpaid.NewRow();
                    double demandAmt = 0;
                    double allotAmt = 0;
                    double concessionAmt = 0;
                    double paidAmt = 0;
                    double paidAmtCr = 0;
                    double balAmt = 0;
                    double studCnt = 0;
                    if (rblMode.SelectedIndex == 0 || rblMode.SelectedIndex == 2 || rblhostelmode.SelectedIndex == 0)
                    {
                        drpaid["Sno"] = Convert.ToString(++rowCnt);
                        drpaid["College"] = Convert.ToString(ds.Tables[0].Rows[row]["Coll_acronymn"]);
                        drpaid["CollegeCode"] = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);

                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["studcount"]), out studCnt);
                        drpaid["Student"] = Convert.ToString(studCnt);
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "'";
                            DataView dvpaid = ds.Tables[1].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                //================Added by saranya on 12/01/2018======================//

                                double.TryParse(Convert.ToString(dvpaid[0]["Allot"]), out allotAmt);
                                double.TryParse(Convert.ToString(dvpaid[0]["Concession"]), out concessionAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paidAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["bal"]), out balAmt);

                                //==================================================================//
                                double.TryParse(Convert.ToString(dvpaid[0]["total"]), out demandAmt);
                            }
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "'";
                            DataView dvpaid = ds.Tables[2].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out paidAmt);
                                double.TryParse(Convert.ToString(dvpaid[0]["credit"]), out paidAmtCr);
                            }
                        }
                        //================Added by saranya on 12/01/2018======================//
                        drpaid["Allot"] = Convert.ToString(allotAmt);
                        drpaid["Concession"] = Convert.ToString(concessionAmt);
                        //==================================================================//
                        drpaid["Total"] = Convert.ToString(demandAmt);
                        drpaid["Receipt"] = Convert.ToString(paidAmt);
                        drpaid["credit"] = Convert.ToString(paidAmtCr);
                        balAmt = demandAmt - paidAmt;
                        drpaid["Balance"] = Convert.ToString(balAmt);
                        dtpaid.Rows.Add(drpaid);

                        if (!httotal.ContainsKey("Student"))
                            httotal.Add("Student", studCnt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                            amount += studCnt;
                            httotal.Remove("Student");
                            httotal.Add("Student", Convert.ToString(amount));
                        }
                        //================Added by saranya on 12/01/2018======================//
                        if (!httotal.ContainsKey("Allot"))
                            httotal.Add("Allot", allotAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                            amount += allotAmt;
                            httotal.Remove("Allot");
                            httotal.Add("Allot", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Concession"))
                            httotal.Add("Concession", concessionAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                            amount += concessionAmt;
                            httotal.Remove("Concession");
                            httotal.Add("Concession", Convert.ToString(amount));
                        }
                        //==================================================================//
                        if (!httotal.ContainsKey("Total"))
                            httotal.Add("Total", demandAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                            amount += demandAmt;
                            httotal.Remove("Total");
                            httotal.Add("Total", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Receipt"))
                            httotal.Add("Receipt", paidAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                            amount += paidAmt;
                            httotal.Remove("Receipt");
                            httotal.Add("Receipt", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Balance"))
                            httotal.Add("Balance", balAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                            amount += balAmt;
                            httotal.Remove("Balance");
                            httotal.Add("Balance", Convert.ToString(amount));
                        }
                    #endregion


                    }
                }
                if (rblhostelmode.SelectedIndex == 1)
                {
                    drpaid = dtpaid.NewRow();
                    double demandAmt = 0;
                    double allotAmt = 0;
                    double concessionAmt = 0;
                    double paidAmt = 0;
                    double paidAmtCr = 0;
                    double balAmt = 0;
                    double studCnt = 0;
                    string hostelname = string.Empty;
                    string hostelcode = returnwithsinglecodevalue(cbl_hostelname);

                    DataTable dthostel = ds.Tables[0].DefaultView.ToTable(true, "HostelMasterFK", "HostelName");
                    for (int i = 0; i < dthostel.Rows.Count; i++)
                    {
                        drpaid = dtpaid.NewRow();

                        drpaid["Sno"] = Convert.ToString(++rowCnt);

                        // drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]);

                        //drpaid["Hostelname"] = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]);

                        hostelname = Convert.ToString(dthostel.Rows[i]["HostelName"]);
                        drpaid["HostelName"] = Convert.ToString(hostelname);
                        if (!httotal.ContainsKey("HostelName"))
                            httotal.Add("HostelName", hostelname);
                        hostelcode = Convert.ToString(dthostel.Rows[i]["HostelMasterFK"]);

                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(HostelCount)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out studCnt);
                        drpaid["Student"] = Convert.ToString(studCnt);

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')";
                            DataView dvpaid = ds.Tables[1].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                //================Added by saranya on 12/01/2018======================//
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Allot)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out allotAmt);
                                // double.TryParse(Convert.ToString(dvpaid[0]["Allot"]), out allotAmt);
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Concession)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out concessionAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["Concession"]), out concessionAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paidAmt);

                                //double.TryParse(Convert.ToString(dvpaid[0]["bal"]), out balAmt);

                                //==================================================================//
                                //double.TryParse(Convert.ToString(dvpaid[0]["total"]), out demandAmt);
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(total)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out demandAmt);
                            }
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')";
                            DataView dvpaid = ds.Tables[2].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(debit)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out paidAmt);
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(credit)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out paidAmtCr);
                                //double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out paidAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["credit"]), out paidAmtCr);
                            }
                        }
                        //================Added by saranya on 12/01/2018======================//
                        drpaid["Allot"] = Convert.ToString(allotAmt);
                        drpaid["Concession"] = Convert.ToString(concessionAmt);
                        //==================================================================//
                        drpaid["Total"] = Convert.ToString(demandAmt);
                        drpaid["Receipt"] = Convert.ToString(paidAmt);
                        drpaid["credit"] = Convert.ToString(paidAmtCr);
                        balAmt = demandAmt - paidAmt;
                        drpaid["Balance"] = Convert.ToString(balAmt);
                        dtpaid.Rows.Add(drpaid);

                        if (!httotal.ContainsKey("Student"))
                            httotal.Add("Student", studCnt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                            amount += studCnt;
                            httotal.Remove("Student");
                            httotal.Add("Student", Convert.ToString(amount));
                        }
                        //================Added by saranya on 12/01/2018======================//
                        if (!httotal.ContainsKey("Allot"))
                            httotal.Add("Allot", allotAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                            amount += allotAmt;
                            httotal.Remove("Allot");
                            httotal.Add("Allot", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Concession"))
                            httotal.Add("Concession", concessionAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                            amount += concessionAmt;
                            httotal.Remove("Concession");
                            httotal.Add("Concession", Convert.ToString(amount));
                        }
                        //==================================================================//
                        if (!httotal.ContainsKey("Total"))
                            httotal.Add("Total", demandAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                            amount += demandAmt;
                            httotal.Remove("Total");
                            httotal.Add("Total", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Receipt"))
                            httotal.Add("Receipt", paidAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                            amount += paidAmt;
                            httotal.Remove("Receipt");
                            httotal.Add("Receipt", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Balance"))
                            httotal.Add("Balance", balAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                            amount += balAmt;
                            httotal.Remove("Balance");
                            httotal.Add("Balance", Convert.ToString(amount));
                        }
                    }

                }
                if (rblhostelmode.SelectedIndex == 2)
                {
                    drpaid = dtpaid.NewRow();
                    double demandAmt = 0;
                    double allotAmt = 0;
                    double concessionAmt = 0;
                    double paidAmt = 0;
                    double paidAmtCr = 0;
                    double balAmt = 0;
                    double studCnt = 0;
                    string hostelname = string.Empty;
                    string collegename = string.Empty;
                    string hostelcode = returnwithsinglecodevalue(cbl_hostelname);
                    string collegecode = returnwithsinglecodevalue(cblclg);
                    DataTable dthostel = ds.Tables[0].DefaultView.ToTable(true, "HostelMasterFK", "HostelName", "collegecode", "Coll_acronymn");
                    for (int i = 0; i < dthostel.Rows.Count; i++)
                    {
                        drpaid = dtpaid.NewRow();

                        drpaid["Sno"] = Convert.ToString(++rowCnt);

                        // drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]);

                        //drpaid["Hostelname"] = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]);

                        hostelname = Convert.ToString(dthostel.Rows[i]["HostelName"]);
                        drpaid["HostelName"] = Convert.ToString(hostelname);
                        collegename = Convert.ToString(dthostel.Rows[i]["Coll_acronymn"]);
                        drpaid["College"] = Convert.ToString(collegename);

                        if (!httotal.ContainsKey("HostelName"))
                            httotal.Add("HostelName", hostelname);
                        hostelcode = Convert.ToString(dthostel.Rows[i]["HostelMasterFK"]);
                        collegecode = Convert.ToString(dthostel.Rows[i]["collegecode"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(HostelCount)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and Collegecode in('" + Convert.ToString(collegecode) + "')")), out studCnt);
                        drpaid["Student"] = Convert.ToString(studCnt);

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')";
                            DataView dvpaid = ds.Tables[1].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                //================Added by saranya on 12/01/2018======================//
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Allot)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')")), out allotAmt);
                                // double.TryParse(Convert.ToString(dvpaid[0]["Allot"]), out allotAmt);
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(Concession)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')")), out concessionAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["Concession"]), out concessionAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paidAmt);

                                //double.TryParse(Convert.ToString(dvpaid[0]["bal"]), out balAmt);

                                //==================================================================//
                                //double.TryParse(Convert.ToString(dvpaid[0]["total"]), out demandAmt);
                                double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(total)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')")), out demandAmt);
                            }
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')";
                            DataView dvpaid = ds.Tables[2].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(debit)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')")), out paidAmt);
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(credit)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code in('" + Convert.ToString(collegecode) + "')")), out paidAmtCr);
                                //double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out paidAmt);
                                //double.TryParse(Convert.ToString(dvpaid[0]["credit"]), out paidAmtCr);
                            }
                        }
                        //================Added by saranya on 12/01/2018======================//
                        drpaid["Allot"] = Convert.ToString(allotAmt);
                        drpaid["Concession"] = Convert.ToString(concessionAmt);
                        //==================================================================//
                        drpaid["Total"] = Convert.ToString(demandAmt);
                        drpaid["Receipt"] = Convert.ToString(paidAmt);
                        drpaid["credit"] = Convert.ToString(paidAmtCr);
                        balAmt = demandAmt - paidAmt;
                        drpaid["Balance"] = Convert.ToString(balAmt);
                        dtpaid.Rows.Add(drpaid);

                        if (!httotal.ContainsKey("Student"))
                            httotal.Add("Student", studCnt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                            amount += studCnt;
                            httotal.Remove("Student");
                            httotal.Add("Student", Convert.ToString(amount));
                        }
                        //================Added by saranya on 12/01/2018======================//
                        if (!httotal.ContainsKey("Allot"))
                            httotal.Add("Allot", allotAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                            amount += allotAmt;
                            httotal.Remove("Allot");
                            httotal.Add("Allot", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Concession"))
                            httotal.Add("Concession", concessionAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                            amount += concessionAmt;
                            httotal.Remove("Concession");
                            httotal.Add("Concession", Convert.ToString(amount));
                        }
                        //==================================================================//
                        if (!httotal.ContainsKey("Total"))
                            httotal.Add("Total", demandAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                            amount += demandAmt;
                            httotal.Remove("Total");
                            httotal.Add("Total", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Receipt"))
                            httotal.Add("Receipt", paidAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                            amount += paidAmt;
                            httotal.Remove("Receipt");
                            httotal.Add("Receipt", Convert.ToString(amount));
                        }
                        if (!httotal.ContainsKey("Balance"))
                            httotal.Add("Balance", balAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                            amount += balAmt;
                            httotal.Remove("Balance");
                            httotal.Add("Balance", Convert.ToString(amount));
                        }
                    }
                }


                if (httotal.Count > 0)
                {
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Total" + "-" + "*";
                    foreach (DictionaryEntry dtrow in httotal)
                    {
                        drpaid[Convert.ToString(dtrow.Key)] = Convert.ToString(dtrow.Value);
                    }
                    dtpaid.Rows.Add(drpaid);
                }
            }
        }
        catch { }
        return dtpaid;
    }

    protected Dictionary<string, string> getCurSem()
    {
        Dictionary<string, string> dtcurSem = new Dictionary<string, string>();
        try
        {
            int year = 0;
            int.TryParse(Convert.ToString(DateTime.Now.ToString("yyyy")), out year);
            dtcurSem.Add(Convert.ToString(year), Convert.ToString("1,2"));
            dtcurSem.Add(Convert.ToString(year - 1), Convert.ToString("3,4"));
            dtcurSem.Add(Convert.ToString(year - 2), Convert.ToString("5,6"));
            dtcurSem.Add(Convert.ToString(year - 3), Convert.ToString("7,8"));
            dtcurSem.Add(Convert.ToString(year - 4), Convert.ToString("9,10"));
            dtcurSem.Add(Convert.ToString(year - 5), Convert.ToString("11,12"));
            dtcurSem.Add(Convert.ToString(year - 6), Convert.ToString("12,13"));
            dtcurSem.Add(Convert.ToString(year - 7), Convert.ToString("14,15"));
        }
        catch { }
        return dtcurSem;
    }

    protected DataTable loadDetailsCurSem(DataSet ds)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("College");
            dtpaid.Columns.Add("CollegeCode");
            dtpaid.Columns.Add("HostelMasterPK");
            dtpaid.Columns.Add("Hostelname");
            dtpaid.Columns.Add("Student");
            dtpaid.Columns.Add("Allot");
            dtpaid.Columns.Add("Concession");
            dtpaid.Columns.Add("Total");
            dtpaid.Columns.Add("Receipt");
            dtpaid.Columns.Add("credit");
            dtpaid.Columns.Add("Balance");
            Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                DataSet dsNormal = ds.Copy();
                try
                {
                    string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                    DataSet dsFinal = new DataSet();
                    DataTable dtAllot = new DataTable();
                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            ds.Tables[3].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "'";
                            DataTable dtYear = ds.Tables[3].DefaultView.ToTable();
                            if (checkSchoolSetting() != 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'  and  batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";//
                                dtAllot = ds.Tables[1].DefaultView.ToTable();
                                ds.Tables[2].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'and  batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";// and batch_year='" + getVal.Key.Split('$')[1] + "' 
                            }
                            else
                            {
                                ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and feecategory in('" + feeCate + "')";//
                                dtAllot = ds.Tables[1].DefaultView.ToTable();
                                ds.Tables[2].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and  feecategory in('" + feeCate + "')";// and batch_year='" + getVal.Key.Split('$')[1] + "' 
                            }

                            DataTable dtPaid = ds.Tables[2].DefaultView.ToTable();
                            if (checkSchoolSetting() != 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "'";// and feecategory in('" + feeCate + "')";
                            }
                            else
                            {
                                ds.Tables[4].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'";
                            }
                            DataTable dtstudCnt = ds.Tables[4].DefaultView.ToTable();
                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtFirst);
                                dsFinal.Tables.Add(dtAllot);
                                dsFinal.Tables.Add(dtPaid);
                                dsFinal.Tables.Add(dtYear);
                                dsFinal.Tables.Add(dtstudCnt);
                                boolDs = true;
                            }
                            else
                            {
                                // ds.Merge(ds.Tables[0]);
                                dsFinal.Merge(dtAllot);
                                dsFinal.Merge(dtPaid);
                                dsFinal.Merge(dtYear);
                                dsFinal.Merge(dtstudCnt);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            DataRow drpaid;
            Hashtable httotal = new Hashtable();
            Dictionary<string, string> currentSem = getCurSem();

            if (dtpaid.Columns.Count > 0)
            {
                bool boolClg = false;
                int rowCnt = 0;


                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    #region
                    drpaid = dtpaid.NewRow();
                    double demandAmt = 0;
                    double paidAmt = 0;
                    double paidAmtCr = 0;
                    double balAmt = 0;
                    double studCnt = 0;
                    double concessionamt = 0;
                    double allotamt = 0;
                    double tempPaidDr = 0;
                    double tempPaidCr = 0;
                    double tempAllot = 0;
                    double allotmentAmount = 0;
                    double ConcessionAmount = 0;
                    if (rblMode.SelectedIndex == 0 || rblMode.SelectedIndex == 2 || rblhostelmode.SelectedIndex == 0)
                    {
                        string clgCode = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);

                        //drpaid["Sno"] = Convert.ToString(++rowCnt);
                        //drpaid["College"] = Convert.ToString(ds.Tables[0].Rows[row]["collname"]);
                        //drpaid["CollegeCode"] = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                        //drpaid["Student"] = Convert.ToString(studCnt);

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            boolClg = false;

                            ds.Tables[3].DefaultView.RowFilter = "college_Code='" + clgCode + "'";
                            DataTable dtbatch = ds.Tables[3].DefaultView.ToTable();
                            if (dtbatch.Rows.Count > 0)
                            {
                                for (int bat = 0; bat < dtbatch.Rows.Count; bat++)
                                {
                                    string batch = Convert.ToString(dtbatch.Rows[bat]["batch_year"]);
                                    string curSem = string.Empty;
                                    string curSemCode = string.Empty;
                                    if (!cbAcdYear.Checked)
                                    {
                                        if (currentSem.ContainsKey(batch))
                                            curSem = Convert.ToString(currentSem[batch]);
                                        Dictionary<string, string> getFeeCode = getFeecode(clgCode);//get current sem code 
                                        curSem = getCurYear(curSem);
                                        if (getFeeCode.ContainsKey(curSem))
                                            curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                                    }
                                    else
                                    {
                                        if (getAcdYear.ContainsKey(clgCode + "$" + batch))
                                        {
                                            curSemCode = Convert.ToString(getAcdYear[clgCode + "$" + batch]);
                                            curSemCode = curSemCode.Replace(",", "','");
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(curSemCode))
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            if (checkSchoolSetting() != 0)
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "' and feecategory in('" + curSemCode + "') and batch_year='" + batch + "'";
                                            }
                                            else
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "' and feecategory in('" + curSemCode + "')";


                                            }
                                            DataTable dvpaid = ds.Tables[1].DefaultView.ToTable();


                                            if (dvpaid.Rows.Count > 0)
                                            {
                                                //------------Added by abarna------------
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(Allot)", "")), out allotamt);
                                                allotmentAmount += allotamt;
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(Concession)", "")), out concessionamt);
                                                ConcessionAmount += concessionamt;
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(total)", "")), out tempAllot);
                                                demandAmt += tempAllot;
                                                //double.TryParse(Convert.ToString(dvpaid[0]["total"]), out tempAllot);

                                            }
                                        }
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            if (checkSchoolSetting() != 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "' and feecategory in('" + curSemCode + "') and batch_year='" + batch + "'";
                                            }
                                            else
                                            {

                                                ds.Tables[2].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "' and feecategory in('" + curSemCode + "')";


                                            }
                                            DataTable dvpaid = ds.Tables[2].DefaultView.ToTable();
                                            if (dvpaid.Rows.Count > 0)
                                            {

                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(debit)", "")), out tempPaidDr);
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(credit)", "")), out tempPaidCr);
                                                //double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out tempPaidDr);
                                                //double.TryParse(Convert.ToString(dvpaid[0]["credit"]), out tempPaidCr);
                                                paidAmt += tempPaidDr;
                                                paidAmtCr += tempPaidCr;
                                            }
                                        }
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {

                                            if (checkSchoolSetting() != 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "' and batch_year='" + batch + "'";// and feecategory in('" + curSemCode + "')";


                                            }
                                            else
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "'";// and feecategory in('" + curSemCode + "')";


                                            }

                                            DataTable dvpaids = ds.Tables[4].DefaultView.ToTable();
                                            if (dvpaids.Rows.Count > 0)
                                            {
                                                double tempStud = 0;
                                                if (checkSchoolSetting() != 0)
                                                {
                                                    DataTable dtColumns = dvpaids.DefaultView.ToTable(true, "studCount", "college_code", "collname", "batch_year");
                                                    double.TryParse(Convert.ToString(dvpaids.Rows[0]["studcount"]), out tempStud);
                                                    studCnt += tempStud;
                                                }
                                                else
                                                {
                                                    DataTable dtColumns = dvpaids.DefaultView.ToTable(true, "studCount", "college_code", "collname");
                                                    double.TryParse(Convert.ToString(dvpaids.Compute("sum(studCount)", "")), out tempStud);
                                                    studCnt += tempStud;
                                                }
                                                //for (int rows = 0; rows < dvpaids.Rows.Count; rows++)
                                                //{
                                                //    double.TryParse(Convert.ToString(dvpaids.Rows[rows]["studcount"]), out tempStud);
                                                //    studCnt += tempStud;
                                                //}
                                                //double.TryParse(Convert.ToString(dvpaids.Compute("count(studcount","")), out tempStud);


                                            }
                                        }
                                        boolClg = true;
                                    }
                                }
                            }
                        }

                        if (boolClg)
                        {
                            drpaid["Sno"] = Convert.ToString(++rowCnt);
                            drpaid["College"] = Convert.ToString(ds.Tables[0].Rows[row]["Coll_acronymn"]);
                            drpaid["CollegeCode"] = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                            drpaid["Student"] = Convert.ToString(studCnt);
                            //----------------------added by abarna 20.01.2018-------------
                            drpaid["Allot"] = Convert.ToString(allotmentAmount);
                            drpaid["Concession"] = Convert.ToString(ConcessionAmount);
                            //-------------------------------------------------------------
                            drpaid["Total"] = Convert.ToString(demandAmt);
                            drpaid["Receipt"] = Convert.ToString(paidAmt);
                            drpaid["credit"] = Convert.ToString(paidAmtCr);
                            balAmt = demandAmt - paidAmt;
                            drpaid["Balance"] = Convert.ToString(balAmt);
                            dtpaid.Rows.Add(drpaid);


                            if (!httotal.ContainsKey("Student"))
                                httotal.Add("Student", studCnt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                                amount += studCnt;
                                httotal.Remove("Student");
                                httotal.Add("Student", Convert.ToString(amount));
                            }

                            //----------------------added by saranya -------------
                            if (!httotal.ContainsKey("Allot"))
                                httotal.Add("Allot", allotmentAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                                amount += allotmentAmount;
                                httotal.Remove("Allot");
                                httotal.Add("Allot", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Concession"))
                                httotal.Add("Concession", ConcessionAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                                amount += ConcessionAmount;
                                httotal.Remove("Concession");
                                httotal.Add("Concession", Convert.ToString(amount));
                            }

                            //-------------------------------------
                            if (!httotal.ContainsKey("Total"))
                                httotal.Add("Total", demandAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                                amount += demandAmt;
                                httotal.Remove("Total");
                                httotal.Add("Total", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Receipt"))
                                httotal.Add("Receipt", paidAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                                amount += paidAmt;
                                httotal.Remove("Receipt");
                                httotal.Add("Receipt", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Balance"))
                                httotal.Add("Balance", balAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                                amount += balAmt;
                                httotal.Remove("Balance");
                                httotal.Add("Balance", Convert.ToString(amount));
                            }
                        }
                    #endregion
                    }
                }

                if (rblhostelmode.SelectedIndex == 1)
                {
                    drpaid = dtpaid.NewRow();

                    DataTable dthostel = ds.Tables[0].DefaultView.ToTable(true, "HostelMasterFK", "HostelName");
                    for (int i = 0; i < dthostel.Rows.Count; i++)
                    {
                        double demandAmt = 0;
                        double paidAmt = 0;
                        double paidAmtCr = 0;
                        double balAmt = 0;
                        double studCnt = 0;
                        double concessionamt = 0;
                        double allotamt = 0;
                        double tempAllot = 0;
                        double tempPaidDr = 0;
                        double tempPaidCr = 0;
                        double allotmentAmount = 0;
                        double ConcessionAmount = 0;
                        string hostelname = string.Empty;
                        string hostelcode = returnwithsinglecodevalue(cbl_hostelname);

                        drpaid = dtpaid.NewRow();

                        //drpaid["Sno"] = Convert.ToString(++rowCnt);

                        // drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]);

                        //drpaid["Hostelname"] = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]);

                        //hostelname = Convert.ToString(dthostel.Rows[i]["HostelName"]);
                        //drpaid["HostelName"] = Convert.ToString(hostelname);
                        //if (!httotal.ContainsKey("HostelName"))
                        //    httotal.Add("HostelName", hostelname);
                        hostelcode = Convert.ToString(dthostel.Rows[i]["HostelMasterFK"]);

                        //double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(HostelCount)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out studCnt);
                        //drpaid["Student"] = Convert.ToString(studCnt);
                        //string clgCode = Convert.ToString(ds.Tables[0].Rows[i]["collegecode"]);
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            boolClg = false;
                            ds.Tables[3].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') ";//and college_Code='" + clgCode + "'
                            DataTable dtbatch = ds.Tables[3].DefaultView.ToTable();
                            if (dtbatch.Rows.Count > 0)
                            {
                                for (int bat = 0; bat < dtbatch.Rows.Count; bat++)
                                {
                                    string batch = Convert.ToString(dtbatch.Rows[bat]["batch_year"]);
                                    string curSem = string.Empty;
                                    string curSemCode = string.Empty;
                                    if (!cbAcdYear.Checked)
                                    {
                                        if (currentSem.ContainsKey(batch))
                                            curSem = Convert.ToString(currentSem[batch]);
                                        Dictionary<string, string> getFeeCode = getFeecode(batch);//get current sem code 
                                        curSem = getCurYear(curSem);
                                        if (getFeeCode.ContainsKey(curSem))
                                            curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                                    }
                                    else
                                    {
                                        if (getAcdYear.ContainsKey(batch))
                                        {
                                            curSemCode = Convert.ToString(getAcdYear[batch]);
                                            curSemCode = curSemCode.Replace(",", "','");
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(curSemCode))
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            // string hostelPk=returnwithsinglecodevalue(cbl_hostelname)
                                            ds.Tables[1].DefaultView.RowFilter = "batch_year='" + batch + "'  and HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')";// feecategory in('" + curSemCode + "') and batch_year='" + batch + "' and 
                                            DataTable dvpaid = ds.Tables[1].DefaultView.ToTable();

                                            if (dvpaid.Rows.Count > 0)
                                            {
                                                //------------Added by abarna------------
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(Allot)", "")), out allotamt);// and college_Code='" + clgCode + "'
                                                allotmentAmount += allotamt;
                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(Concession)", "")), out concessionamt);//and college_Code='" + clgCode + "'
                                                ConcessionAmount += concessionamt;
                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(total)", "")), out tempAllot);//and college_Code='" + clgCode + "'
                                                demandAmt += tempAllot;
                                            }
                                        }
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = " batch_year='" + batch + "' and HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')";//feecategory in('" + curSemCode + "') and
                                            DataTable dvpaid = ds.Tables[2].DefaultView.ToTable();
                                            if (dvpaid.Rows.Count > 0)
                                            {

                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(debit)", "")), out tempPaidDr);//and college_Code='" + clgCode + "'
                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(credit)", "")), out tempPaidCr);//and college_Code='" + clgCode + "'
                                                paidAmt += tempPaidDr;
                                                paidAmtCr += tempPaidCr;
                                            }
                                        }
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')";// and feecategory in('" + curSemCode + "')
                                            DataTable dvpaids = ds.Tables[4].DefaultView.ToTable();


                                            //double tempStud = 0;
                                            //DataTable dtColumns = dvpaids.DefaultView.ToTable(true, "studCount", "hostelname", "HostelMasterFK");
                                            ////for (int rows = 0; rows < dvpaids.Rows.Count; rows++)
                                            ////{
                                            ////    double.TryParse(Convert.ToString(dvpaids.Rows[rows]["studcount"]), out tempStud);
                                            ////    studCnt += tempStud;
                                            ////}
                                            ////double.TryParse(Convert.ToString(dvpaids.Compute("count(studcount","")), out tempStud);
                                            //double.TryParse(Convert.ToString(dvpaids.Rows[0]["studcount"]), out tempStud);
                                            //studCnt += tempStud;
                                            //if (dvpaids.Rows.Count > 0)
                                            //{
                                            //    double tempStud = 0;
                                            //    DataTable dtColumns = dvpaids.DefaultView.ToTable(true, "HostelCount", "HostelMasterFK", "batch_year");
                                            //    //for (int rows = 0; rows < dvpaids.Rows.Count; rows++)
                                            //    //{
                                            //    //    double.TryParse(Convert.ToString(dvpaids.Rows[rows]["studcount"]), out tempStud);
                                            //    //    studCnt += tempStud;
                                            //    //}
                                            //    //double.TryParse(Convert.ToString(dvpaids.Compute("count(HostelCount", "")), out tempStud);
                                            //    double.TryParse(Convert.ToString(dvpaids.Rows[0]["HostelCount"]), out tempStud);
                                            //    studCnt += tempStud;

                                            //}
                                        }
                                        boolClg = true;
                                    }
                                }
                            }
                        }

                        if (boolClg)
                        {
                            // drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]);

                            //drpaid["Hostelname"] = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]);

                            //hostelname = Convert.ToString(dthostel.Rows[i]["HostelName"]);
                            //drpaid["HostelName"] = Convert.ToString(hostelname);
                            //if (!httotal.ContainsKey("HostelName"))
                            //    httotal.Add("HostelName", hostelname);
                            //hostelcode = Convert.ToString(dthostel.Rows[i]["HostelMasterFK"]);

                            double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(HostelCount)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')")), out studCnt);
                            //drpaid["Student"] = Convert.ToString(studCnt);
                            drpaid["Sno"] = Convert.ToString(++rowCnt);
                            drpaid["Hostelname"] = Convert.ToString(ds.Tables[4].Rows[i]["HostelName"]);
                            drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[4].Rows[i]["HostelMasterFK"]);
                            drpaid["Student"] = Convert.ToString(studCnt);
                            //----------------------added by abarna 20.01.2018-------------

                            drpaid["Allot"] = Convert.ToString(allotmentAmount);
                            drpaid["Concession"] = Convert.ToString(ConcessionAmount);
                            //-------------------------------------------------------------
                            drpaid["Total"] = Convert.ToString(demandAmt);
                            drpaid["Receipt"] = Convert.ToString(paidAmt);
                            drpaid["credit"] = Convert.ToString(paidAmtCr);
                            balAmt = demandAmt - paidAmt;
                            drpaid["Balance"] = Convert.ToString(balAmt);
                            dtpaid.Rows.Add(drpaid);

                            if (!httotal.ContainsKey("Student"))
                                httotal.Add("Student", studCnt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                                amount += studCnt;
                                httotal.Remove("Student");
                                httotal.Add("Student", Convert.ToString(amount));
                            }

                            //----------------------added by abarna 20.01.2018-------------
                            if (!httotal.ContainsKey("Allot"))
                                httotal.Add("Allot", allotmentAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                                amount += allotmentAmount;
                                httotal.Remove("Allot");
                                httotal.Add("Allot", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Concession"))
                                httotal.Add("Concession", ConcessionAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                                amount += ConcessionAmount;
                                httotal.Remove("Concession");
                                httotal.Add("Concession", Convert.ToString(amount));
                            }

                            //-------------------------------------
                            if (!httotal.ContainsKey("Total"))
                                httotal.Add("Total", demandAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                                amount += demandAmt;
                                httotal.Remove("Total");
                                httotal.Add("Total", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Receipt"))
                                httotal.Add("Receipt", paidAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                                amount += paidAmt;
                                httotal.Remove("Receipt");
                                httotal.Add("Receipt", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Balance"))
                                httotal.Add("Balance", balAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                                amount += balAmt;
                                httotal.Remove("Balance");
                                httotal.Add("Balance", Convert.ToString(amount));
                            }

                        }

                    }
                }
                #region collegewise
                if (rblhostelmode.SelectedIndex == 2)
                {
                    drpaid = dtpaid.NewRow();


                    DataTable dthostel = ds.Tables[0].DefaultView.ToTable(true, "HostelMasterFK", "HostelName", "college_code", "Coll_acronymn");
                    for (int i = 0; i < dthostel.Rows.Count; i++)
                    {
                        drpaid = dtpaid.NewRow();
                        double demandAmt = 0;
                        double paidAmt = 0;
                        double paidAmtCr = 0;
                        double balAmt = 0;
                        double studCnt = 0;
                        double concessionamt = 0;
                        double allotamt = 0;
                        double tempAllot = 0;
                        double tempPaidDr = 0;
                        double tempPaidCr = 0;
                        double allotmentAmount = 0;
                        double ConcessionAmount = 0;
                        string hostelname = string.Empty;
                        string collegename = string.Empty;
                        string hostelcode = returnwithsinglecodevalue(cbl_hostelname);
                        string collegecode = returnwithsinglecodevalue(cblclg);
                        drpaid["Sno"] = Convert.ToString(++rowCnt);

                        // drpaid["HostelMasterPK"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]);

                        //drpaid["Hostelname"] = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]);

                        hostelname = Convert.ToString(dthostel.Rows[i]["HostelName"]);
                        drpaid["HostelName"] = Convert.ToString(hostelname);
                        collegename = Convert.ToString(dthostel.Rows[i]["Coll_acronymn"]);
                        drpaid["College"] = Convert.ToString(collegename);
                        if (!httotal.ContainsKey("HostelName"))
                            httotal.Add("HostelName", hostelname);
                        hostelcode = Convert.ToString(dthostel.Rows[i]["HostelMasterFK"]);
                        collegecode = Convert.ToString(dthostel.Rows[i]["college_code"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(HostelCount)", "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "')  and College_code in('" + Convert.ToString(collegecode) + "')")), out studCnt);
                        drpaid["Student"] = Convert.ToString(studCnt);
                        string clgCode = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]);

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            boolClg = false;
                            ds.Tables[3].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_code='" + collegecode + "'";//HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and 
                            DataTable dtbatch = ds.Tables[3].DefaultView.ToTable();
                            if (dtbatch.Rows.Count > 0)
                            {
                                for (int bat = 0; bat < dtbatch.Rows.Count; bat++)
                                {
                                    string batch = Convert.ToString(dtbatch.Rows[bat]["batch_year"]);
                                    string curSem = string.Empty;
                                    string curSemCode = string.Empty;
                                    if (!cbAcdYear.Checked)
                                    {
                                        if (currentSem.ContainsKey(batch))
                                            curSem = Convert.ToString(currentSem[batch]);
                                        Dictionary<string, string> getFeeCode = getFeecode(collegecode);//get current sem code 
                                        curSem = getCurYear(curSem);
                                        if (getFeeCode.ContainsKey(curSem))
                                            curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                                    }
                                    else
                                    {
                                        if (getAcdYear.ContainsKey(collegecode + "$" + batch))
                                        {
                                            curSemCode = Convert.ToString(getAcdYear[collegecode + "$" + batch]);
                                            curSemCode = curSemCode.Replace(",", "','");
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(curSemCode))
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_Code='" + collegecode + "' and  batch_year='" + batch + "'  ";// feecategory in('" + curSemCode + "') and and feecategory in('" + curSemCode + "') and batch_year='" + batch + "'  
                                            DataTable dvpaid = ds.Tables[1].DefaultView.ToTable();


                                            if (dvpaid.Rows.Count > 0)
                                            {
                                                //------------Added by abarna------------
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(Allot)", "")), out allotamt);
                                                allotmentAmount += allotamt;

                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(Concession)", "")), out concessionamt);
                                                ConcessionAmount += concessionamt;
                                                //double.TryParse(Convert.ToString(dvpaid.Compute("sum(paid)", "")), out paidAmt);
                                                //double.TryParse(Convert.ToString(dvpaid.Compute("sum(bal)", "")), out balAmt);
                                                //----------------------------------------------
                                                //  double.TryParse(Convert.ToString(dvpaid[0]["Concession"]), out concessionAmt);
                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(total)", "")), out tempAllot);
                                                //double.TryParse(Convert.ToString(dvpaid[0]["total"]), out tempAllot);
                                                demandAmt += tempAllot;


                                            }
                                        }
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') and college_Code='" + collegecode + "' and  batch_year='" + batch + "'";//feecategory in('" + curSemCode + "') and
                                            DataTable dvpaid = ds.Tables[2].DefaultView.ToTable();
                                            if (dvpaid.Rows.Count > 0)
                                            {

                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(debit)", "")), out tempPaidDr);
                                                double.TryParse(Convert.ToString(dvpaid.Compute("Sum(credit)", "")), out tempPaidCr);
                                                //double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out tempPaidDr);
                                                //double.TryParse(Convert.ToString(dvpaid[0]["credit"]), out tempPaidCr);
                                                paidAmt += tempPaidDr;
                                                paidAmtCr += tempPaidCr;
                                            }
                                        }
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[i]["college_code"] + "' and batch_year='" + batch + "' and HostelMasterFK in ('" + Convert.ToString(hostelcode) + "') ";//and feecategory in('" + curSemCode + "')
                                            DataTable dvpaids = ds.Tables[4].DefaultView.ToTable();

                                        }
                                        boolClg = true;
                                    }
                                }
                            }
                        }

                        if (boolClg)
                        {


                            //----------------------added by abarna 20.01.2018-------------
                            drpaid["Allot"] = Convert.ToString(allotmentAmount);
                            drpaid["Concession"] = Convert.ToString(ConcessionAmount);
                            //-------------------------------------------------------------
                            drpaid["Total"] = Convert.ToString(demandAmt);
                            drpaid["Receipt"] = Convert.ToString(paidAmt);
                            drpaid["credit"] = Convert.ToString(paidAmtCr);
                            balAmt = demandAmt - paidAmt;
                            drpaid["Balance"] = Convert.ToString(balAmt);
                            dtpaid.Rows.Add(drpaid);

                            if (!httotal.ContainsKey("Student"))
                                httotal.Add("Student", studCnt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Student"]), out amount);
                                amount += studCnt;
                                httotal.Remove("Student");
                                httotal.Add("Student", Convert.ToString(amount));
                            }

                            //----------------------added by abarna 20.01.2018-------------
                            if (!httotal.ContainsKey("Allot"))
                                httotal.Add("Allot", allotmentAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Allot"]), out amount);
                                amount += allotmentAmount;
                                httotal.Remove("Allot");
                                httotal.Add("Allot", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Concession"))
                                httotal.Add("Concession", ConcessionAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Concession"]), out amount);
                                amount += ConcessionAmount;
                                httotal.Remove("Concession");
                                httotal.Add("Concession", Convert.ToString(amount));
                            }

                            //-------------------------------------
                            if (!httotal.ContainsKey("Total"))
                                httotal.Add("Total", demandAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Total"]), out amount);
                                amount += demandAmt;
                                httotal.Remove("Total");
                                httotal.Add("Total", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Receipt"))
                                httotal.Add("Receipt", paidAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Receipt"]), out amount);
                                amount += paidAmt;
                                httotal.Remove("Receipt");
                                httotal.Add("Receipt", Convert.ToString(amount));
                            }
                            if (!httotal.ContainsKey("Balance"))
                                httotal.Add("Balance", balAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal["Balance"]), out amount);
                                amount += balAmt;
                                httotal.Remove("Balance");
                                httotal.Add("Balance", Convert.ToString(amount));
                            }
                        }

                    }

                }
                #endregion
                if (httotal.Count > 0)
                {
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Total" + "-" + "*";
                    foreach (DictionaryEntry dtrow in httotal)
                    {
                        drpaid[Convert.ToString(dtrow.Key)] = Convert.ToString(dtrow.Value);
                    }
                    dtpaid.Rows.Add(drpaid);
                }
            }
        }

        catch { }
        return dtpaid;
    }

    protected void loadSpreadDetails(DataTable dtpaid)
    {
        try
        {
            #region design

            DataTable dtInstWiseBalReport = new DataTable();
            DataRow drowInst = null;
            ArrayList arrColHdrNames = new ArrayList();

            if (rblhostelmode.SelectedIndex != 2)
            {
                arrColHdrNames.Add("S.No");
                dtInstWiseBalReport.Columns.Add("col0");
                if (rblhostelmode.SelectedIndex == 1)//added by abarna 27.01.2018
                {
                    arrColHdrNames.Add("HostelName");
                    dtInstWiseBalReport.Columns.Add("col1");
                    arrColHdrNames.Add("HostelMasterPK");
                    dtInstWiseBalReport.Columns.Add("col2");
                }

                if (rblhostelmode.SelectedIndex == 0 || rblMode.SelectedIndex == 0 || rblMode.SelectedIndex == 2)////added by abarna 27.01.2018
                {
                    arrColHdrNames.Add(lblclg.Text);
                    dtInstWiseBalReport.Columns.Add("col1");
                    arrColHdrNames.Add("CollegeCode");
                    dtInstWiseBalReport.Columns.Add("col2");
                }
                arrColHdrNames.Add("Student");
                dtInstWiseBalReport.Columns.Add("col3");
                arrColHdrNames.Add("Allot");
                dtInstWiseBalReport.Columns.Add("col4");
                arrColHdrNames.Add("Concession");
                dtInstWiseBalReport.Columns.Add("col5");
                arrColHdrNames.Add("Total");
                dtInstWiseBalReport.Columns.Add("col6");
                arrColHdrNames.Add("Receipt");
                dtInstWiseBalReport.Columns.Add("col7");
                arrColHdrNames.Add("Credit");
                dtInstWiseBalReport.Columns.Add("col8");
                arrColHdrNames.Add("Balance");
                dtInstWiseBalReport.Columns.Add("col9");
            }
            if (rblhostelmode.SelectedIndex == 2)
            {
                arrColHdrNames.Add("S.No");
                dtInstWiseBalReport.Columns.Add("col0");
                arrColHdrNames.Add("HostelName");
                dtInstWiseBalReport.Columns.Add("col1");
                arrColHdrNames.Add("HostelMasterPK");
                dtInstWiseBalReport.Columns.Add("col2");
                arrColHdrNames.Add(lblclg.Text);
                dtInstWiseBalReport.Columns.Add("col3");
                arrColHdrNames.Add("CollegeCode");
                dtInstWiseBalReport.Columns.Add("col4");
                arrColHdrNames.Add("Student");
                dtInstWiseBalReport.Columns.Add("col5");
                arrColHdrNames.Add("Allot");
                dtInstWiseBalReport.Columns.Add("col6");
                arrColHdrNames.Add("Concession");
                dtInstWiseBalReport.Columns.Add("col7");
                arrColHdrNames.Add("Total");
                dtInstWiseBalReport.Columns.Add("col8");
                arrColHdrNames.Add("Receipt");
                dtInstWiseBalReport.Columns.Add("col9");
                arrColHdrNames.Add("Credit");
                dtInstWiseBalReport.Columns.Add("col10");
                arrColHdrNames.Add("Balance");
                dtInstWiseBalReport.Columns.Add("col11");
            }

            DataRow drHdr1 = dtInstWiseBalReport.NewRow();
            for (int grCol = 0; grCol < dtInstWiseBalReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtInstWiseBalReport.Rows.Add(drHdr1);

            #endregion

            #region value

            int height = 0;
            int rowCnt = 0;
            for (int row = 0; row < dtpaid.Rows.Count; row++)
            {
                height += 30;
                string payModeText = Convert.ToString(dtpaid.Rows[row]["Sno"]);

                if (!payModeText.Trim().Contains("*"))
                {
                    if (rblhostelmode.SelectedIndex != 2)
                    {
                        drowInst = dtInstWiseBalReport.NewRow();
                        drowInst[0] = ++rowCnt;
                        if (rblhostelmode.SelectedIndex == 1)//added by abarna 27.01.2018
                        {
                            drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Hostelname"]);
                            drowInst[2] = Convert.ToString(dtpaid.Rows[row]["HostelMasterPK"]);
                        }
                        if (rblhostelmode.SelectedIndex == 0 || rblMode.SelectedIndex == 0 || rblMode.SelectedIndex == 2)//added by abarna 27.01.2018
                        {
                            drowInst[1] = Convert.ToString(dtpaid.Rows[row]["College"]);
                            drowInst[2] = Convert.ToString(dtpaid.Rows[row]["CollegeCode"]);
                        }
                        drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Student"]);
                        drowInst[4] = Convert.ToString(dtpaid.Rows[row]["Allot"]);
                        drowInst[5] = Convert.ToString(dtpaid.Rows[row]["Concession"]);
                        drowInst[6] = Convert.ToString(dtpaid.Rows[row]["Total"]);
                        drowInst[7] = Convert.ToString(dtpaid.Rows[row]["Receipt"]);
                        drowInst[8] = Convert.ToString(dtpaid.Rows[row]["credit"]);
                        drowInst[9] = Convert.ToString(dtpaid.Rows[row]["Balance"]);
                    }
                    if (rblhostelmode.SelectedIndex == 2)
                    {
                        drowInst = dtInstWiseBalReport.NewRow();
                        drowInst[0] = ++rowCnt;
                        drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Hostelname"]);
                        drowInst[2] = Convert.ToString(dtpaid.Rows[row]["HostelMasterPK"]);
                        drowInst[3] = Convert.ToString(dtpaid.Rows[row]["College"]);
                        drowInst[4] = Convert.ToString(dtpaid.Rows[row]["CollegeCode"]);
                        drowInst[5] = Convert.ToString(dtpaid.Rows[row]["Student"]);
                        drowInst[6] = Convert.ToString(dtpaid.Rows[row]["Allot"]);
                        drowInst[7] = Convert.ToString(dtpaid.Rows[row]["Concession"]);
                        drowInst[8] = Convert.ToString(dtpaid.Rows[row]["Total"]);
                        drowInst[9] = Convert.ToString(dtpaid.Rows[row]["Receipt"]);
                        drowInst[10] = Convert.ToString(dtpaid.Rows[row]["credit"]);
                        drowInst[11] = Convert.ToString(dtpaid.Rows[row]["Balance"]);
                    }
                }

                else
                {
                    if (rblhostelmode.SelectedIndex != 2)
                    {
                        drowInst = dtInstWiseBalReport.NewRow();
                        drowInst[0] = payModeText.Split('*')[0].TrimEnd('-');

                        drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Student"]);
                        drowInst[4] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Allot"]));
                        drowInst[5] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Concession"]));
                        drowInst[6] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Total"]));
                        drowInst[7] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Receipt"]));
                        drowInst[8] = "0";
                        drowInst[9] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Balance"]));
                    }
                    if (rblhostelmode.SelectedIndex == 2)
                    {
                        drowInst = dtInstWiseBalReport.NewRow();
                        drowInst[0] = payModeText.Split('*')[0].TrimEnd('-');
                        drowInst[5] = Convert.ToString(dtpaid.Rows[row]["Student"]);
                        drowInst[6] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Allot"]));
                        drowInst[7] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Concession"]));
                        drowInst[8] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Total"]));
                        drowInst[9] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Receipt"]));
                        drowInst[10] = "0";
                        drowInst[11] = d2.numberformat(Convert.ToString(dtpaid.Rows[row]["Balance"]));
                    }

                }
                dtInstWiseBalReport.Rows.Add(drowInst);
            }
            if (rblhostelmode.SelectedIndex == 2)
            {
                //spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            GrdRowCount = dtInstWiseBalReport.Rows.Count - 1;
            grdInstWiseBalReport.DataSource = dtInstWiseBalReport;
            grdInstWiseBalReport.DataBind();
            grdInstWiseBalReport.Visible = true;

            foreach (GridViewRow gvrow in grdInstWiseBalReport.Rows)
            {
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                string sNoVal = Convert.ToString(grdInstWiseBalReport.Rows[RowCnt].Cells[0].Text);
                if (Convert.ToString(sNoVal).All(char.IsNumber))
                {
                }
                else
                {
                    if (sNoVal == "Total")
                    {
                        grdInstWiseBalReport.Rows[RowCnt].BackColor = Color.Green;
                        grdInstWiseBalReport.Rows[RowCnt].Font.Bold = true;
                        grdInstWiseBalReport.Rows[RowCnt].Font.Size = 13;
                    }
                }
            }
            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;

            #endregion
        }
        catch { }
    }

    protected void grdInstWiseBalReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Width = 200;
                e.Row.Font.Bold = true;
            }

            if (rblhostelmode.SelectedIndex != 2)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].BorderColor = Color.Black;
                e.Row.Cells[7].Font.Underline = true;
                e.Row.Cells[7].ForeColor = Color.Blue;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            if (rblhostelmode.SelectedIndex == 2)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[9].BorderColor = Color.Black;
                e.Row.Cells[9].Font.Underline = true;
                e.Row.Cells[9].ForeColor = Color.Blue;
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[10].Visible = false;
            }

        }
    }

    //Grid cell click get student details  

    protected void grdInstWiseBalReport_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        int rowCnt = grdInstWiseBalReport.Rows.Count;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex != 0 && e.Row.RowIndex != GrdRowCount)
            {
                if (rblhostelmode.SelectedIndex != 2)
                {
                    //for (int i = 1; i < e.Row.Cells.Count; i++)
                    //{
                    TableCell cell = e.Row.Cells[7];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 7
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                    //  }
                }
                if (rblhostelmode.SelectedIndex == 2)
                {
                    //for (int i = 1; i < e.Row.Cells.Count; i++)
                    //{
                    TableCell cell = e.Row.Cells[9];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 9
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                    // }
                }
            }
            else
            {
                
            }
        }
    }

    protected void grdInstWiseBalReport_SelectedIndexChanged(Object sender, EventArgs e)
    {
        string collegecode = string.Empty;
        RollAndRegSettings();
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        DataSet dsval = new DataSet();

        #region get Value

        string rights = "";
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
        {
            rights = "and group_code='" + group_user + "'";
        }
        else
        {
            rights = " and user_code='" + usercode + "'";
        }
        UserbasedRights();
        string hdText = string.Empty;
        string payMode = string.Empty;
        string batch = string.Empty;
        string degreeCode = string.Empty;
        string sem = string.Empty;
        string sec = string.Empty;
        string finyear = string.Empty;
        string studMode = string.Empty;
        if (cblclg.Items.Count > 0)
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
        hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
        string SelQuery = "";
        if (!string.IsNullOrEmpty(hdText))
        {
            SelQuery = "select distinct headerpk as code from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdText + "')";
            hdText = getFilterValues(SelQuery);
        }
        payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
        batch = Convert.ToString(getCblSelectedText(cblbatch));
        degreeCode = Convert.ToString(getCblSelectedValue(cbl_branch));
        if (!string.IsNullOrEmpty(degreeCode))
        {
            SelQuery = "     select distinct dt.dept_code as code from degree d,department dt,course c ,deptprivilages dp where c.course_id=d.course_id and dt.dept_code=d.dept_code and c.college_code = d.college_code and dt.college_code = d.college_code and dp.Degree_code=d.Degree_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "') " + rights + "";
            // SelQuery = " select distinct d.degree_code as code from degree d,course c,department dt,deptprivilages dp where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=dp.degree_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "') " + rights + "";
            degreeCode = getFilterValues(SelQuery);
        }
        sem = Convert.ToString(getCblSelectedValue(cbl_sem));
        sec = Convert.ToString(getCblSelectedValue(cbl_sec));
        //finyear = Convert.ToString(ddlfinyear.SelectedItem.Value);
        //if (!string.IsNullOrEmpty(finyear))
        //{
        //    string frDate = finyear.Split('-')[0];
        //    string toDate = finyear.Split('-')[1];
        //    SelQuery = " select distinct finyearpk as code from FM_FinYearMaster where CollegeCode in('" + collegecode + "') and FinYearStart='" + frDate.Split('/')[1] + "/" + frDate.Split('/')[0] + "/" + frDate.Split('/')[2] + "' and FinYearEnd='" + toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2] + "'  ";
        //    finyear = getFilterValues(SelQuery);
        //}
        finyear = Convert.ToString(getCblSelectedValue(chklsfyear));
        studMode = Convert.ToString(getCblSelectedValue(cbl_type));
        string fromdate = txt_fromdate.Text;
        string todate = txt_todate.Text;
        string[] frdate = fromdate.Split('/');
        if (frdate.Length == 3)
            fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
        string[] tdate = todate.Split('/');
        if (tdate.Length == 3)
            todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
        //  string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
        string strInclude = getStudCategory();
        StringBuilder sbFinlYr = new StringBuilder();
        Dictionary<string, string> htFinlYR = getFinancialYear();
        if (chklsfyear.Items.Count > 0)
        {
            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
            {
                if (!chklsfyear.Items[fnl].Selected)
                    continue;
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (!cblclg.Items[clg].Selected)
                        continue;
                    string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + cblclg.Items[clg].Value);//to pass value get key from dictionary 
                    sbFinlYr.Append(KeyVal + "','");
                }

            }
            if (sbFinlYr.Length > 0)
                sbFinlYr.Remove(sbFinlYr.Length - 3, 3);

        }
        string studType = string.Empty;
        if (rblMode.SelectedIndex == 0)
            studType = "";
        else if (rblMode.SelectedIndex == 1)
            studType = " and stud_type in('Hostler')";
        else
            studType = " and isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>''";
        #endregion

        #region Query

        string collgCode = "";
        string debit = "";
        string credit = "";
        if (rblhostelmode.SelectedIndex != 2)
        {
            collgCode = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[2].Text);
            debit = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[7].Text);
            credit = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[8].Text);
        }
        if (rblhostelmode.SelectedIndex == 2)
        {
            collgCode = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[4].Text);
            debit = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[9].Text);
            credit = Convert.ToString(grdInstWiseBalReport.Rows[rowIndex].Cells[10].Text);
        }
        if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
        {
            //collecode = collgCode;
            string selCol = "f.Transcode,convert(varchar(10),transdate,103) as Transdate, (select headername from fm_headermaster h where h.headerpk=f.headerfk and r.college_code=h.collegecode) as headername ,roll_no,reg_no,roll_admit,stud_name,sum(debit) as credit,sum(credit) as debit,batch_year,(select (c.course_name+'-'+dt.dept_acronym) from course c,degree d,department dt where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code and d.college_code in('" + collgCode + "')) as deptName ";
            string GrpselCol = " f.Transcode,transdate, headerfk ,roll_no,reg_no,roll_admit,stud_name,credit,debit,batch_year,r.college_code,r.degree_code";
            string SelQ = string.Empty;
            SelQ = " select distinct ''sno," + selCol + " from registration r,ft_findailytransaction f where f.app_no=r.app_no and r.college_code in('" + collgCode + "') and f.headerfk in('" + hdText + "') and f.paymode in('" + payMode + "')   and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + Convert.ToString(sbFinlYr) + "') and isnull(paid_Istransfer,'0')='0' ";
            if (cbdate.Checked)
                SelQ += " and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            if (!string.IsNullOrEmpty(studMode))
                SelQ += " and r.mode in('" + studMode + "')";
            SelQ += "" + strInclude + " " + studType + " and isnull(iscanceled,'0')='0' and transcode<>'' and isnull(debit,'0')>'0' group by " + GrpselCol + "  order by f.Transcode";
            dsval.Clear();
            dsval = d2.select_method_wo_parameter(SelQ, "Text");
            spcredit.InnerHtml = debit;
            spdebit.InnerHtml = credit;
            //double credit = 0;
            //double debit = 0;
            //double.TryParse(Convert.ToString(e.Row.Cells[8].Text), out credit);
            //double.TryParse(Convert.ToString(e.Row.Cells[9].Text), out debit);
        }

        #endregion

        //DataSet dsval = bindStudentDetails(ref collegecode);
        if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
        {
            getStudentDetails(dsval, collegecode);
        }
        else
        {
            popstud.Attributes.Add("Style", "display:none;");
            // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }

    protected void getStudentDetails(DataSet ds, string collegecode)
    {
        try
        {
            Hashtable htRealName = htcolumnHeaderValue();
            for (int row = 0; row < ds.Tables[0].Columns.Count; row++)
            {
                string oldName = Convert.ToString(ds.Tables[0].Columns[row].ColumnName);
                string viewcolName = Convert.ToString(htRealName[oldName.Trim()]);
                ds.Tables[0].Columns[row].ColumnName = viewcolName;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridstud.DataSource = ds;
                gridstud.DataBind();
                gridstud.Visible = true;
                pnlContents.Visible = true;
                printCollegeDet(collegecode);
                popstud.Attributes.Add("Style", "height: 100%; z-index: 1000;width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px; display:block;");
                Session["grid"] = ds;
                //pnlContents.Visible = true;
                //buttonHide();
                //printCollegeDet();
                // gridAlignment();
            }
        }
        catch { }
    }

    //==========================//

    protected string getCurrentSemester(string batch, string collegecode, string strRen)
    {
        string curSem = string.Empty;
        StringBuilder sbCurSem = new StringBuilder();
        string selQ = "   select distinct current_semester from registration r where r.batch_year in('" + batch + "') and r.college_code='" + collegecode + "' " + strRen + "";
        DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
        if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
        {
            for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
            {
                sbCurSem.Append(Convert.ToString(dsval.Tables[0].Rows[row]["current_semester"]) + "','");
            }
            if (sbCurSem.Length > 0)
            {
                sbCurSem.Remove(sbCurSem.Length - 3, 3);
                curSem = Convert.ToString(sbCurSem);
            }
        }

        return curSem;
    }

    protected Dictionary<string, string> getFeecode(string collegecode)
    {
        Dictionary<string, string> htsem = new Dictionary<string, string>();
        try
        {
            string featDegcode = string.Empty;
            //Convert.ToString(getCblSelectedValue(cbl_dept));
            string type = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            DataSet dsval = d2.loadFeecategory(Convert.ToString(collegecode), usercode, ref type);
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                string feecatg = "";
                string cblvalue = "";
                //string selQ = " select textval,textcode from textvaltable where textcriteria='FEECA' and  college_code='" + collegecode + "'";
                //DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    for (int sem = 0; sem < dsval.Tables[0].Rows.Count; sem++)
                    {
                        cblvalue = Convert.ToString(dsval.Tables[0].Rows[sem]["textcode"]);
                        cbltext = Convert.ToString(dsval.Tables[0].Rows[sem]["textval"]);
                        #region
                        if (type == "Semester")
                        {
                            #region semester
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[0] == "1" || feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                            else if (feesem[0] == "3" || feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("2"))
                                    htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2");
                                    htsem.Add(Convert.ToString("2"), feecatg);
                                }
                            }
                            else if (feesem[0] == "5" || feesem[0] == "6")
                            {
                                if (!htsem.ContainsKey("3"))
                                    htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3");
                                    htsem.Add(Convert.ToString("3"), feecatg);
                                }
                            }
                            else if (feesem[0] == "7" || feesem[0] == "8")
                            {
                                if (!htsem.ContainsKey("4"))
                                    htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4");
                                    htsem.Add(Convert.ToString("4"), feecatg);
                                }
                            }
                            #endregion
                        }
                        else if (type == "Term")
                        {
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[1] == "1" || feesem[1] == "2" || feesem[1] == "3" || feesem[1] == "4")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                        }
                        else if (type == "Year")
                        {
                            #region year
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[0] == "1")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                            if (feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("2"))
                                    htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2");
                                    htsem.Add(Convert.ToString("2"), feecatg);
                                }
                            }
                            if (feesem[0] == "3")
                            {
                                if (!htsem.ContainsKey("3"))
                                    htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3");
                                    htsem.Add(Convert.ToString("3"), feecatg);
                                }
                            }
                            if (feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("4"))
                                    htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4");
                                    htsem.Add(Convert.ToString("4"), feecatg);
                                }
                            }
                            #endregion
                        }
                        #endregion

                    }
                }
            }
            ViewState["feecat"] = htsem;
        }
        catch { }
        return htsem;
    }

    protected string getCurYear(string curSem)
    {
        string curYear = string.Empty;
        try
        {
            switch (curSem)
            {
                case "1":
                case "2":
                case "1,2":
                    curYear = "1";
                    break;
                case "3":
                case "4":
                case "3,4":
                    curYear = "2";
                    break;
                case "5":
                case "6":
                case "5,6":
                    curYear = "3";
                    break;
                case "7":
                case "8":
                case "7,8":
                    curYear = "4";
                    break;
                case "9":
                case "10":
                case "9,10":
                    curYear = "5";
                    break;
            }
        }
        catch { }
        return curYear;
    }

    protected string getHeaderFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct headerpk from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["headerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        bool boolCheck = false;

        if (!cbCurSem.Checked && !cbAcdYear.Checked)
        {
            ds = loadDetails();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtpaid = loadDetails(ds);
                if (dtpaid.Rows.Count > 0)
                {
                    loadSpreadDetails(dtpaid);
                }
            }
            else
                boolCheck = true;
        }
        else
        {
            ds = loadDetailsCurSem();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtpaid = loadDetailsCurSem(ds);
                if (dtpaid.Rows.Count > 0)
                {
                    loadSpreadDetails(dtpaid);
                }
            }
            else
                boolCheck = true;
        }
        //else
        //{
        //    txtexcelname.Text = string.Empty;
        //    grdInstWiseBalReport.Visible = false;
        //    print.Visible = false;
        //    divlabl.Visible = false;
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        //}

        if (boolCheck)
        {
            //lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            grdInstWiseBalReport.Visible = false;
            print.Visible = false;
            divlabl.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            //lbl_alert.Text = "No Record Found";
            //imgdiv2.Visible = true;
        }

    }

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdInstWiseBalReport, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            string ledgerAcr = getledgerAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            txtexcelname.Text = "";
            string AcademicYear = string.Empty;
            if (cbAcdYear.Checked)
                AcademicYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Institutionwise Balance Report\n" + clgAcr + '@' + "Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "Ledger : " + ledgerAcr + '@' + "Academic Year : " + AcademicYear;
            pagename = "FinanceBalDet.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(grdInstWiseBalReport, pagename, degreedetails, 0, ss);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            //lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            string ledgerAcr = getledgerAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            string AcademicYear = string.Empty;
            if (cbAcdYear.Checked)
                AcademicYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Institutionwise Balance Report\n" + clgAcr + '@' + "Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "Ledger : " + ledgerAcr + '@' + "Academic Year : " + AcademicYear;
            pagename = "FinanceBalDet.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(grdInstWiseBalReport, pagename, degreedetails, 0, ss);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected string getclgAcr(string collegecode)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select collname,college_code,acr,Coll_acronymn from collinfo where college_code in('" + collegecode + "')";//modified by abarna
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["Coll_acronymn"]) + ",");//change by abarna 11.04.2018
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }

    protected string getledgerAcr(string collegecode)
    {
        string strAcr = string.Empty;
        try
        {
            string ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            StringBuilder clgAcr = new StringBuilder();
            string selQ = "        select distinct ledgeracr as acr from fm_ledgermaster where collegecode in('" + collegecode + "') and isnull(ledgeracr,'')<>'' and ledgername in('" + ldText + "')";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]) + ",");
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }

    #endregion

    #region print settings

    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }

    #endregion

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

    private string getCblSelectedTempText(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
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

    #region Include setting

    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                txtinclude.Enabled = true;
                LoadIncludeSetting();
            }
            else
            {
                txtinclude.Enabled = false;
                cblinclude.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Cancel", "4"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Prolong Absent", "5"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Apply", "6"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Issued", "7"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Readmission", "8"));
            //if (cblinclude.Items.Count > 0)
            //{
            //    for (int i = 0; i < cblinclude.Items.Count; i++)
            //    {
            //        cblinclude.Items[i].Selected = true;
            //    }
            //    cbinclude.Checked = true;
            //    txtinclude.Text = "Student(" + cblinclude.Items.Count + ")";
            //}
        }
        catch { }
    }

    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");
    }

    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");

    }

    #endregion

    //type 

    protected void loadType()
    {
        try
        {
            cbl_type.Items.Clear();
            if (checkSchoolSetting() == 0)
            {
                cbl_type.Items.Add(new ListItem("Old Studnent", "1"));
                cbl_type.Items.Add(new ListItem("New Student", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
        }
        catch { }
    }

    protected void cb_type_CheckedChanged(object sender, EventArgs e)
    {
        string type = "";
        if (cb_type.Checked == true)
        {
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                cbl_type.Items[i].Selected = true;
                type = Convert.ToString(cbl_type.Items[i].Text);
            }
            if (cbl_type.Items.Count == 1)
            {
                txt_type.Text = "" + type + "";
            }
            else
            {
                txt_type.Text = "Type(" + (cbl_type.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                cbl_type.Items[i].Selected = false;
            }
            txt_type.Text = "--Select--";
        }

    }

    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_type.Text = "--Select--";
        string type = "";
        cb_type.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_type.Items.Count; i++)
        {
            if (cbl_type.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                type = Convert.ToString(cbl_type.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_type.Items.Count)
            {
                cb_type.Checked = true;
            }
            if (commcount == 1)
            {
                txt_type.Text = "" + type + "";
            }
            else
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
            }
        }

    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    protected void printCollegeDet(string collegecode)
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3,com_name from collinfo where college_code=" + collegecode + " ";
            string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
            academicyear = academicyear.Trim().Trim(',').Replace(",", "-");
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string collegename = "";
            string add1 = "";
            string add2 = "";
            string add3 = "";
            string univ = "";
            string feedet = "";
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                // spDateSession.InnerText = "PRE-PRIMARY COMPARTMENT";
                sprptnamedt.InnerText = "STUDENTS PAID DETAILS--" + academicyear + "";
                spdate.InnerText = fromdate + "-" + todate;
                //DateTime.Now.ToString("dd.MM.yyyy");
                //spdate.InnerText = "STUDENTS ATTENDANCE CONSOLIDATION--" + academicyear + "";
            }
        }
        catch { }
    }

    protected Hashtable htcolumnHeaderValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("sno", "SNo");
            htcol.Add("Transcode", "Receipt No");
            htcol.Add("Transdate", "Receipt date");
            htcol.Add("headername", "Header");
            htcol.Add("roll_no", "Roll No");
            htcol.Add("reg_no", "Reg No");
            htcol.Add("roll_admit", "Admission No");
            htcol.Add("stud_name", "Student Name");
            htcol.Add("credit", "Credit");
            htcol.Add("debit", "Debit");
            htcol.Add("batch_year", "Batch Year");
            htcol.Add("deptName", "Department");
        }
        catch { }
        return htcol;
    }

    protected void gridstud_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            #region
            if (roll == 0)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 1)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 2)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;

            }
            else if (roll == 3)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 4)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 5)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 6)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 7)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = "" + ((((GridView)sender).PageIndex * ((GridView)sender).PageSize) + (e.Row.RowIndex + 1));

            #region
            if (roll == 0)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 1)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 2)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;

            }
            else if (roll == 3)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 4)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 5)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 6)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 7)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            #endregion
        }
    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
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

    #endregion

    //discontinue,delflag
    //protected string getStudCategory()
    //{
    //    string strInclude = string.Empty;
    //    try
    //    {
    //        #region includem

    //        string cc = "";
    //        string debar = "";
    //        string disc = "";
    //        string cancel = "";
    //        if (cblinclude.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cblinclude.Items.Count; i++)
    //            {
    //                if (cblinclude.Items[i].Selected == true)
    //                {
    //                    if (cblinclude.Items[i].Value == "1")
    //                        cc = " r.cc=1";
    //                    if (cblinclude.Items[i].Value == "2")
    //                        debar = " r.Exam_Flag like '%debar'";
    //                    if (cblinclude.Items[i].Value == "3")
    //                        disc = "  r.DelFlag=1";
    //                    if (cblinclude.Items[i].Value == "4")
    //                        cancel = "  r.DelFlag=2";
    //                }
    //            }
    //        }
    //        if (!checkdicon.Checked)
    //        {
    //            if (cc != "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
    //            if (cc == "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
    //            if (cc == "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
    //            if (cc == "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
    //            //2
    //            if (cc != "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
    //            if (cc != "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
    //            if (cc != "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
    //            //
    //            if (cc == "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
    //            if (cc == "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
    //            //
    //            if (cc == "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            //3
    //            if (cc != "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
    //            if (cc != "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            if (cc != "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
    //            if (cc == "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            if (cc == "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
    //            if (cc != "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = "";
    //        }
    //        else
    //        {
    //            if (cc != "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and " + cc + "";
    //            if (cc == "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and " + debar + "";
    //            if (cc == "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and " + disc + "";
    //            if (cc == "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and " + cancel + "";
    //            //2
    //            if (cc != "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and( " + cc + " or " + debar + ")";
    //            if (cc != "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or " + disc + ")";
    //            if (cc != "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + cancel + ")";
    //            //
    //            if (cc == "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + debar + " or " + disc + ")";
    //            if (cc == "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + debar + " or " + cancel + ")";
    //            //
    //            if (cc == "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + disc + " or " + cancel + ")";
    //            //3
    //            if (cc != "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
    //            if (cc != "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
    //            if (cc != "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
    //            if (cc == "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
    //            if (cc == "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
    //            if (cc != "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
    //        }

    //        #endregion
    //    }
    //    catch { }
    //    return strInclude;
    //}

    //added by abarna 4.12.2017

    protected string getStudCategory()
    {
        string strInclude = string.Empty;
        try
        {
            #region includem
            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string pro = "";
            string tcapply = "";
            string tcissued = "";//abarna
            string readmit = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1  ";//and  r.ProlongAbsent=0
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0 ";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                        if (cblinclude.Items[i].Value == "5")
                            pro = " r.ProlongAbsent=1 and r.DelFlag=1";
                        if (cblinclude.Items[i].Value == "6")
                            tcapply = " isnull(r.IsTcIssued,'0')=0 and r.tcapply=1";
                        if (cblinclude.Items[i].Value == "7")
                            tcissued = "r.IsTcIssued=1";//abarna
                        if (cblinclude.Items[i].Value == "8")
                            readmit = "r.app_no in(select app_no from Readmission)";//abarna
                    }
                }
            }
            if (checkdicon.Checked)
            {
                if (cc != "")
                    strInclude = "(r.cc=1)";// and  r.ProlongAbsent=0
                if (debar != "")
                {
                    if (strInclude != "")
                    {
                        //strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        // strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                }
                if (disc != "")
                {
                    if (strInclude != "")
                    {
                        strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        strInclude += " (r.DelFlag=1 and isnull(r.ProlongAbsent,'0')=0)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += " r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0)";
                    }
                }
                if (cancel != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "  (r.DelFlag=2)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.DelFlag=2)";
                    }
                }
                if (pro != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += " (r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                }
                if (tcapply != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                }
                if (tcissued != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.IsTcIssued=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.IsTcIssued=1)";
                    }
                }
                if (readmit != "")//abarna
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.app_no in(select app_no from Readmission))";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.app_no in(select app_no from Readmission))";
                    }
                }
                if (strInclude != "")

                    strInclude = "and (" + strInclude + ")";
            }
            //if (!checkdicon.Checked)
            //{
            //    if (cc != "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            //    if (cc == "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc == "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //2
            //    if (cc != "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc != "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    //3
            //    if (cc != "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc != "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
            //    if (cc != "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = "";
            //}
            else
            {

                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";

                //if (cc != "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and " + cc + "";
                //if (cc == "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and " + debar + "";
                //if (cc == "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and " + disc + "";
                //if (cc == "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and " + cancel + "";
                ////2
                //if (cc != "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and( " + cc + " or " + debar + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + cancel + ")";
                ////
                //if (cc == "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + debar + " or " + disc + ")";
                //if (cc == "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + cancel + ")";
                ////
                //if (cc == "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + disc + " or " + cancel + ")";
                ////3
                //if (cc != "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                //if (cc != "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                //if (cc == "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                //if (cc == "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                //if (cc != "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }
            #endregion
        }
        catch { }
        return strInclude;
    }

    //added by sudhagar 31.07.2017

    protected void rblMode_Selected(object sender, EventArgs e)
    {
        try
        {
            string headerName = string.Empty;
            if (rblMode.SelectedIndex == 0)
            {
                bindheader(headerName);
            }
            else if (rblMode.SelectedIndex == 1)
            {
                rblhostelmode.Visible = true;
                //loadHostelHeader(headerName);
            }
            else
            {
                loadTransHeader(headerName);
            }
            cbloadMaster_Changed(sender, e);
        }
        catch { }
    }

    //added by abarna 26.01.2018

    protected void rblhostelmode_Selected(object sender, EventArgs e)
    {
        try
        {
            string headerName = string.Empty;
            if (rblhostelmode.SelectedIndex == 0)
            {
                loadHostelHeader(headerName);
            }
            if (rblhostelmode.SelectedIndex == 1)
            {
                bindhostel();
                loadHostelHeader(headerName);
            }
            if (rblhostelmode.SelectedIndex == 2)
            {
                bindhostel();
                loadHostelHeader(headerName);
            }
        }
        catch
        {
        }
    }

    public void loadTransHeader(string headerName)
    {
        try
        {
            StringBuilder sbName = new StringBuilder();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            Dictionary<string, string> dtHeader = getTransportSetting(collegecode);
            foreach (KeyValuePair<string, string> hdName in dtHeader)
            {
                sbName.Append(hdName.Key + "','");
            }
            if (sbName.Length > 0)
            {
                sbName.Remove(sbName.Length - 3, 3);
            }
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            if (sbName.Length > 0)
            {
                //   string headname = "select HeaderName,HeaderPK from FM_HeaderMaster where headerPK in('" + hedaderid + "')";
                string headname = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode  in('" + collegecode + "') and h.headername in('" + Convert.ToString(sbName) + "')  ";
                DataSet dshed = new DataSet();
                dshed = d2.select_method_wo_parameter(headname, "Text");
                if (dshed.Tables[0].Rows.Count > 0)
                {
                    chkl_studhed.DataSource = dshed;
                    chkl_studhed.DataValueField = "HeaderName";
                    chkl_studhed.DataTextField = "HeaderName";
                    chkl_studhed.DataBind();
                    int cnt = 0;
                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(headerName))
                        {
                            if (headerName.Contains(chkl_studhed.Items[i].Text))
                            {
                                chkl_studhed.Items[i].Selected = true;
                                cnt++;
                            }
                            else
                                chkl_studhed.Items[i].Selected = false;
                        }
                        else
                        {
                            chkl_studhed.Items[i].Selected = true;
                            cnt++;
                        }
                    }
                    txt_studhed.Text = lblheader.Text + "(" + cnt + ")";
                    if (chkl_studhed.Items.Count == cnt)
                        chk_studhed.Checked = true;
                    string ledgerName = string.Empty;
                    loadTransLedger(ledgerName);
                }
            }
        }
        catch
        {
        }
    }

    public void loadTransLedger(string ledgerName)
    {
        try
        {
            StringBuilder sbName = new StringBuilder();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            Dictionary<string, string> dtHeader = getTransportSettingLedger(collegecode);
            foreach (KeyValuePair<string, string> hdName in dtHeader)
            {
                sbName.Append(hdName.Key + "','");
            }
            if (sbName.Length > 0)
            {
                sbName.Remove(sbName.Length - 3, 3);
            }
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            if (sbName.Length > 0)
            {
                //   string headname = "select HeaderName,HeaderPK from FM_HeaderMaster where headerPK in('" + hedaderid + "')";
                // string headname = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode  in('" + collegecode + "') and h.headername in('" + Convert.ToString(sbName) + "')  ";
                string headname = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK =h.HeaderPK and   l.CollegeCode in('" + collegecode + "' ) and l.ledgername in('" + Convert.ToString(sbName) + "' )";
                DataSet dshed = new DataSet();
                dshed = d2.select_method_wo_parameter(headname, "Text");
                if (dshed.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = dshed;
                    chkl_studled.DataValueField = "ledgername";
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataBind();

                    int cnt = 0;
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ledgerName))
                        {
                            if (ledgerName.Contains(chkl_studled.Items[i].Text))
                            {
                                chkl_studled.Items[i].Selected = true;
                                cnt++;
                            }
                            else
                                chkl_studled.Items[i].Selected = false;
                        }
                        else
                        {
                            chkl_studled.Items[i].Selected = true;
                            cnt++;
                        }
                    }
                    txt_studled.Text = lbl_ledger.Text + "(" + cnt + ")";
                    if (cnt == chkl_studled.Items.Count)
                        chk_studled.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected Dictionary<string, string> getTransportSetting(string collegecode)
    {
        Dictionary<string, string> dtSetting = new Dictionary<string, string>();
        try
        {
            string selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + collegecode + "')";
            selQ += " select distinct headerpk,headername,collegecode from fm_headermaster where collegecode in('" + collegecode + "')";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]);
                    string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                    string[] leng = linkValue.Split(',');
                    if (leng.Length == 2)
                    {
                        string hdFK = Convert.ToString(leng[0]);
                        string ldFK = Convert.ToString(leng[1]);
                        if (dsVal.Tables[1].Rows.Count > 0)
                        {
                            dsVal.Tables[1].DefaultView.RowFilter = "headerpk='" + hdFK + "'";
                            DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                            if (dtHdName.Rows.Count > 0)
                            {
                                string hdName = Convert.ToString(dtHdName.Rows[0]["headername"]);
                                if (!dtSetting.ContainsKey(hdName))
                                {
                                    dtSetting.Add(hdName, clgcode);
                                }
                                // dtSetting.Add(clgcode + "$" + Convert.ToString(dtHdName.Rows[0]["headername"]), Convert.ToString(dtHdName.Rows[0]["headername"]));
                            }
                        }
                        // string headerName = d2.GetFunction("select distinct headername from fm_headermaster where headername like 'Bus Fees%'");
                    }
                }

            }
        }
        catch { }
        return dtSetting;
    }

    protected Dictionary<string, string> getTransportSettingLedger(string collegecode)
    {
        Dictionary<string, string> dtSetting = new Dictionary<string, string>();
        try
        {
            string selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + collegecode + "')";
            selQ += " select distinct ledgerpk,ledgername,collegecode from fm_ledgermaster where collegecode in('" + collegecode + "')";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]);
                    string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                    string[] leng = linkValue.Split(',');
                    if (leng.Length == 2)
                    {
                        string hdFK = Convert.ToString(leng[0]);
                        string ldFK = Convert.ToString(leng[1]);
                        if (dsVal.Tables[1].Rows.Count > 0)
                        {
                            dsVal.Tables[1].DefaultView.RowFilter = "ledgerpk='" + ldFK + "'";
                            DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                            if (dtHdName.Rows.Count > 0)
                            {
                                string hdName = Convert.ToString(dtHdName.Rows[0]["ledgername"]);
                                if (!dtSetting.ContainsKey(hdName))
                                {
                                    dtSetting.Add(hdName, clgcode);
                                }
                                // dtSetting.Add(clgcode + "$" + Convert.ToString(dtHdName.Rows[0]["headername"]), Convert.ToString(dtHdName.Rows[0]["headername"]));
                            }
                        }
                        // string headerName = d2.GetFunction("select distinct headername from fm_headermaster where headername like 'Bus Fees%'");
                    }
                }

            }
        }
        catch { }
        return dtSetting;
    }

    public void loadHostelHeader(string headerName)
    {
        try
        {
            StringBuilder sbName = new StringBuilder();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            Dictionary<string, string> dtHeader = getHostelSetting(collegecode);
            foreach (KeyValuePair<string, string> hdName in dtHeader)
            {
                sbName.Append(hdName.Key + "','");
            }
            if (sbName.Length > 0)
            {
                sbName.Remove(sbName.Length - 3, 3);
            }
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            if (sbName.Length > 0)
            {
                //   string headname = "select HeaderName,HeaderPK from FM_HeaderMaster where headerPK in('" + hedaderid + "')";
                string headname = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode  in('" + collegecode + "') and h.headername in('" + Convert.ToString(sbName) + "')  ";
                DataSet dshed = new DataSet();
                dshed = d2.select_method_wo_parameter(headname, "Text");
                if (dshed.Tables[0].Rows.Count > 0)
                {
                    chkl_studhed.DataSource = dshed;
                    chkl_studhed.DataValueField = "HeaderName";
                    chkl_studhed.DataTextField = "HeaderName";
                    chkl_studhed.DataBind();

                    int cnt = 0;
                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(headerName))
                        {
                            if (headerName.Contains(chkl_studhed.Items[i].Text))
                            {
                                chkl_studhed.Items[i].Selected = true;
                                cnt++;
                            }
                            else
                                chkl_studhed.Items[i].Selected = false;
                        }
                        else
                        {
                            chkl_studhed.Items[i].Selected = true;
                            cnt++;
                        }
                    }
                    txt_studhed.Text = lblheader.Text + "(" + cnt + ")";
                    if (chkl_studhed.Items.Count == cnt)
                        chk_studhed.Checked = true;
                    string ledgerName = string.Empty;
                    loadHostelLedger(ledgerName);
                }
            }
        }
        catch
        {
        }
    }

    public void loadHostelLedger(string ledgerName)
    {
        try
        {
            StringBuilder sbName = new StringBuilder();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            Dictionary<string, string> dtHeader = getHostelSettingLedger(collegecode);
            foreach (KeyValuePair<string, string> hdName in dtHeader)
            {
                sbName.Append(hdName.Key + "','");
            }
            if (sbName.Length > 0)
            {
                sbName.Remove(sbName.Length - 3, 3);
            }
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            if (sbName.Length > 0)
            {
                //   string headname = "select HeaderName,HeaderPK from FM_HeaderMaster where headerPK in('" + hedaderid + "')";
                string headname = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK =h.HeaderPK and   l.CollegeCode in('" + collegecode + "' ) and l.ledgername in('" + Convert.ToString(sbName) + "' )";
                // string headname = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode  in('" + collegecode + "') and h.headername in('" + Convert.ToString(sbName) + "')  ";
                DataSet dshed = new DataSet();
                dshed = d2.select_method_wo_parameter(headname, "Text");
                if (dshed.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = dshed;
                    chkl_studled.DataValueField = "ledgername";
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataBind();
                    int cnt = 0;
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ledgerName))
                        {
                            if (ledgerName.Contains(chkl_studled.Items[i].Text))
                            {
                                chkl_studled.Items[i].Selected = true;
                                cnt++;
                            }
                            else
                                chkl_studled.Items[i].Selected = false;
                        }
                        else
                        {
                            chkl_studled.Items[i].Selected = true;
                            cnt++;
                        }
                    }
                    txt_studled.Text = lbl_ledger.Text + "(" + cnt + ")";
                    if (cnt == chkl_studled.Items.Count)
                        chk_studled.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected Dictionary<string, string> getHostelSetting(string collegecode)
    {
        Dictionary<string, string> dtSetting = new Dictionary<string, string>();
        try
        {
            string selQ = " select distinct headerfk,r.college_code from ft_feeallotdegree ft,registration r where r.batch_year=ft.batchyear and r.degree_code=ft.degreecode and  isnull(ishostelfees,'0')='1' and r.college_code in('" + collegecode + "')";

            selQ += " select distinct headerpk,headername,collegecode from fm_headermaster where collegecode in('" + collegecode + "')";
            selQ += " select * from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + usercode + "' and college_code  in('" + collegecode + "') ";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0)
            {
                if (dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["headerfk"]);
                        string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                        if (dsVal.Tables[1].Rows.Count > 0)
                        {
                            dsVal.Tables[1].DefaultView.RowFilter = "headerpk='" + linkValue + "'";
                            DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                            if (dtHdName.Rows.Count > 0)
                            {
                                string hdName = Convert.ToString(dtHdName.Rows[0]["headername"]);
                                if (!dtSetting.ContainsKey(hdName))
                                    dtSetting.Add(hdName, clgcode);
                            }
                        }
                    }
                }
                if (dsVal.Tables[2].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[2].Rows.Count; row++)
                    {
                        string linkValue = Convert.ToString(dsVal.Tables[2].Rows[row]["linkvalue"]);
                        string clgcode = Convert.ToString(dsVal.Tables[2].Rows[row]["college_code"]);
                        string[] leng = linkValue.Split(',');
                        if (leng.Length == 2)
                        {
                            string hdFK = Convert.ToString(leng[0].Split('$')[1]);
                            string ldFK = Convert.ToString(leng[1]);
                            if (leng[0].Split('$')[0] == "1" && dsVal.Tables[1].Rows.Count > 0)
                            {
                                dsVal.Tables[1].DefaultView.RowFilter = "headerpk='" + hdFK + "'";
                                DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                                if (dtHdName.Rows.Count > 0)
                                {
                                    string hdName = Convert.ToString(dtHdName.Rows[0]["headername"]);
                                    if (!dtSetting.ContainsKey(hdName))
                                        dtSetting.Add(hdName, clgcode);
                                }
                            }
                        }
                    }
                }

            }
        }
        catch { }
        return dtSetting;
    }

    protected Dictionary<string, string> getHostelSettingLedger(string collegecode)
    {
        Dictionary<string, string> dtSetting = new Dictionary<string, string>();
        try
        {
            string selQ = " select distinct ledgerfk ,r.college_code from ft_feeallotdegree ft,registration r where r.batch_year=ft.batchyear and r.degree_code=ft.degreecode and  isnull(ishostelfees,'0')='1' and r.college_code in('" + collegecode + "')";

            selQ += " select distinct ledgerpk ,ledgername,collegecode from fm_ledgermaster where collegecode in('" + collegecode + "')";
            selQ += " select * from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + usercode + "' and college_code  in('" + collegecode + "') ";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0)
            {
                if (dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerfk"]);
                        string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                        if (dsVal.Tables[1].Rows.Count > 0)
                        {
                            dsVal.Tables[1].DefaultView.RowFilter = "ledgerpk='" + linkValue + "'";
                            DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                            if (dtHdName.Rows.Count > 0)
                            {
                                string hdName = Convert.ToString(dtHdName.Rows[0]["ledgername"]);
                                if (!dtSetting.ContainsKey(hdName))
                                    dtSetting.Add(hdName, clgcode);
                            }
                        }
                    }
                }
                if (dsVal.Tables[2].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[2].Rows.Count; row++)
                    {
                        string linkValue = Convert.ToString(dsVal.Tables[2].Rows[row]["linkvalue"]);
                        string clgcode = Convert.ToString(dsVal.Tables[2].Rows[row]["college_code"]);
                        string[] leng = linkValue.Split(',');
                        if (leng.Length == 2)
                        {
                            string hdFK = Convert.ToString(leng[0].Split('$')[1]);
                            string ldFK = Convert.ToString(leng[1].Split('$')[0]);
                            if (leng[0].Split('$')[0] == "1" && dsVal.Tables[1].Rows.Count > 0)
                            {
                                dsVal.Tables[1].DefaultView.RowFilter = "ledgerpk='" + ldFK + "'";
                                DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                                if (dtHdName.Rows.Count > 0)
                                {
                                    string hdName = Convert.ToString(dtHdName.Rows[0]["ledgername"]);
                                    if (!dtSetting.ContainsKey(hdName))
                                        dtSetting.Add(hdName, clgcode);
                                }
                            }
                        }
                    }
                }

            }
        }
        catch { }
        return dtSetting;
    }

    //added by sudhagar 23.08.2017

    protected void cbloadMaster_Changed(object sender, EventArgs e)
    {
        if (cbloadMaster.Checked)
        {
            string linkName = string.Empty;
            switch (rblMode.SelectedIndex)
            {
                case 0:
                    linkName = "43LoadMasterGeneral";
                    break;
                case 1:
                    linkName = "43LoadMasterHostel";
                    break;
                case 2:
                    linkName = "43LoadMasterTransport";
                    break;
            }
            string hdText = Convert.ToString(getCblSelectedTempText(chkl_studhed));
            string ldText = Convert.ToString(getCblSelectedTempText(chkl_studled));
            bool boolSel = false;
            string linkValue = loadMaster(linkName, hdText, ldText, boolSel);
            if (linkValue != "0")
            {
                string header = linkValue.Split('$')[0];
                string ledger = linkValue.Split('$')[1];
                switch (rblMode.SelectedIndex)
                {
                    case 0:
                        bindheader(header);
                        bindledger(ledger);
                        break;
                    case 1:
                        loadHostelHeader(header);
                        loadHostelLedger(ledger);
                        break;
                    case 2:
                        loadTransHeader(header);
                        loadTransLedger(ledger);
                        break;
                }
            }
        }
        else
        {
            string headerName = string.Empty;
            switch (rblMode.SelectedIndex)
            {
                case 0:
                    bindheader(headerName);
                    break;
                case 1:
                    loadHostelHeader(headerName);
                    break;
                case 2:
                    loadTransHeader(headerName);
                    break;
            }
        }
    }

    protected string loadMaster(string linkName, string hdTxt, string ldTxt, bool boolSel)
    {
        string linkVal = string.Empty;
        try
        {
            if (boolSel)
            {
                string linkValue = hdTxt + "$" + ldTxt;
                string InsQ = "if exists(select * from new_inssettings where linkname='" + linkName + "')update new_inssettings set linkvalue='" + linkValue + "' where linkname='" + linkName + "' else insert into new_inssettings (linkname,linkvalue) values('" + linkName + "','" + linkValue + "')";
                int insVal = d2.update_method_wo_parameter(InsQ, "Text");
            }
            if (!boolSel)
                linkVal = d2.GetFunction("select linkvalue from new_inssettings where linkname='" + linkName + "'");
        }
        catch { linkVal = "0"; }
        return linkVal;

    }

    //added by sudhagar 28.08.2017

    public void getAcademicYear()
    {
        try
        {
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "SELECT distinct ACD_YEAR FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD WHERE  AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK  AND  ACD_COLLEGE_CODE IN('" + collegecode + "') order by ACD_YEAR desc";
            ds.Dispose();
            ds.Reset();
            ddlAcademic.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["ACD_YEAR"].ToString();
                    ddlAcademic.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected Dictionary<string, string> getOldSettings(string acdYears)
    {
        Dictionary<string, string> htAcademic = new Dictionary<string, string>();
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            if (rblhostelmode.SelectedIndex == 1)
            {
                selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE  and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";

            }

            DataSet dsPrevAMount = d2.select_method_wo_parameter(selQ, "Text");
            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
            {
                DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();
                if (dtAcdYear.Rows.Count > 0)
                {
                    int Sno = 0;
                    for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                    {
                        Sno++;
                        string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                        string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                        dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                        DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                        if (dtBatch.Rows.Count > 0)
                        {
                            for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                            {
                                string acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtFee = dtFeecat.DefaultView.ToTable();
                                if (dtFee.Rows.Count > 0)
                                {
                                    StringBuilder sbSem = new StringBuilder();
                                    StringBuilder sbSemStr = new StringBuilder();
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        string feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        sbSem.Append(feecaT + ",");
                                        // sbSemStr.Append(feecaTStr + ",");
                                    }
                                    if (sbSem.Length > 0)
                                        sbSem.Remove(sbSem.Length - 1, 1);
                                    if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                        htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                    if (rblhostelmode.SelectedIndex == 1)
                                    {
                                        if (!htAcademic.ContainsKey(acdBatchYear))
                                            htAcademic.Add(acdBatchYear, Convert.ToString(sbSem));
                                    }
                                    //if (sbSemStr.Length > 0)
                                    //    sbSemStr.Remove(sbSemStr.Length - 1, 1);                              
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return htAcademic;

    }

    private string getCblSelectedTerm_Text(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true && (cblSelected.Items[sel].Text.ToUpper().Contains("TERM")))
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

    //added by abarna 26.01.2018

    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cb_hostelname.Checked == true)
                        {
                            cbl_hostelname.Items[i].Selected = true;
                            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                            build1 = cbl_hostelname.Items[i].Value.ToString();
                            if (buildvalue1 == "")
                            {
                                buildvalue1 = build1;
                            }
                            else
                            {
                                buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";

                }
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_hostelname.Text = "--Select--";
        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname.Checked = false;
                ///new 22/08/15
                build = cbl_hostelname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }

            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }
    }

    protected void bindhostel()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                //mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                // cbl_hostelname.Items.Insert(0, "--Select--");
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_seat_CheckedChanged(object sender, EventArgs e)
    {
        string seat = "";
        if (cb_seat.Checked == true)
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = true;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
            if (cbl_seat.Items.Count == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + (cbl_seat.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = false;
            }
            txt_seat.Text = "--Select--";
        }

    }

    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_seat.Text = "--Select--";
        string seat = "";
        cb_seat.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_seat.Items.Count; i++)
        {
            if (cbl_seat.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_seat.Items.Count)
            {
                cb_seat.Checked = true;
            }
            if (commcount == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
            }
        }
        loadquota();

    }

    public void loadseat()
    {

        try
        {

            cbl_seat.Items.Clear();

            string seat = "";
            string deptquery = "select distinct TextVal from TextValTable  where TextCriteria='seat' and college_code in('" + collegecode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextVal";
                cbl_seat.DataBind();

                if (cbl_seat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        cbl_seat.Items[i].Selected = true;
                        seat = Convert.ToString(cbl_seat.Items[i].Text);
                    }
                    if (cbl_seat.Items.Count == 1)
                    {
                        txt_seat.Text = "Seat(" + seat + ")";
                    }
                    else
                    {
                        txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    }
                    cb_seat.Checked = true;
                }
            }
            else
            {
                txt_seat.Text = "--Select--";

            }
        }
        catch
        {
        }

    }

    #region Quota

    protected void loadquota()
    {
        try
        {
            ds.Clear();
            cblQuota.Items.Clear();

            string itemheader = "";
            //for (int i = 0; i < cbl_seat.Items.Count; i++)
            //{
            //    if (cbl_seat.Items[i].Selected == true)
            //    {
            //        if (itemheader == "")
            //        {
            //            itemheader = "" + cbl_seat.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            itemheader = itemheader + "'" + "," + "" + "'" + cbl_seat.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            string SelQuery = string.Empty;
            SelQuery = "select distinct textcode as code from TextValTable  where TextCriteria='seat' and college_code in('" + collegecode + "')";
            // itemheader = getFilterValues(SelQuery);//modified by abarna
            // string itemheader = "";
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    if (itemheader == "")
                        itemheader = cbl_seat.Items[i].Value.ToString();
                    else
                        itemheader = itemheader + "','" + "" + cbl_seat.Items[i].Value.ToString() + "";
                }
            }
            //string SelQuery = string.Empty;
            SelQuery = "select distinct textcode as code from TextValTable  where TextCriteria='seat' and college_code in('" + collegecode + "') and textval in('" + itemheader + "') ";
            itemheader = getFilterValues(SelQuery);
            if (itemheader.Trim() != "")
            {
                // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.HeaderFK in('" + itemheader + "') and l.LedgerMode='0' and l.CollegeCode =" + collegecode1 + "";
                // string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                //string deptquery = "select distinct cat_code,category_name from seattype_cat where quota in('" + itemheader + "') and  degree_code in('" + degree + "')";
                //select distinct quota from seattype_cat where quota in('" + itemheader + "') and college_code='" + collegecode1 + "';

                string deptquery = "select distinct quotaname from stu_quotaseetinges where settype in('" + itemheader + "') and  collegecode in('" + collegecode + "')";//

                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblQuota.DataSource = ds;
                    cblQuota.DataTextField = "quotaname";
                    cblQuota.DataValueField = "quotaname";
                    cblQuota.DataBind();

                    for (int i = 0; i < cblQuota.Items.Count; i++)
                    {
                        cblQuota.Items[i].Selected = true;
                    }
                    txtQuota.Text = "Quota(" + cblQuota.Items.Count + ")";
                    cbQuota.Checked = true;

                }
                else
                {
                    txtQuota.Text = "--Select--";
                    cbQuota.Checked = false;
                }
            }
            else
            {
                txtQuota.Text = "--Select--";
                cbQuota.Checked = false;
            }
        }
        catch
        {
        }
    }

    protected void cbQuota_checkedchange(object sender, EventArgs e)
    {
        string ledgername = "";
        if (cbQuota.Checked == true)
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = true;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
            if (cblQuota.Items.Count == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + (cblQuota.Items.Count) + ")";
            }
            // txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = false;
            }
            txtQuota.Text = "--Select--";
        }

    }

    protected void cblQuota_SelectedIndexChange(object sender, EventArgs e)
    {
        string ledgername = "";
        txtQuota.Text = "--Select--";
        cbQuota.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblQuota.Items.Count; i++)
        {
            if (cblQuota.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            //   txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
            if (commcount == cblQuota.Items.Count)
            {
                cbQuota.Checked = true;
            }
            if (commcount == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + commcount.ToString() + ")";
            }
        }

    }

    #endregion
}