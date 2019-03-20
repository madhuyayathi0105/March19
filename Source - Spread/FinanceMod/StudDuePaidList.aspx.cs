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

public partial class StudDuePaidList : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
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
            //****************************************************//
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                edu_level();
                bindbatch();
                degree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                bindheader();
                loadpaid();
                //  loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                getPrintSettings();
                LoadIncludeSetting();
                //  loadfinanceyear();
                loadType();
                loadseat();
                loadcommunity();
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            popstud.Attributes.Add("Style", "display:none;");
        }
        catch (Exception ex) { }
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
        bindheader();
        loadpaid();
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
        bindheader();
        loadpaid();
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
    public void bindheader()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderName";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = lblheader.Text + "(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
                bindledger();
            }
        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        bindledger();
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        bindledger();
    }
    #endregion

    #region Ledger
    public void bindledger()
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
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK =h.HeaderPK and   l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ds;
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataValueField = "ledgername";
                    chkl_studled.DataBind();
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        chkl_studled.Items[i].Selected = true;
                    }
                    txt_studled.Text = lbl_ledger.Text + "(" + chkl_studled.Items.Count + ")";
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

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc"; 
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by edate desc";
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
                    ddlfinyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }

                //for (int i = 0; i < chklsfyear.Items.Count; i++)
                //{
                //    chklsfyear.Items[i].Selected = true;
                //    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                //}
                //if (chklsfyear.Items.Count == 1)
                //{
                //    txtfyear.Text = "" + fnalyr + "";
                //}
                //else
                //{
                //    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                //}
                //// txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                //chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
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

    #region seat type
    public void loadseat()
    {
        try
        {
            cbl_seat.Items.Clear();
            txt_seat.Text = "--Select--";
            cb_seat.Checked = false;
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
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
                    txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    cb_seat.Checked = true;
                }
            }
        }
        catch
        {
        }

    }
    protected void cb_seat_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_seat, cbl_seat, txt_seat, "Seat", "--Select--");
    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_seat, cbl_seat, txt_seat, "Seat", "--Select--");
    } 
    #endregion

    #region community
    public void loadcommunity()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            txt_community.Text = "--Select--";
            cb_community.Checked = false;
            cbl_community.Items.Clear();
            string comm = "";
         //   string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code in('" + collegecode + "')";
            string selq = "SELECT Distinct T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code in('" + collegecode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_community.DataSource = ds;
                    cbl_community.DataTextField = "TextVal";
                    cbl_community.DataValueField = "TextVal";
                    cbl_community.DataBind();
                    if (cbl_community.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_community.Items.Count; i++)
                        {
                            cbl_community.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_community.Items[i].Text);
                        }
                        txt_community.Text = "Community(" + cbl_community.Items.Count + ")";
                        cb_community.Checked = true;
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void cb_community_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_community, cbl_community, txt_community, "Community", "--Select--");
    }
    protected void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_community, cbl_community, txt_community, "Community", "--Select--");
    }
    #endregion

    #region student type
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
        CallCheckboxChange(cb_type, cbl_type, txt_type, "Type", "--Select--");
    }
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_type, cbl_type, txt_type, "Community", "--Select--");
    }
    #endregion

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
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
                SelQuery = " select distinct d.degree_code as code from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "')";
                degreeCode = getFilterValues(SelQuery);
            }
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sec));
            finyear = Convert.ToString(ddlfinyear.SelectedItem.Value);
            if (!string.IsNullOrEmpty(finyear))
            {
                string frDate = finyear.Split('-')[0];
                string toDate = finyear.Split('-')[1];
                SelQuery = " select distinct finyearpk as code from FM_FinYearMaster where CollegeCode in('" + collegecode + "') and FinYearStart='" + frDate.Split('/')[1] + "/" + frDate.Split('/')[0] + "/" + frDate.Split('/')[2] + "' and FinYearEnd='" + toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2] + "'  ";
                finyear = getFilterValues(SelQuery);
            }
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
            #region includem

            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string strInclude = string.Empty;
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1";
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "  r.DelFlag=1";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                    }
                }
            }
            if (!checkdicon.Checked)
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = "";
            }
            else
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + "";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + debar + "";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + disc + "";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cancel + "";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + "";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + disc + "";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + cancel + "";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + debar + " and " + disc + "";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + debar + " and " + cancel + "";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + disc + " or " + cancel + ")";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + " and " + disc + "";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and (" + disc + " or " + cancel + ")";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and " + cancel + "";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + debar + " and (" + disc + " or " + cancel + ")";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and (" + disc + " or " + cancel + ")";
            }

            #endregion
            #endregion

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
            {
                #region Query
                // string hdFK = getHeaderFK(hdText, collegecode);
                string SelQ = string.Empty;
                SelQ = " select  count(distinct app_no) as studcount,r.college_code,c.collname from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') ";
                if (!string.IsNullOrEmpty(studMode))
                    SelQ += " and r.mode in('" + studMode + "')";
                SelQ += "" + strInclude + " group by r.college_code ,c.collname order by r.college_code";

                SelQ += " select distinct sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,r.college_code from ft_feeallot f,registration r where r.app_no=f.app_no  and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdText + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + finyear + "')  ";
                if (!string.IsNullOrEmpty(studMode))
                    SelQ += " and r.mode in('" + studMode + "')";
                SelQ += "  " + strInclude + " group by r.college_code order by r.college_code";

                SelQ += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.college_code from ft_findailytransaction f,registration r where r.app_no=f.app_no and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and finyearfk in('" + finyear + "')    ";
                if (!string.IsNullOrEmpty(studMode))
                    SelQ += " and r.mode in('" + studMode + "')";
                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " group by r.college_code order by r.college_code";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelQ, "Text");
                #endregion
            }
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
    protected DataTable loadDetails(DataSet ds)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("College");
            dtpaid.Columns.Add("CollegeCode");
            dtpaid.Columns.Add("Student");
            dtpaid.Columns.Add("Demand");
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
                    double paidAmt = 0;
                    double paidAmtCr = 0;
                    double balAmt = 0;
                    double studCnt = 0;
                    drpaid["Sno"] = Convert.ToString(++rowCnt);
                    drpaid["College"] = Convert.ToString(ds.Tables[0].Rows[row]["collname"]);
                    drpaid["CollegeCode"] = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["studcount"]), out studCnt);
                    drpaid["Student"] = Convert.ToString(studCnt);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "college_code='" + ds.Tables[0].Rows[row]["college_code"] + "'";
                        DataView dvpaid = ds.Tables[1].DefaultView;
                        if (dvpaid.Count > 0)
                            double.TryParse(Convert.ToString(dvpaid[0]["total"]), out demandAmt);
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
                    drpaid["Demand"] = Convert.ToString(demandAmt);
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

                    if (!httotal.ContainsKey("Demand"))
                        httotal.Add("Demand", demandAmt);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(httotal["Demand"]), out amount);
                        amount += demandAmt;
                        httotal.Remove("Demand");
                        httotal.Add("Demand", Convert.ToString(amount));
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
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.TextCellType txtdemand = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtrcpt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtbal = new FarPoint.Web.Spread.TextCellType();

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblclg.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[1].Width = 400;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].Width = 90;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Demand";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[3].Width = 120;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Receipt";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[4].Width = 120;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Balance";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[5].Width = 120;


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
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtpaid.Rows[row]["College"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dtpaid.Rows[row]["CollegeCode"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtpaid.Rows[row]["Student"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtpaid.Rows[row]["Demand"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtpaid.Rows[row]["Receipt"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dtpaid.Rows[row]["credit"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dtpaid.Rows[row]["Balance"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtdemand;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtrcpt;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].CellType = txtbal;

                }
                else
                {
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = payModeText.Split('*')[0].TrimEnd('-');
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Total")
                    {
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    }
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtpaid.Rows[row]["Student"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtpaid.Rows[row]["Demand"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtpaid.Rows[row]["Receipt"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dtpaid.Rows[row]["Balance"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtdemand;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtrcpt;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].CellType = txtbal;
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = height;
            spreadDet.SaveChanges();
            #endregion
        }
        catch { }
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
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataTable dtpaid = loadDetails(ds);
            if (dtpaid.Rows.Count > 0)
            {
                loadSpreadDetails(dtpaid);
            }
            else
            {
                txtexcelname.Text = string.Empty;
                spreadDet.Visible = false;
                print.Visible = false;
                divlabl.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        else
        {
            //lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
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
                d2.printexcelreport(spreadDet, reportname);
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

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));

            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Institutionwise Balance Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FinanceBalDet.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
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
            string selQ = " select collname,college_code,acr from collinfo where college_code in('" + collegecode + "')";
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
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = true;
                }
                cbinclude.Checked = true;
                txtinclude.Text = "Include Settings(" + cblinclude.Items.Count + ")";
            }
        }
        catch { }
    }


    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");

    }


    #endregion

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    //spread cell click get student details
    protected void spreadDet_OnCellClick(object sender, EventArgs e)
    {
        boolStudClick = true;
    }
    protected void spreadDet_Selectedindexchanged(object sender, EventArgs e)
    {
        if (boolStudClick)
        {
            string collegecode = string.Empty;
            RollAndRegSettings();
            DataSet dsval = bindStudentDetails(ref collegecode);
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                getStudentDetails(dsval, collegecode);
            }
            else
            {
                popstud.Attributes.Add("Style", "display:none;");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
            boolStudClick = false;
        }
    }

    protected DataSet bindStudentDetails(ref string collecode)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
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
                SelQuery = " select distinct d.degree_code as code from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "') and dt.dept_name in('" + degreeCode + "')";
                degreeCode = getFilterValues(SelQuery);
            }
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sec));
            finyear = Convert.ToString(ddlfinyear.SelectedItem.Value);
            if (!string.IsNullOrEmpty(finyear))
            {
                string frDate = finyear.Split('-')[0];
                string toDate = finyear.Split('-')[1];
                SelQuery = " select distinct finyearpk as code from FM_FinYearMaster where CollegeCode in('" + collegecode + "') and FinYearStart='" + frDate.Split('/')[1] + "/" + frDate.Split('/')[0] + "/" + frDate.Split('/')[2] + "' and FinYearEnd='" + toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2] + "'  ";
                finyear = getFilterValues(SelQuery);
            }
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
            #region includem

            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string strInclude = string.Empty;
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1";
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "  r.DelFlag=1";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                    }
                }
            }
            if (!checkdicon.Checked)
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = "";
            }
            else
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + "";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + debar + "";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + disc + "";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cancel + "";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + "";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + disc + "";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + cancel + "";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + debar + " and " + disc + "";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + debar + " and " + cancel + "";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + disc + " or " + cancel + ")";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + " and " + disc + "";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and (" + disc + " or " + cancel + ")";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and " + cancel + "";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + debar + " and (" + disc + " or " + cancel + ")";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and (" + disc + " or " + cancel + ")";
            }

            #endregion
            #endregion
            int actrow = 0;
            int actCol = 0;
            int.TryParse(Convert.ToString(spreadDet.ActiveSheetView.ActiveRow), out actrow);
            int.TryParse(Convert.ToString(spreadDet.ActiveSheetView.ActiveColumn), out actCol);
            #region Query
            if (actCol == 4 && actrow != -1)
            {
                string collgCode = Convert.ToString(spreadDet.Sheets[0].Cells[actrow, 1].Tag);
                string debit = Convert.ToString(spreadDet.Sheets[0].Cells[actrow, actCol].Text);
                string credit = Convert.ToString(spreadDet.Sheets[0].Cells[actrow, actCol].Tag);
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batch))
                {
                    collecode = collgCode;
                    string selCol = "f.Transcode,convert(varchar(10),transdate,103) as Transdate, (select headername from fm_headermaster h where h.headerpk=f.headerfk and r.college_code=h.collegecode) as headername ,roll_no,reg_no,roll_admit,stud_name,sum(debit) as credit,sum(credit) as debit,batch_year,(select (c.course_name+'-'+dt.dept_acronym) from course c,degree d,department dt where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code and d.college_code in('" + collgCode + "')) as deptName ";
                    string GrpselCol = " f.Transcode,transdate, headerfk ,roll_no,reg_no,roll_admit,stud_name,credit,debit,batch_year,r.college_code,r.degree_code";
                    string SelQ = string.Empty;
                    SelQ = " select distinct ''sno," + selCol + " from registration r,ft_findailytransaction f where f.app_no=r.app_no and r.college_code in('" + collgCode + "') and f.headerfk in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "'  and r.batch_year in('" + batch + "') and r.degree_code in('" + degreeCode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and finyearfk in('" + finyear + "') ";
                    if (!string.IsNullOrEmpty(studMode))
                        SelQ += " and r.mode in('" + studMode + "')";
                    SelQ += "" + strInclude + " and isnull(iscanceled,'0')='0' and transcode<>'' and isnull(debit,'0')>'0' group by " + GrpselCol + "  order by f.Transcode";
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(SelQ, "Text");
                    spcredit.InnerHtml = debit;
                    spdebit.InnerHtml = credit;
                    //double credit = 0;
                    //double debit = 0;
                    //double.TryParse(Convert.ToString(e.Row.Cells[8].Text), out credit);
                    //double.TryParse(Convert.ToString(e.Row.Cells[9].Text), out debit);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Click Receipt Column')", true);
            }
            #endregion
        }
        catch { }
        return dsload;
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
    protected void printCollegeDet(string collegecode)
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode + " ";
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
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
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
}