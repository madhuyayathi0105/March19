using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class CriteriaForInternal : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    InsproDirectAccess dir = new InsproDirectAccess();
    DataSet ds = new DataSet();
    Hashtable has = new Hashtable();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = string.Empty;
    Boolean cellflag = false;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string fromtime = string.Empty;
    string totime = string.Empty;
    DateTime dtEndDateTime;
    DateTime dtStartDateTime;
    ReuasableMethods rs = new ReuasableMethods();
    DataTable dtbl = new DataTable();
    DataRow drtest;
    static Dictionary<int, string> dictstdets = new Dictionary<int, string>();

    protected void Page_PreInit(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            if (!IsPostBack)
            {

                ddlcriteria.Attributes.Add("onfocus", "frelig()");
                txtedate.Attributes.Add("readonly", "readonly");
                txtldate.Attributes.Add("readonly", "readonly");
                ViewState["CurrentTable"] = null;
                clear();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                GetTest();
                bindSuType();
                bindSubject();
                bindCourseOutcome();
                lbl_err.Visible = false;
                lblerrcritiria.Visible = false;
                lblexamerror.Visible = false;
                divSubCatagory.Visible = false;
                subctgrychckbox.Enabled = true;
            }
        }
        catch (Exception ex) { }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void binddegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            txt_degree.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < ddlbatch.Items.Count; i++)
            {
                if (ddlbatch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(ddlbatch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(ddlbatch.Items[i].Text);
                    }
                }

            }

            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
            }
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                cb_degree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }







    }

    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();

            cb_branch.Checked = false;
            txt_branch.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < ddlbatch.Items.Count; i++)
            {
                if (ddlbatch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(ddlbatch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(ddlbatch.Items[i].Text);
                    }
                }

            }
            string degree = "";
            for (int i1 = 0; i1 < cbl_degree.Items.Count; i1++)
            {
                if (cbl_degree.Items[i1].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i1].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i1].Value);
                    }
                }

            }




            if (cbl_degree.Items.Count > 0)
            {
                string degreecode = cbl_degree.SelectedValue.ToString();
                if (degreecode.Trim() != "")
                {
                    ds.Clear();
                    ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degree, collegecode = Session["collegecode"].ToString(), Session["usercode"].ToString());
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_branch.DataSource = ds;
                        cbl_branch.DataTextField = "dept_name";
                        cbl_branch.DataValueField = "degree_code";
                        cbl_branch.DataBind();
                    }
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                        cb_branch.Checked = true;
                    }
                }
            }



        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            if (cbl_branch.Items.Count > 0)
            {
                string degreecode = cbl_branch.SelectedValue.ToString();
                string strgetfuncuti = da.GetFunction("select max(Duration) from Degree");
                if (degreecode.Trim() != "")
                {
                    strgetfuncuti = da.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
                }
                if (Convert.ToInt32(strgetfuncuti) > 0)
                {
                    for (int loop_val = 1; loop_val <= Convert.ToInt32(strgetfuncuti); loop_val++)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }

    public void GetTest()
    {
        try
        {
            ddlcriteria.Items.Clear();
            if (ddlbatch.Items.Count == 0)
            {
                return;
            }
            if (cbl_branch.Items.Count == 0)
            {
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                return;
            }
            //  string gettext = "select c.criteria,c.Criteria_no from syllabus_master sy,CriteriaForInternal c where sy.syll_code=c.syll_code and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "'";
            string gettext = "select distinct criteria from CriteriaForInternal order by criteria";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method_wo_parameter(gettext, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcriteria.DataSource = ds;
                // ddlcriteria.DataValueField = "Criteria_No";
                ddlcriteria.DataTextField = "Criteria";
                ddlcriteria.DataBind();
                //ddlcriteria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }

    public void bindSuType()
    {
        try
        {
            txtSubType.Text = "";
            string sem = ddlsem.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degCode = string.Empty;
            CheckBoxList1.Items.Clear();
            if (cbl_branch.Items.Count > 0)
                degCode = rs.getCblSelectedValue(cbl_branch);
            if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batch))
            {
                string selBranch = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "'  order by ss.subject_type";//sub_sem.syll_Code = subject.syll_code and
                ds = da.select_method_wo_parameter(selBranch, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CheckBoxList1.DataSource = ds;
                    CheckBoxList1.DataTextField = "subject_type";
                    CheckBoxList1.DataValueField = "subject_type";
                    CheckBoxList1.DataBind();
                    checkBoxListselectOrDeselect(CheckBoxList1, true);
                    CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
                }
            }
        }
        catch
        {

        }
    }

    public void bindSubject()
    {
        try
        {
            txtSubject.Text = "";
            string sem = ddlsem.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degCode = string.Empty;
            string subtype = string.Empty;
            if (CheckBoxList1.Items.Count > 0)
                subtype = getCblSelectedText(CheckBoxList1);
            if (cbl_branch.Items.Count > 0)
                degCode = rs.getCblSelectedValue(cbl_branch);
            cblSubject.Items.Clear();
            if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(subtype))
            {
                //string SelectQ = "select s.subject_code,CONVERT(nvarchar(max),isnull(s.subject_code,'')+'-'+isnull(s.subject_name,'')) as text from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' and ss.subject_type in(" + subtype + ") order by ss.subType_no,s.subject_no";

                string SelectQ = "select  distinct subject.subject_code,subject.subject_name,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,registration where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=registration.degree_code and syllabus_master.semester =registration.current_semester and syllabus_master.batch_year=registration.batch_year   and  registration.degree_code in('" + degCode + "') and registration.batch_year in('" + batch + "') and registration.current_semester='" + sem + "' and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and sub_sem.subject_type in(" + subtype + ") order by subject.subject_name";//sub_sem.syll_Code = subject.syll_code and
                DataTable dtsubject = dir.selectDataTable(SelectQ);

                if (dtsubject.Rows.Count > 0)
                {
                    cblSubject.DataSource = dtsubject;
                    cblSubject.DataTextField = "text";
                    cblSubject.DataValueField = "subject_code";
                    cblSubject.DataBind();
                    if (cblSubject.Items.Count > 0)
                    {
                        for (int i = 0; i < cblSubject.Items.Count; i++)
                        {
                            cblSubject.Items[i].Selected = true;
                        }
                        txtSubject.Text = "Subject(" + cblSubject.Items.Count + ")";
                        cbSubjet.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void bindCourseOutcome()
    {
        try
        {
            ddlCo.Items.Clear();
            DataTable dtCoSett = dir.selectDataTable("select * from Master_Settings where settings='COSettings'");
            if (dtCoSett.Rows.Count > 0)
            {
                ddlCo.DataSource = dtCoSett;
                ddlCo.DataTextField = "template";
                ddlCo.DataValueField = "masterno";
                ddlCo.DataBind();
                ddlCo.Items.Insert(0, "");
            }
        }
        catch
        {
        }
    }

    public void clear()
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        GridView2.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
        PCriteria.Visible = false;
        PExamdetails.Visible = false;
    }
    //****added by Mullai
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_branch.Checked = false;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            bindbranch();
            bindsem();
            bindSuType();
            bindSubject();
        }
        catch { }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            bindbranch();
            bindsem();
            bindSuType();
            bindSubject();

        }
        catch { }
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int i = 0;
            cb_branch.Checked = false;
            int commcount = 0;
            txt_branch.Text = "--Select--";
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Department(" + commcount.ToString() + ")";
            }

            bindsem();
            bindSuType();
            bindSubject();


        }
        catch { }
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_branch.Text = "--Select--";
            if (cb_branch.Checked == true)
            {

                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Department(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
            }
            bindsem();
            bindSuType();
            bindSubject();


        }
        catch { }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int i = 0;
            cbSubjet.Checked = false;
            int commcount = 0;
            txtSubject.Text = "--Select--";
            for (i = 0; i < cblSubject.Items.Count; i++)
            {
                if (cblSubject.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSubject.Items.Count)
                {
                    cbSubjet.Checked = true;
                }
                txtSubject.Text = "Subjects(" + commcount.ToString() + ")";
            }
        }
        catch { }
    }


    public void cbSubjet_checkedchange(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtSubject.Text = "--Select--";
            if (cbSubjet.Checked == true)
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = true;
                }
                txtSubject.Text = "Subjects(" + (cblSubject.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = false;
                }
            }



        }
        catch { }
    }
    //****

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
        bindSubject();
    }
    public void CheckBox1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
        bindSubject();
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        binddegree();
        bindbranch();
        bindsem();
        bindSuType();
        bindSubject();
        // GetTest();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindbranch();
        bindsem();

        //GetTest();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindsem();
        // GetTest();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindSuType();
        bindSubject();
        // GetTest();
    }

    protected void ddlcriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            clear();
            if (ddlbatch.Items.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Batch And Then Proceed";
                return;
            }
            if (cbl_branch.Items.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Degree and Branch And Then Proceed";
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Sem And Then Proceed";
                return;
            }
            if (ddlcriteria.Items.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Criteria And Then Proceed";
                return;
            }
            string degCode = string.Empty;
            if (cbl_branch.Items.Count > 0)
                degCode = rs.getCblSelectedValue(cbl_branch);

            string strsecquery = "select distinct batch_year from Registration where Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code in('" + degCode + "') and current_semester='" + ddlsem.SelectedValue.ToString() + "' and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by batch_year";
            DataSet dssect1 = da.select_method_wo_parameter(strsecquery, "text");
            if (dssect1.Tables[0].Rows.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Student's Available";
                return;
            }
            ViewState["CurrentTable"] = null;

            dtbl.Columns.Clear();
            dtbl.Columns.Add("sno");
            dtbl.Columns.Add("degree");
            dtbl.Columns.Add("exam_code");
            dtbl.Columns.Add("branch");
            dtbl.Columns.Add("sections");
            dtbl.Columns.Add("criteria");
            dtbl.Columns.Add("criteriano");
            dtbl.Columns.Add("subcode");
            dtbl.Columns.Add("subno");
            dtbl.Columns.Add("subname");
            dtbl.Columns.Add("syllcode");
            dtbl.Columns.Add("exmdate");
            dtbl.Columns.Add("examduration");
            dtbl.Columns.Add("lock");
            dtbl.Columns.Add("lockdate");
            dtbl.Columns.Add("oldminmrk");
            dtbl.Columns.Add("minmark");
            dtbl.Columns.Add("oldmaxmrk");
            dtbl.Columns.Add("MaxMark");
            dtbl.Columns.Add("starttime");
            dtbl.Columns.Add("endtime");

            drtest = dtbl.NewRow();
            drtest["sno"] = "S.No";
            drtest["degree"] = "Degree";
            drtest["exam_code"] = "exam_code";
            drtest["branch"] = "Branch";
            drtest["sections"] = "Sections";
            drtest["criteria"] = "Criteria";
            drtest["criteriano"] = "criteriano";
            drtest["subcode"] = "Subject Code";
            drtest["subno"] = "subno";
            drtest["subname"] = "Subject Name";
            drtest["syllcode"] = "syllcode";
            drtest["exmdate"] = "Exam Date";
            drtest["examduration"] = "Exam Duration";
            drtest["lock"] = "Lock";
            drtest["lockdate"] = "Lock Date";
            drtest["oldminmrk"] = "oldminmrk";
            drtest["minmark"] = "Min.Mark";
            drtest["oldmaxmrk"] = "oldmaxmrk";
            drtest["MaxMark"] = "Max.Mark";
            drtest["starttime"] = "Start Time";
            drtest["endtime"] = "End Time";
            dtbl.Rows.Add(drtest);



            //  style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

            string subjectCode = string.Empty;
            string subCode = string.Empty;
            if (cblSubject.Items.Count > 0)
                subjectCode = rs.getCblSelectedValue(cblSubject);

            if (!string.IsNullOrEmpty(subjectCode))
                subCode = "  and s.subject_code in('" + subjectCode + "')";

            int srno = 0;
            for (int b = 0; b < cbl_branch.Items.Count; b++)
            {
                if (cbl_branch.Items[b].Selected)
                {
                    string strdegCode = Convert.ToString(cbl_branch.Items[b].Value);

                    string strsecquery1 = "select distinct Sections from Registration where Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code in('" + strdegCode + "') and current_semester='" + ddlsem.SelectedValue.ToString() + "' and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by Sections";
                    DataSet dssect = da.select_method_wo_parameter(strsecquery1, "text");

                    string strsubjectquery = "select sy.syll_code,ss.subject_type,ss.subType_no,s.subject_code,s.subject_no,s.subject_name from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and  sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code in('" + strdegCode + "') and sy.semester='" + ddlsem.SelectedValue.ToString() + "'  " + subCode + "   order by ss.subType_no,s.subject_no";
                    ds.Dispose();
                    ds.Reset();
                    ds = da.select_method_wo_parameter(strsubjectquery, "Text");
                    string deptstr = "select de.dept_acronym,c.Course_Name from Degree d,course c,Department de where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.Degree_Code='" + strdegCode + "'";
                    DataTable dtdept = dirAcc.selectDataTable(deptstr);
                    string dept = string.Empty;
                    string course = string.Empty;
                    if (dtdept.Rows.Count > 0)
                    {
                        dept = Convert.ToString(dtdept.Rows[0]["dept_acronym"]);
                        course = Convert.ToString(dtdept.Rows[0]["Course_Name"]);
                    }
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        GridView2.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        btnxl.Visible = true;
                        btnmasterprint.Visible = true;
                        string syllcode = ds.Tables[0].Rows[0]["syll_code"].ToString();
                        string criteriano = da.GetFunction("select distinct c.Criteria_no from CriteriaForInternal c where c.syll_code='" + ds.Tables[0].Rows[0]["syll_code"].ToString() + "' and c.criteria='" + ddlcriteria.SelectedItem.ToString() + "'");
                        if (criteriano.ToString().Trim() == "" || criteriano.Trim() == "0" || criteriano == null)
                        {
                            criteriano = "0";
                        }
                        //string strexamdetailsquery = "select c.syll_code,e.subject_no,e.sections,convert(nvarchar(15),e.exam_date,103) edate,e.min_mark,e.max_mark,e.new_maxmark,e.new_minmark,islock,convert(nvarchar(15),elockdate,103) elockdate,e.durationNew,e.duration,e.exam_code from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and c.syll_code='" + ds.Tables[0].Rows[0]["syll_code"].ToString() + "' and c.Criteria_no='" + criteriano + "'";
                        //Modified by rajkumar 12-10-2017//CONVERT(varchar,start_time,108)
                        string strexamdetailsquery = "select c.syll_code,e.subject_no,e.sections,convert(nvarchar(15),e.exam_date,103) edate,e.min_mark,e.max_mark,e.new_maxmark,e.new_minmark,islock,convert(nvarchar(15),elockdate,103) elockdate,e.durationNew,e.duration,e.exam_code,CONVERT(varchar,e.examFromTime,108) as examFromTime,CONVERT(varchar,e.examToTime,108) as examToTime from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and c.syll_code='" + ds.Tables[0].Rows[0]["syll_code"].ToString() + "' and c.Criteria_no='" + criteriano + "'";
                        DataSet dsexamdetsil = da.select_method_wo_parameter(strexamdetailsquery, "Text");
                        Hashtable hatsec = new Hashtable();
                        for (int s = 0; s < dssect.Tables[0].Rows.Count; s++)
                        {
                            string secval = Convert.ToString(dssect.Tables[0].Rows[s]["Sections"]);
                            string sectval = string.Empty;
                            if (secval.Trim() == "" || secval.Trim() == "-1" || secval.Trim() == null)
                            {
                                secval = string.Empty;
                            }
                            else
                            {
                                sectval = " and sections='" + secval + "'";
                            }
                            if (!hatsec.Contains(secval))
                            {
                                hatsec.Add(secval, secval);
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    string scode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                    string sname = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                    string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                                    srno++;


                                    string maxmark = string.Empty;
                                    string minmark = string.Empty;
                                    string newmaxmark = string.Empty;
                                    string newminmark = string.Empty;
                                    string edate = string.Empty;
                                    string elock = string.Empty;
                                    string eloackdate = string.Empty;
                                    string examDurationNew = string.Empty;
                                    string examDuration = string.Empty;
                                    string examCode = string.Empty;
                                    //
                                    string examFromTime = string.Empty;
                                    string examEndTime = string.Empty;
                                    //
                                    TimeSpan tsDuration = new TimeSpan(0, 0, 0);
                                    if (criteriano.ToString().Trim() != "" || criteriano.Trim() != "0" || criteriano != null)
                                    {
                                        dsexamdetsil.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "' " + sectval + "";
                                        DataView dvexam = dsexamdetsil.Tables[0].DefaultView;
                                        if (dvexam.Count > 0)
                                        {
                                            maxmark = dvexam[0]["max_mark"].ToString();
                                            minmark = dvexam[0]["min_mark"].ToString();
                                            newmaxmark = dvexam[0]["new_maxmark"].ToString();
                                            newminmark = dvexam[0]["new_minmark"].ToString();
                                            edate = dvexam[0]["edate"].ToString();
                                            elock = dvexam[0]["islock"].ToString();
                                            eloackdate = dvexam[0]["elockdate"].ToString();
                                            examDurationNew = Convert.ToString(dvexam[0]["durationNew"]).Trim();
                                            examDuration = Convert.ToString(dvexam[0]["duration"]).Trim();
                                            examCode = Convert.ToString(dvexam[0]["exam_code"]).Trim();
                                            examFromTime = Convert.ToString(dvexam[0]["examFromTime"]).Trim();
                                            examEndTime = Convert.ToString(dvexam[0]["examToTime"]).Trim();

                                        }
                                    }
                                    if (elock.Trim().ToLower() == "true" || elock.Trim() == "1")
                                    {
                                        elock = "Yes";
                                    }
                                    else
                                    {
                                        elock = "No";
                                        eloackdate = string.Empty;
                                    }
                                    //examDuration = txtDuration.Text.Trim();
                                    int hour = 0;
                                    int min = 0;
                                    int seconds = 0;
                                    //if (string.IsNullOrEmpty(examDuration.Trim()))
                                    //{
                                    //    lblexamerror.Visible = true;
                                    //    lblexamerror.Text = "Please Enter Exam Duration And Then Proceed";
                                    //    return;
                                    //}
                                    string[] durationSplit = examDurationNew.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (durationSplit.Length > 0)
                                    {
                                        if (durationSplit.Length >= 3)
                                        {
                                            int.TryParse(durationSplit[0].Trim(), out hour);
                                            int.TryParse(durationSplit[1].Trim(), out min);
                                            int.TryParse(durationSplit[2].Trim(), out seconds);
                                        }
                                        else if (durationSplit.Length == 2)
                                        {
                                            int temp1 = 0;
                                            int temp2 = 0;
                                            int.TryParse(durationSplit[0].Trim(), out temp1);
                                            int.TryParse(durationSplit[1].Trim(), out temp2);
                                            //int.TryParse(durationSplit[2].Trim(), out seconds);
                                            if (temp1 <= 12 || temp1 <= 23)
                                            {
                                                hour = temp1;
                                            }
                                            else if (temp1 < 60)
                                            {
                                                min = temp1;
                                            }
                                            if (temp2 <= 59)
                                            {
                                                min = temp2;
                                            }
                                        }
                                        else if (durationSplit.Length == 1)
                                        {
                                            int temp1 = 0;
                                            int.TryParse(durationSplit[0].Trim(), out temp1);
                                            //int.TryParse(durationSplit[1].Trim(), out temp2);
                                            //int.TryParse(durationSplit[2].Trim(), out seconds);
                                            if (temp1 <= 12 || temp1 <= 23)
                                            {
                                                hour = temp1;
                                            }
                                            else if (temp1 < 60)
                                            {
                                                min = temp1;
                                            }
                                        }
                                    }
                                    if (hour == 0 && min == 0 && seconds == 0)
                                    {
                                        durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (durationSplit.Length > 0)
                                        {
                                            if (durationSplit.Length >= 3)
                                            {
                                                int.TryParse(durationSplit[0].Trim(), out hour);
                                                int.TryParse(durationSplit[1].Trim(), out min);
                                                int.TryParse(durationSplit[2].Trim(), out seconds);
                                            }
                                            else if (durationSplit.Length == 2)
                                            {
                                                int temp1 = 0;
                                                int temp2 = 0;
                                                int.TryParse(durationSplit[0].Trim(), out temp1);
                                                int.TryParse(durationSplit[1].Trim(), out temp2);
                                                //int.TryParse(durationSplit[2].Trim(), out seconds);
                                                if (temp1 <= 12 || temp1 <= 23)
                                                {
                                                    hour = temp1;
                                                }
                                                else if (temp1 < 60)
                                                {
                                                    min = temp1;
                                                }
                                                if (temp2 <= 59)
                                                {
                                                    min = temp2;
                                                }
                                            }
                                            else if (durationSplit.Length == 1)
                                            {
                                                int temp1 = 0;
                                                int.TryParse(durationSplit[0].Trim(), out temp1);
                                                //int.TryParse(durationSplit[1].Trim(), out temp2);
                                                //int.TryParse(durationSplit[2].Trim(), out seconds);
                                                if (temp1 <= 12 || temp1 <= 23)
                                                {
                                                    hour = temp1;
                                                }
                                                else if (temp1 < 60)
                                                {
                                                    min = temp1;
                                                }
                                            }
                                        }
                                    }
                                    tsDuration = new TimeSpan(hour, min, seconds);
                                    string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                                    drtest = dtbl.NewRow();
                                    drtest["sno"] = Convert.ToString(srno);
                                    drtest["degree"] = Convert.ToString(course);
                                    drtest["exam_code"] = Convert.ToString(examCode);
                                    drtest["branch"] = Convert.ToString(dept);
                                    drtest["sections"] = Convert.ToString(secval);
                                    drtest["criteria"] = Convert.ToString(ddlcriteria.SelectedItem);
                                    drtest["criteriano"] = Convert.ToString(criteriano);
                                    drtest["subcode"] = Convert.ToString(scode);
                                    drtest["subno"] = Convert.ToString(subno);
                                    drtest["subname"] = Convert.ToString(sname);
                                    drtest["syllcode"] = Convert.ToString(syllcode);
                                    drtest["exmdate"] = Convert.ToString(edate);
                                    drtest["examduration"] = Convert.ToString(newduartion);
                                    drtest["lock"] = Convert.ToString(elock);
                                    drtest["lockdate"] = Convert.ToString(eloackdate);
                                    drtest["oldminmrk"] = Convert.ToString(minmark);
                                    drtest["minmark"] = Convert.ToString(newminmark);
                                    drtest["oldmaxmrk"] = Convert.ToString(maxmark);
                                    drtest["MaxMark"] = Convert.ToString(newmaxmark);
                                    drtest["starttime"] = Convert.ToString(examFromTime);
                                    drtest["endtime"] = Convert.ToString(examEndTime);

                                    dtbl.Rows.Add(drtest);


                                }
                                GridView2.DataSource = dtbl;
                                GridView2.DataBind();

                                GridView2.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                GridView2.ShowHeader = false;
                                GridView2.Rows[0].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                GridView2.Rows[0].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                                GridView2.Rows[0].Font.Bold = true;


                            }
                        }
                    }
                }
            }
            if (GridView2.Rows.Count == 0)
            {
                GridView2.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                lbl_err.Visible = true;
                lbl_err.Text = "No Subject's Available";
            }
            GridView2.HeaderRow.Visible = false;
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string ss = Session["usercode"].ToString();
        GridView2.Visible = true;
        Printcontrol.loadspreaddetails(GridView2, "CriteriaForInternal.aspx", "Criteria For Internal", 0, ss);
        //Printcontrol.loadspreaddetails(FpSpread1, "CriteriaForInternal.aspx", "Criteria For Internal");
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreportgrid(GridView2, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void btnreovecritreia_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtcriteria.Text = string.Empty;
            string degCode = rs.getCblSelectedValue(cbl_branch);
            if (ddlcriteria.Items.Count > 0)
            {
                string strgettest = ddlcriteria.SelectedItem.ToString();
                string crt = "select r.marks_obtained from CriteriaForInternal c,Exam_type e,Result r where c.criteria='" + ddlcriteria.SelectedItem.Text.ToString() + "' and e.criteria_no=c.Criteria_no and r.exam_code=e.exam_code";
                DataSet ds3 = new DataSet();
                ds3 = da.select_method_wo_parameter(crt, "text");
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    div4.Visible = true;
                    div5.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Marks Entered For This Test! Please Delete Marks.";
                    return;
                }
                else
                {
                    string crtino = da.GetFunction("select criteria_no from CriteriaForInternal where criteria='" + ddlcriteria.SelectedItem.Text.ToString() + "'");
                    if (!string.IsNullOrEmpty(crtino) && crtino != "0")
                    {
                        div6.Visible = true;
                        div7.Visible = true;
                        Label4.Visible = true;
                        Label4.Text = "Test Exits For This Criteria! Do You Want To Delete?";
                        return;
                        // ddlcriteria.Items.Remove(ddlcriteria.SelectedItem.Text.ToString());
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Test Exits For This Criteria')", true);
                        //return;
                    }
                    else
                    {
                        ddlcriteria.Items.Remove(ddlcriteria.SelectedItem.Text.ToString());
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Criteria Removed successfully')", true);
                    }
                }
                //string strget = da.GetFunction("select c.criteria,c.Criteria_no from syllabus_master sy,CriteriaForInternal c,Exam_type e where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + ddlsem.SelectedValue.ToString() + "' and c.criteria='" + strgettest + "'");
                //if (strget.Trim() == "0" || strget.Trim() == "")
                //{
                //    ddlcriteria.Items.Remove(ddlcriteria.SelectedItem.Text.ToString());
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Criteria Removed successfully')", true);
                //}
                //else
                //{
                //    lbl_err.Visible = true;
                //    lbl_err.Text = "Please Delete " + strgettest + " Details And Mark And Then Proceed";
                //}
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Criteria Availabe For Delete";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void btnrm_Click(object sender, EventArgs e)
    {
        try
        {
          
            string crtino = da.GetFunction("select criteria_no from CriteriaForInternal where criteria='" + ddlcriteria.SelectedItem.Text.ToString() + "'");
            string del = "delete Exam_type where criteria_no='" + crtino + "'; delete CriteriaForInternal where criteria_no='"+crtino+"' ";
            int del1 = da.update_method_wo_parameter(del, "text");
            if (del1 > 0)
            {
                ddlcriteria.Items.Remove(ddlcriteria.SelectedItem.Text.ToString());
                div6.Visible = false;
                div7.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Test Deleted Successfully";
            }
            else
            {
                div6.Visible = false;
                div7.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Test Not Deleted";
            }
        }
        catch
        {
           
        }
    }

    protected void btnrm1_Click(object sender, EventArgs e)
    {
        div6.Visible = false;
        div7.Visible = false;
    }

    protected void btnnewcriteria_Click(object sender, EventArgs e)
    {
        try
        {
            txtcriteria.Text = string.Empty;
            clear();
            PCriteria.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["dt"] != null)
            {
                DataTable dtNewTable = (DataTable)Session["dt"];
                GridView1.DataSource = dtNewTable;
                GridView1.DataBind();
                string uid = Request.Form["__EVENTTARGET"];// this.Page.Request.Params.Get("__EVENTTARGET");
                if (uid != null && !uid.Contains("btnMarkAtt"))
                {
                    if (!uid.Contains("chkAbsEntry"))
                    {
                        Session.Remove("dt");
                    }
                }
            }
        }
        catch { }
    }

    //added on 15/9/17
    protected void btnnewsubcriteria_Click(object sender, EventArgs e)
    {
        try
        {
            txtsubcriteria.Text = string.Empty;
            SubCriteria.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    //added on 15/9/17
    protected void btnsubdelcritreia_Click(object sender, EventArgs e)
    {
        string del = ddlsubcriteriamain.SelectedValue;
        string clgcode = Convert.ToString(Session["collegecode"]);
        string qry = " if not exists( select subjectId from  subsubjectTestDetails where textCode='" + ddlsubcriteriamain.SelectedValue + "') delete from TextValTable where TextCode=" + del + "and college_code=" + clgcode;
        da.update_method_wo_parameter(qry, "text");
        bindddl();
        //if exists( select subjectId from  subsubjectTestDetails where textCode='" + ddlSubSubjectName.SelectedValue + "' and examCode='" + examcode + "')
        GridView1.DataSource = tbl(false);
        GridView1.Visible = true;
        GridView1.DataBind();
        divSubCatagory.Visible = true;
    }

    //added on 15/9/17
    protected void btnsubcriteraiadd_Click(object sender, EventArgs e)
    {
        string clgcode = Convert.ToString(Session["collegecode"]);
        string newcriteria = txtsubcriteria.Text;
        if (!String.IsNullOrWhiteSpace(newcriteria))
        {
            try
            {
                string sqlcmd = "if not exists (select TextVal,TextCode from TextValTable where college_code='" + clgcode + "' and TextCriteria='ssub' and TextVal='" + newcriteria + "') insert into TextValTable (TextVal,TextCriteria,college_code) values('" + newcriteria + "','ssub','" + clgcode + "'); ";
                da.update_method_wo_parameter(sqlcmd, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Criteria Added successfully')", true);
                SubCriteria.Visible = false;
                PExamdetails.Visible = true;
                GridView1.DataSource = tbl(false);
                GridView1.Visible = true;
                GridView1.DataBind();
                divSubCatagory.Visible = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Problem with adding criteria')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('enter a valid name')", true);
        }
        bindddl();
    }

    //added on 15/9/17
    public DataTable binddropdown()
    {
        string clgcode = Convert.ToString(Session["collegecode"]);
        string qry = "select TextVal,TextCode from TextValTable where college_code='" + clgcode + "' and  TextCriteria='ssub';";
        DataTable dt = dirAcc.selectDataTable(qry);
        return dt;
    }

    //added on 15/9/17
    public DataTable tbl(bool addRow)
    {
        DataTable dtSubSubject = null; ;
        try
        {
            ArrayList addnew = new ArrayList();
            addnew.Add("1");
            dtSubSubject = new DataTable();
            dtSubSubject.Columns.Add("Category", typeof(string));
            dtSubSubject.Columns.Add("Min Marks", typeof(string));
            dtSubSubject.Columns.Add("Max Marks", typeof(string));
            dtSubSubject.Columns.Add("CategoryCode", typeof(string));
            dtSubSubject.Columns.Add("subjectId", typeof(string));

            DataRow dr;
            for (int row = 0; row < addnew.Count; row++)
            {
                dr = dtSubSubject.NewRow();
                dr[0] = addnew[row].ToString();
                if (ddlsubcriteriamain.Items.Count > dtSubSubject.Rows.Count)
                    dtSubSubject.Rows.Add(dr);
            }
            if (dtSubSubject.Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dtSubSubject;
                GridView1.DataSource = dtSubSubject;
                GridView1.DataBind();
            }
        }
        catch { }
        return dtSubSubject;
        //if (addRow)
        //{
        //    foreach (GridViewRow gvRow in GridView1.Rows)
        //    {
        //        DataRow drRow = dtSubSubject.NewRow();
        //        for (int cell = 0; cell < gvRow.Cells.Count; cell++)
        //        {
        //            drRow[cell] = gvRow.Cells[cell].Text;
        //        }
        //        dtSubSubject.Rows.Add(drRow);
        //    }
        //    if (ddlsubcriteriamain.Items.Count > dtSubSubject.Rows.Count)
        //        dtSubSubject.Rows.Add("", "", "", ""); 
        //}
        //if (dtSubSubject.Rows.Count == 0 && !addRow)
        //    dtSubSubject.Rows.Add("", "", "", "");
        //Session["dt"] = dtSubSubject;
    }

    //added on 15/9/17
    protected void btnsubcriteraiexit_Click(object sender, EventArgs e)
    {
        clear();
        SubCriteria.Visible = false;
        GridView2.Visible = true;
        PExamdetails.Visible = true;
    }

    //added on 15/9/17
    protected void bindddl()
    {
        string clgcode = Convert.ToString(Session["collegecode"]);
        string qry = "select TextVal,TextCode from TextValTable where college_code='" + clgcode + "' and  TextCriteria='ssub';";
        DataTable dt = dirAcc.selectDataTable(qry);
        ddlsubcriteriamain.DataSource = dt;
        ddlsubcriteriamain.DataTextField = "TextVal";
        ddlsubcriteriamain.DataValueField = "TextCode";
        ddlsubcriteriamain.DataBind();
    }

    //added on 15/9/17
    protected void subctgrychckbox_CheckedChanged(object sender, EventArgs e)
    {
        divSubCatagory.Visible = false;
        if (subctgrychckbox.Checked == true)
        {
            btnsubdelcritreia.Visible = true;
            ddlsubcriteriamain.Visible = true;
            btnnewsubcriteria.Visible = true;
            divSubCatagory.Visible = true;
            bindddl();
            if (ViewState["CurrentTable"] != null)
            {
                GridView1.DataSource = ViewState["CurrentTable"];
            }
            else
                GridView1.DataSource = tbl(true);

            GridView1.Visible = true;
            GridView1.DataBind();
        }
    }

    protected void GridView1_DataBound(object sender, GridViewRowEventArgs e)
    {
        string clgcode = Convert.ToString(Session["collegecode"]);
        string qry = "select TextVal,TextCode from TextValTable where college_code='" + clgcode + "' and  TextCriteria='ssub';";
        DataTable dt = dirAcc.selectDataTable(qry);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (e.Row.FindControl("ddlsubcriteria") as DropDownList);
            ddl.DataSource = dt;
            ddl.DataTextField = "TextVal";
            ddl.DataValueField = "TextCode";
            ddl.DataBind();
            Label lblCategory = (e.Row.FindControl("lblCategory") as Label);
            Label lblSubjectId = (e.Row.FindControl("lblSubjectId") as Label);
            ddl.Enabled = true;
            if (!string.IsNullOrEmpty(lblSubjectId.Text.Trim()) && lblSubjectId.Text.Trim() != "0")
            {
                ddl.SelectedValue = lblCategory.Text;
                //ddl.Enabled = false;
            }
        }
    }

    protected void addnewrow(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                DropDownList academic = new DropDownList();
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        academic = (DropDownList)GridView1.Rows[i].Cells[0].FindControl("ddlsubcriteria");
                        box1 = (TextBox)GridView1.Rows[i].Cells[1].FindControl("txtminmarks");
                        box2 = (TextBox)GridView1.Rows[i].Cells[2].FindControl("txtmaxmarks");
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i][0] = Convert.ToString(academic.SelectedItem).Trim();
                        dtCurrentTable.Rows[i][1] = box1.Text;
                        dtCurrentTable.Rows[i][2] = box2.Text;
                        dtCurrentTable.Rows[i][3] = Convert.ToString(academic.SelectedValue).Trim();
                        rowIndex++;
                    }
                    if (ddlsubcriteriamain.Items.Count > dtCurrentTable.Rows.Count)
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView1.DataSource = dtCurrentTable;
                    GridView1.DataBind();
                    divSubCatagory.Visible = true;
                }
            }
            else
            {
                GridView1.DataSource = tbl(false);
                GridView1.DataBind();
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Unable to add row ')", true);
            //throw;
        }
    }

    protected void btncriteraiadd_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            PCriteria.Visible = true;
            string newtestname = txtcriteria.Text.ToString();
            for (int i = 0; i < ddlcriteria.Items.Count; i++)
            {
                string exitcrite = ddlcriteria.Items[i].Text.ToString().Trim().ToLower();
                if (newtestname.Trim().ToLower() == exitcrite)
                {
                    lblerrcritiria.Visible = true;
                    lblerrcritiria.Text = "Criteria Already Exists";
                    return;
                }
            }
            ddlcriteria.Items.Insert(0, new System.Web.UI.WebControls.ListItem(txtcriteria.Text.ToString()));
            PCriteria.Visible = false;
            divSubCatagory.Visible = true;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Criteria Added successfully')", true);
        }
        catch (Exception ex)
        {
            lblerrcritiria.Visible = true;
            lblerrcritiria.Text = ex.ToString();
        }
    }

    protected void btncriteraiexit_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            PCriteria.Visible = false;
            ViewState["CurrentTable"] = null;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnexamexits_Click(object sender, EventArgs e)
    {
        PExamdetails.Visible = false;
    }

    protected void chklock_CheckedChanged(object sender, EventArgs e)
    {
        if (chklock.Checked == true)
        {
            txtldate.Enabled = true;
        }
        else
        {
            txtldate.Enabled = false;
        }
    }
    //Modified in 21-1-2017
    protected void btnexamDelete_Click(object sender, EventArgs e)
    {
        DataTable dtResult = new DataTable();
        string critreiano = string.Empty;
        string syllcode = string.Empty;
        string subno = string.Empty;
        string sections = string.Empty;
        string ExamCode = string.Empty;
        string SubCode = string.Empty;
        if (dictstdets.Count > 0)
        {
            foreach (KeyValuePair<int, string> dtval in dictstdets)
            {
                string details = dtval.Value;
                string[] det = details.Split(';');
                ExamCode = Convert.ToString(det[0]);
                critreiano = Convert.ToString(det[4]);
                int sylcd = dtval.Key;
                syllcode = Convert.ToString(sylcd);
                subno = Convert.ToString(det[5]);
                sections = Convert.ToString(det[3]);
                SubCode = Convert.ToString(det[6]);

            }
        }
        if (!string.IsNullOrEmpty(ExamCode))
        {
            //int insupdate=0
            string delResult = "Select * from Result where exam_code='" + ExamCode + "'";
            dtResult = dirAcc.selectDataTable(delResult);
            if (dtResult.Rows.Count > 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Result was Entered! Do you want Delete!";
                divPopAlert.Visible = true;
                btnConfirm.Visible = true;
                return;
            }
            else
            {
                int insupdate = 0;


                if (chkSameSub.Checked == true)
                {
                    // foreach(GridViewRow gr in GridView2.Rows)
                    for (int gr = 1; gr < GridView2.Rows.Count; gr++)
                    {
                        string critreia = ddlcriteria.SelectedItem.ToString().Trim();
                        string batchYear = ddlbatch.SelectedValue.ToString().Trim();
                        string critreiano1 = GridView2.Rows[gr].Cells[6].Text;
                        string syllcode1 = GridView2.Rows[gr].Cells[10].Text;
                        string subno1 = GridView2.Rows[gr].Cells[8].Text;
                        string sections1 = GridView2.Rows[gr].Cells[4].Text;
                        string SubCode1 = GridView2.Rows[gr].Cells[7].Text;
                        string exmCode = da.GetFunction("select exam_code from Exam_type where subject_no='" + subno1 + "' and criteria_no='" + critreiano1 + "' and sections='" + sections1 + "'");
                        if (SubCode.Trim().ToLower() == SubCode1.Trim().ToLower())
                        {
                            if (!string.IsNullOrEmpty(critreiano1) && !string.IsNullOrEmpty(syllcode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(critreia) && !string.IsNullOrEmpty(exmCode))
                            {
                                insupdate = 0;
                                string delExamType = "Delete from exam_type where criteria_no='" + critreiano1 + "' and batch_year='" + batchYear + "' and subject_no='" + subno1 + "' and sections='" + sections1 + "' and exam_code='" + exmCode + "'";
                                insupdate = da.update_method_wo_parameter(delExamType, "text");
                            }
                        }
                    }
                  
                }
                else if (chkcommonforall.Checked == true)
                {
                    for (int gr = 1; gr < GridView2.Rows.Count; gr++)
                    {
                        string critreia = ddlcriteria.SelectedItem.ToString().Trim();
                        string batchYear = ddlbatch.SelectedValue.ToString().Trim();
                        string critreiano1 = GridView2.Rows[gr].Cells[6].Text;
                        string syllcode1 = GridView2.Rows[gr].Cells[10].Text;
                        string subno1 = GridView2.Rows[gr].Cells[8].Text;
                        string sections1 = GridView2.Rows[gr].Cells[4].Text;
                        string SubCode1 = GridView2.Rows[gr].Cells[7].Text;
                        string exmCode = da.GetFunction("select exam_code from Exam_type where subject_no='" + subno1 + "' and criteria_no='" + critreiano1 + "' and sections='" + sections1 + "'");                       
                            if (!string.IsNullOrEmpty(critreiano1) && !string.IsNullOrEmpty(syllcode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(critreia) && !string.IsNullOrEmpty(exmCode))
                            {
                                //string delResultNew = "Delete from Result where exam_code='" + ExamCode + "'";
                                //insupdate = da.update_method_wo_parameter(delResultNew, "text");
                                insupdate = 0;
                                string delExamType = "Delete from exam_type where criteria_no='" + critreiano1 + "' and batch_year='" + batchYear + "' and subject_no='" + subno1 + "' and sections='" + sections1 + "' and exam_code='" + exmCode + "'";
                                insupdate = da.update_method_wo_parameter(delExamType, "text");
                            }
                    }
                }
                else
                {
                    string critreia = ddlcriteria.SelectedItem.ToString().Trim();
                    string batchYear = ddlbatch.SelectedValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(critreiano) && !string.IsNullOrEmpty(syllcode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(critreia) && !string.IsNullOrEmpty(ExamCode))
                    {
                        insupdate = 0;
                        string delExamType = "Delete from exam_type where criteria_no='" + critreiano + "' and batch_year='" + batchYear + "' and subject_no='" + subno + "' and sections='" + sections + "' and exam_code='" + ExamCode + "'";
                        insupdate = da.update_method_wo_parameter(delExamType, "text");
                    }
                }

                //insupdate = 0;
                //string delCriteria = "Delete from CriteriaForInternal where Criteria_no='" + critreiano + "' and syll_code='" + syllcode + "' and criteria='" + critreia + "' ";
                //insupdate = da.update_method_wo_parameter(delCriteria, "text");

                if (insupdate > 0)
                {
                    //ViewState.Clear();  
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Deleted successfully";
                    btnConfirm.Visible = false;
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Exam not Delete";
                    divPopAlert.Visible = true;
                    btnConfirm.Visible = true;
                    return;
                }
            }
        }
    }

    //modified on 15/9/17
    protected void btnexamsave_Click(object sender, EventArgs e)
    {
        try
        {
            int insupdate = 0;
            string examDuration = string.Empty;
            string critreiano = string.Empty;
            string syllcode = string.Empty;
            string sections = string.Empty;
            string subno = string.Empty;
            string maxmark = string.Empty;
            string minmark = string.Empty;
            string edate = string.Empty;
            string SubCode = string.Empty;


            if (dictstdets.Count > 0)
            {
                string exmcdde = string.Empty;
                string deg = string.Empty;
                string brnh = string.Empty;
                string snam = string.Empty;
                string exdat = string.Empty;
                string exdur = string.Empty;
                string elok = string.Empty;
                string elokd = string.Empty;
                foreach (KeyValuePair<int, string> dtval in dictstdets)
                {
                    int sylcd = dtval.Key;
                    syllcode = Convert.ToString(sylcd);
                    string dicval = dtval.Value;
                    string[] dicdetails = dicval.Split(';');
                    //string dicdetails = examCode + ";" + degree + ";" + branch + ";" + section + ";" + criteraino + ";" + subjectno + ";" + sub_cod + ";" + sub_nam + ";" + edate + ";" + examDuration + ";" + elock + ";" + elck_dat + ";" + mimrk + ";" + maxmk;
                    critreiano = Convert.ToString(dicdetails[4]);
                    sections = Convert.ToString(dicdetails[3]);
                    subno = Convert.ToString(dicdetails[5]);
                    maxmark = Convert.ToString(dicdetails[13]);
                    minmark = Convert.ToString(dicdetails[12]);
                    SubCode = Convert.ToString(dicdetails[6]);
                    exmcdde = Convert.ToString(dicdetails[0]);
                    deg = Convert.ToString(dicdetails[1]);
                    brnh = Convert.ToString(dicdetails[2]);
                    snam = Convert.ToString(dicdetails[7]);
                    exdat = Convert.ToString(dicdetails[8]);
                    exdur = Convert.ToString(dicdetails[9]);
                    elok = Convert.ToString(dicdetails[10]);
                    elokd = Convert.ToString(dicdetails[11]);

                }



                TimeSpan tsDuration = new TimeSpan(0, 0, 0);


                if (chkcommonforall.Checked == false && chkSameSub.Checked == false)
                {

                    edate = txtedate.Text.ToString();
                    if (edate.Trim() == "")
                    {
                        lblexamerror.Visible = true;
                        lblexamerror.Text = "Please Enter Exam Date And Then Proceed";
                        return;
                    }
                    string[] spe = edate.Split('/');
                    DateTime dte = Convert.ToDateTime(spe[1] + '/' + spe[0] + '/' + spe[2]);
                    examDuration = txtDuration.Text.Trim();
                    int hour = 0;
                    int min = 0;
                    int seconds = 0;
                    if (string.IsNullOrEmpty(examDuration.Trim()))
                    {
                        lblexamerror.Visible = true;
                        lblexamerror.Text = "Please Enter Exam Duration And Then Proceed";
                        return;
                    }

                    string[] durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (durationSplit.Length > 0)
                    {
                        if (durationSplit.Length >= 3)
                        {
                            int.TryParse(durationSplit[0].Trim(), out hour);
                            int.TryParse(durationSplit[1].Trim(), out min);
                            int.TryParse(durationSplit[2].Trim(), out seconds);
                        }
                        else if (durationSplit.Length == 2)
                        {
                            int temp1 = 0;
                            int temp2 = 0;
                            int.TryParse(durationSplit[0].Trim(), out temp1);
                            int.TryParse(durationSplit[1].Trim(), out temp2);
                            //int.TryParse(durationSplit[2].Trim(), out seconds);
                            if (temp1 <= 12 || temp1 <= 23)
                            {
                                hour = temp1;
                            }
                            else if (temp1 < 60)
                            {
                                min = temp1;
                            }
                            if (temp2 <= 59)
                            {
                                min = temp2;
                            }
                        }
                        else if (durationSplit.Length == 1)
                        {
                            int temp1 = 0;
                            int.TryParse(durationSplit[0].Trim(), out temp1);
                            //int.TryParse(durationSplit[1].Trim(), out temp2);
                            //int.TryParse(durationSplit[2].Trim(), out seconds);
                            if (temp1 <= 12 || temp1 <= 23)
                            {
                                hour = temp1;
                            }
                            else if (temp1 < 60)
                            {
                                min = temp1;
                            }
                        }
                    }
                    tsDuration = new TimeSpan(hour, min, seconds);
                    string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                    string lockdate = null;
                    string isloc = "0";
                    if (chklock.Checked == true)
                    {
                        isloc = "1";
                        string ldate = txtldate.Text.ToString();
                        if (ldate.Trim() != "")
                        {
                            string[] spl = ldate.Split('/');
                            DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                            lockdate = dtl.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            lblexamerror.Visible = true;
                            lblexamerror.Text = "Please Enter Lock Date And Then Proceed";
                            return;
                        }
                    }
                   
                    // MODIFIED RAJKUMAR
                    string[] frm = new string[5];
                    string[] to = new string[5];
                    fromtime = txtStartTime.Text;
                    totime = txtEndTime.Text;
                    frm = fromtime.Split(':');
                    to = totime.Split(':');
                    string[] startdurationSplit = fromtime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] EnddurationSplit = totime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (startdurationSplit.Length > 0 && EnddurationSplit.Length > 0)
                    {
                        if (startdurationSplit.Length == 2 && EnddurationSplit.Length == 2)
                        {
                            int temp1 = 0;
                            int temp2 = 0;
                            int temp3 = 0;
                            int temp4 = 0;
                            int.TryParse(startdurationSplit[0].Trim(), out temp1);
                            int.TryParse(startdurationSplit[1].Trim(), out temp2);
                            int.TryParse(EnddurationSplit[0].Trim(), out temp3);
                            int.TryParse(EnddurationSplit[1].Trim(), out temp4);
                            //int.TryParse(durationSplit[2].Trim(), out seconds);
                            if (temp1 <= 12 || temp1 <= 23 && temp2 <= 59)
                            {
                                if (temp3 <= 12 || temp3 <= 23 && temp4 <= 59)
                                {
                                    if (Convert.ToInt32(frm[0]) < Convert.ToInt32(to[0]))
                                    {
                                        string stTime = txtStartTime.Text;
                                        DateTime dtStartTime = new DateTime();
                                        DateTime.TryParseExact(stTime, "HH:mm", null, DateTimeStyles.None, out dtStartTime);
                                        dtStartDateTime = new DateTime(1, 1, 1, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second);

                                        string endtime = txtEndTime.Text;
                                        DateTime dtEndTime = new DateTime();
                                        DateTime.TryParseExact(endtime, "HH:mm", null, DateTimeStyles.None, out dtEndTime);

                                        dtEndDateTime = new DateTime(1, 1, 1, dtEndTime.Hour, dtEndTime.Minute, dtEndTime.Second);
                                    }
                                    else
                                    {
                                        lblexamerror.Visible = true;
                                        lblexamerror.Text = "Enter Valid StartTime and EndTime";
                                        return;

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblexamerror.Visible = true;
                        lblexamerror.Text = "Enter StartTime or EndTime";
                        return;
                    }
                    //------------------------------------------

                    string newminmark = txtminmark.Text.ToString().Trim();
                    string newmaxmark = txtmaxmark.Text.ToString().Trim();
                    if (newminmark.Trim() == "")
                    {
                        lblexamerror.Visible = true;
                        lblexamerror.Text = "Please Enter Min.Mark And Then Proceed";
                        return;
                    }
                    if (newmaxmark.Trim() == "")
                    {
                        lblexamerror.Visible = true;
                        lblexamerror.Text = "Please Enter Max.Mark And Then Proceed";
                        return;
                    }
                    if (minmark.Trim() == "")
                    {
                        minmark = newminmark;
                    }
                    if (maxmark.Trim() == "")
                    {
                        maxmark = newmaxmark;
                    }
                    int visible = 0;
                    if (CheckBox2.Checked)
                        visible = 1;

                    if (critreiano.Trim() == "0" || critreiano.Trim() == "")
                    {
                        string strcriteriainsert = "if exists (select * from CriteriaForInternal where syll_code='" + syllcode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "')";
                        strcriteriainsert = strcriteriainsert + " update CriteriaForInternal set max_mark='" + maxmark + "',min_mark='" + minmark + "',LastDate='" + lockdate + "'  where syll_code='" + syllcode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'";//LastDate='" + lockdate + "' rajkumar on 28-5-2018
                        strcriteriainsert = strcriteriainsert + " else";
                        strcriteriainsert = strcriteriainsert + " insert into CriteriaForInternal (syll_code,criteria,max_mark,min_mark,LastDate) values('" + syllcode + "','" + ddlcriteria.SelectedItem.ToString() + "','" + maxmark + "','" + minmark + "','" + lockdate + "')";
                        insupdate = da.update_method_wo_parameter(strcriteriainsert, "text");
                        critreiano = da.GetFunction("select criteria_no from CriteriaForInternal where syll_code='" + syllcode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'");
                    }
                    string insupdatevalues = "if not exists(select * from Exam_type where criteria_no='" + critreiano + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "')";
                    insupdatevalues = insupdatevalues + " insert into Exam_type (criteria_no,subject_no,exam_date,batch_year,max_mark,min_mark,sections,new_minmark,new_maxmark,islock,elockdate,durationNew,duration,examFromTime,examToTime,Visiblity)";
                    insupdatevalues = insupdatevalues + " values('" + critreiano + "','" + subno + "','" + dte.ToString("MM/dd/yyyy") + "','" + ddlbatch.SelectedValue.ToString() + "','" + newmaxmark + "','" + newminmark + "','" + sections + "','" + newminmark + "','" + newmaxmark + "','" + isloc + "','" + lockdate + "','" + newduartion + "','" + newduartion + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + visible + "')";
                    insupdatevalues = insupdatevalues + " else";
                    insupdatevalues = insupdatevalues + " update Exam_type set max_mark='" + newmaxmark + "',min_mark='" + newminmark + "',new_maxmark='" + newmaxmark + "',new_minmark='" + newminmark + "',exam_date='" + dte.ToString("MM/dd/yyyy") + "',islock='" + isloc + "', elockdate='" + lockdate + "',durationNew='" + newduartion + "',duration='" + newduartion + "',examFromTime='" + dtStartDateTime.ToString("HH:mm") + "',examToTime='" + dtEndDateTime.ToString("HH:mm") + "',Visiblity='" + visible + "'  where criteria_no='" + critreiano + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'";

                    insupdate = da.update_method_wo_parameter(insupdatevalues, "text");

                    string examcode = dirAcc.selectScalarString("select exam_code from Exam_type where criteria_no='" + critreiano + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'");   //modified on 15/9/17
                    savefunnew(examcode, GridView1);  //modified on 15/9/17
                    //btngo_Click(sender, e);
                }
                else if (chkcommonforall.Checked && chkSameSub.Checked == false)
                {
                    for (int gr = 1; gr < GridView2.Rows.Count; gr++)
                    {

                        sections = GridView2.Rows[gr].Cells[4].Text;
                        if (sections.Trim() == "&nbsp;")
                            sections = "";
                        subno = GridView2.Rows[gr].Cells[8].Text;
                        maxmark = GridView2.Rows[gr].Cells[18].Text;
                        if (maxmark.Trim() == "&nbsp;")
                            maxmark = "";
                        minmark = GridView2.Rows[gr].Cells[16].Text;
                        if (minmark.Trim() == "&nbsp;")
                            minmark = "";
                        string syllCode = GridView2.Rows[gr].Cells[10].Text;
                        if (syllCode.Trim() == "&nbsp;")
                            syllCode = "";
                        string criteriaCode = GridView2.Rows[gr].Cells[6].Text;
                        string SuCode = GridView2.Rows[gr].Cells[7].Text;

                        string slectedSub = string.Empty;
                        if (cblSubject.Items.Count > 0)
                            slectedSub = rs.getCblSelectedValue(cblSubject);
                        if (!string.IsNullOrEmpty(slectedSub))
                        {
                            if (slectedSub.Trim().ToLower().Contains(SuCode.Trim().ToLower()))//SubCode.Trim().ToLower() == SuCode.Trim().ToLower()
                            {
                                edate = txtedate.Text.ToString();
                                if (edate.Trim() == "")
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Please Enter Exam Date And Then Proceed";
                                    return;
                                }
                                string[] spe = edate.Split('/');
                                DateTime dte = Convert.ToDateTime(spe[1] + '/' + spe[0] + '/' + spe[2]);
                                examDuration = txtDuration.Text.Trim();
                                int hour = 0;
                                int min = 0;
                                int seconds = 0;
                                if (string.IsNullOrEmpty(examDuration.Trim()))
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Please Enter Exam Duration And Then Proceed";
                                    return;
                                }
                                // MODIFIED RAJKUMAR
                                string[] frm = new string[5];
                                string[] to = new string[5];
                                fromtime = txtStartTime.Text;
                                totime = txtEndTime.Text;
                                frm = fromtime.Split(':');
                                to = totime.Split(':');
                                string[] startdurationSplit = fromtime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                string[] EnddurationSplit = totime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (startdurationSplit.Length > 0 && EnddurationSplit.Length > 0)
                                {
                                    if (startdurationSplit.Length == 2 && EnddurationSplit.Length == 2)
                                    {
                                        int temp1 = 0;
                                        int temp2 = 0;
                                        int temp3 = 0;
                                        int temp4 = 0;
                                        int.TryParse(startdurationSplit[0].Trim(), out temp1);
                                        int.TryParse(startdurationSplit[1].Trim(), out temp2);
                                        int.TryParse(EnddurationSplit[0].Trim(), out temp3);
                                        int.TryParse(EnddurationSplit[1].Trim(), out temp4);
                                        //int.TryParse(durationSplit[2].Trim(), out seconds);
                                        if (temp1 <= 12 || temp1 <= 23 && temp2 <= 59)
                                        {
                                            if (temp3 <= 12 || temp3 <= 23 && temp4 <= 59)
                                            {
                                                if (Convert.ToInt32(frm[0]) < Convert.ToInt32(to[0]))
                                                {
                                                    string stTime = txtStartTime.Text;
                                                    DateTime dtStartTime = new DateTime();
                                                    DateTime.TryParseExact(stTime, "HH:mm", null, DateTimeStyles.None, out dtStartTime);
                                                    dtStartDateTime = new DateTime(1, 1, 1, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second);

                                                    string endtime = txtEndTime.Text;
                                                    DateTime dtEndTime = new DateTime();
                                                    DateTime.TryParseExact(endtime, "HH:mm", null, DateTimeStyles.None, out dtEndTime);

                                                    dtEndDateTime = new DateTime(1, 1, 1, dtEndTime.Hour, dtEndTime.Minute, dtEndTime.Second);
                                                    //string dur = dtresult.ToString();
                                                    //txtDuration.Text = dur;
                                                }
                                                else
                                                {
                                                    lblexamerror.Visible = true;
                                                    lblexamerror.Text = "Enter Valid StartTime and EndTime";
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Enter StartTime or EndTime";
                                    return;
                                }
                                //------------------------------------------
                                string[] durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (durationSplit.Length > 0)
                                {
                                    if (durationSplit.Length >= 3)
                                    {
                                        int.TryParse(durationSplit[0].Trim(), out hour);
                                        int.TryParse(durationSplit[1].Trim(), out min);
                                        int.TryParse(durationSplit[2].Trim(), out seconds);
                                    }
                                    else if (durationSplit.Length == 2)
                                    {
                                        int temp1 = 0;
                                        int temp2 = 0;
                                        int.TryParse(durationSplit[0].Trim(), out temp1);
                                        int.TryParse(durationSplit[1].Trim(), out temp2);
                                        //int.TryParse(durationSplit[2].Trim(), out seconds);
                                        if (temp1 <= 12 || temp1 <= 23)
                                        {
                                            hour = temp1;
                                        }
                                        else if (temp1 < 60)
                                        {
                                            min = temp1;
                                        }
                                        if (temp2 <= 59)
                                        {
                                            min = temp1;
                                        }
                                    }
                                    else if (durationSplit.Length == 1)
                                    {
                                        int temp1 = 0;
                                        int.TryParse(durationSplit[0].Trim(), out temp1);
                                        //int.TryParse(durationSplit[1].Trim(), out temp2);
                                        //int.TryParse(durationSplit[2].Trim(), out seconds);
                                        if (temp1 <= 12 || temp1 <= 23)
                                        {
                                            hour = temp1;
                                        }
                                        else if (temp1 < 60)
                                        {
                                            min = temp1;
                                        }
                                    }
                                }
                                tsDuration = new TimeSpan(hour, min, seconds);
                                string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                                string lockdate = null;
                                string isloc = "0";
                                if (chklock.Checked == true)
                                {
                                    isloc = "1";
                                    string ldate = txtldate.Text.ToString();
                                    if (ldate.Trim() != "")
                                    {
                                        string[] spl = ldate.Split('/');
                                        DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                                        lockdate = dtl.ToString("MM/dd/yyyy");
                                    }
                                    else
                                    {
                                        lblexamerror.Visible = true;
                                        lblexamerror.Text = "Please Enter Lock Date And Then Proceed";
                                        return;
                                    }
                                }
                                string newminmark = txtminmark.Text.ToString();
                                string newmaxmark = txtmaxmark.Text.ToString();
                                if (newminmark.Trim() == "")
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Please Enter Min.Mark And Then Proceed";
                                    return;
                                }
                                if (newmaxmark.Trim() == "")
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Please Enter Max.Mark And Then Proceed";
                                    return;
                                }
                                if (minmark.Trim() == "")
                                {
                                    minmark = newminmark;
                                }
                                if (maxmark.Trim() == "")
                                {
                                    maxmark = newmaxmark;
                                }
                                int vis = 0;
                                if (CheckBox2.Checked)
                                    vis = 1;
                                if (criteriaCode.Trim() == "0" || string.IsNullOrEmpty(criteriaCode))
                                {
                                    string strcriteriainsert = "if exists (select * from CriteriaForInternal where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "')";
                                    strcriteriainsert = strcriteriainsert + " update CriteriaForInternal set max_mark='" + maxmark + "',min_mark='" + minmark + "',LastDate='" + lockdate + "' where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'";
                                    strcriteriainsert = strcriteriainsert + " else";
                                    strcriteriainsert = strcriteriainsert + " insert into CriteriaForInternal (syll_code,criteria,max_mark,min_mark,LastDate) values('" + syllCode + "','" + ddlcriteria.SelectedItem.ToString() + "','" + maxmark + "','" + minmark + "','" + lockdate + "')";
                                    insupdate = da.update_method_wo_parameter(strcriteriainsert, "text");
                                    criteriaCode = da.GetFunction("select criteria_no from CriteriaForInternal where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'");
                                }
                                string insupdatevalues = "if not exists(select * from Exam_type where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "')";
                                insupdatevalues = insupdatevalues + " insert into Exam_type (criteria_no,subject_no,exam_date,batch_year,max_mark,min_mark,sections,new_minmark,new_maxmark,islock,elockdate,durationNew,duration,examFromTime,examToTime,Visiblity)";//examFromTime,examToTime
                                insupdatevalues = insupdatevalues + " values('" + criteriaCode + "','" + subno + "','" + dte.ToString("MM/dd/yyyy") + "','" + ddlbatch.SelectedValue.ToString() + "','" + newmaxmark + "','" + newminmark + "','" + sections + "','" + newminmark + "','" + newmaxmark + "','" + isloc + "','" + lockdate + "','" + newduartion + "','" + newduartion + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + vis + "')";
                                insupdatevalues = insupdatevalues + " else";
                                insupdatevalues = insupdatevalues + " update Exam_type set max_mark='" + newmaxmark + "',min_mark='" + newminmark + "',new_maxmark='" + newmaxmark + "',new_minmark='" + newminmark + "',exam_date='" + dte.ToString("MM/dd/yyyy") + "',islock='" + isloc + "', elockdate='" + lockdate + "',durationNew='" + newduartion + "',duration='" + newduartion + "',examFromTime='" + dtStartDateTime.ToString("HH:mm") + "',examToTime='" + dtEndDateTime.ToString("HH:mm") + "',Visiblity='" + vis + "' where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'";
                                insupdate = da.update_method_wo_parameter(insupdatevalues, "text");
                                string examcode = dirAcc.selectScalarString("select exam_code from Exam_type where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'");
                                savefunnew(examcode, GridView1);
                                //btngo_Click(sender, e);
                            }
                        }
                    }

                   
                }
                else if (chkSameSub.Checked)//-----------------------------Modified By Rajkumar
                {
                    // foreach (GridViewRow gr1 in GridView2.Rows)
                    for (int gr1 = 1; gr1 < GridView2.Rows.Count; gr1++)
                    {
                        sections = GridView2.Rows[gr1].Cells[4].Text;
                        if (sections.Trim() == "&nbsp;")
                            sections = "";
                        subno = GridView2.Rows[gr1].Cells[8].Text;
                        maxmark = GridView2.Rows[gr1].Cells[18].Text;
                        if (maxmark.Trim() == "&nbsp;")
                            maxmark = "";
                        minmark = GridView2.Rows[gr1].Cells[16].Text;
                        if (minmark.Trim() == "&nbsp;")
                            minmark = "";
                        string syllCode = GridView2.Rows[gr1].Cells[10].Text;
                        if (syllCode.Trim() == "&nbsp;")
                            syllCode = "";
                        string criteriaCode = GridView2.Rows[gr1].Cells[6].Text;
                        string SuCode = GridView2.Rows[gr1].Cells[7].Text;

                        if (SubCode.Trim().ToLower() == SuCode.Trim().ToLower())
                        {

                            edate = txtedate.Text.ToString();
                            if (edate.Trim() == "")
                            {
                                lblexamerror.Visible = true;
                                lblexamerror.Text = "Please Enter Exam Date And Then Proceed";
                                return;
                            }
                            string[] spe = edate.Split('/');
                            DateTime dte = Convert.ToDateTime(spe[1] + '/' + spe[0] + '/' + spe[2]);
                            examDuration = txtDuration.Text.Trim();
                            int hour = 0;
                            int min = 0;
                            int seconds = 0;
                            if (string.IsNullOrEmpty(examDuration.Trim()))
                            {
                                lblexamerror.Visible = true;
                                lblexamerror.Text = "Please Enter Exam Duration And Then Proceed";
                                return;
                            }
                            // MODIFIED RAJKUMAR
                            string[] frm = new string[5];
                            string[] to = new string[5];
                            fromtime = txtStartTime.Text;
                            totime = txtEndTime.Text;
                            frm = fromtime.Split(':');
                            to = totime.Split(':');
                            string[] startdurationSplit = fromtime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] EnddurationSplit = totime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (startdurationSplit.Length > 0 && EnddurationSplit.Length > 0)
                            {
                                if (startdurationSplit.Length == 2 && EnddurationSplit.Length == 2)
                                {
                                    int temp1 = 0;
                                    int temp2 = 0;
                                    int temp3 = 0;
                                    int temp4 = 0;
                                    int.TryParse(startdurationSplit[0].Trim(), out temp1);
                                    int.TryParse(startdurationSplit[1].Trim(), out temp2);
                                    int.TryParse(EnddurationSplit[0].Trim(), out temp3);
                                    int.TryParse(EnddurationSplit[1].Trim(), out temp4);
                                    //int.TryParse(durationSplit[2].Trim(), out seconds);
                                    if (temp1 <= 12 || temp1 <= 23 && temp2 <= 59)
                                    {
                                        if (temp3 <= 12 || temp3 <= 23 && temp4 <= 59)
                                        {
                                            if (Convert.ToInt32(frm[0]) < Convert.ToInt32(to[0]))
                                            {
                                                string stTime = txtStartTime.Text;
                                                DateTime dtStartTime = new DateTime();
                                                DateTime.TryParseExact(stTime, "HH:mm", null, DateTimeStyles.None, out dtStartTime);
                                                dtStartDateTime = new DateTime(1, 1, 1, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second);

                                                string endtime = txtEndTime.Text;
                                                DateTime dtEndTime = new DateTime();
                                                DateTime.TryParseExact(endtime, "HH:mm", null, DateTimeStyles.None, out dtEndTime);

                                                dtEndDateTime = new DateTime(1, 1, 1, dtEndTime.Hour, dtEndTime.Minute, dtEndTime.Second);
                                                //string dur = dtresult.ToString();
                                                //txtDuration.Text = dur;
                                            }
                                            else
                                            {
                                                lblexamerror.Visible = true;
                                                lblexamerror.Text = "Enter Valid StartTime and EndTime";
                                                return;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblexamerror.Visible = true;
                                lblexamerror.Text = "Enter StartTime or EndTime";
                                return;
                            }
                            //------------------------------------------
                            string[] durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (durationSplit.Length > 0)
                            {
                                if (durationSplit.Length >= 3)
                                {
                                    int.TryParse(durationSplit[0].Trim(), out hour);
                                    int.TryParse(durationSplit[1].Trim(), out min);
                                    int.TryParse(durationSplit[2].Trim(), out seconds);
                                }
                                else if (durationSplit.Length == 2)
                                {
                                    int temp1 = 0;
                                    int temp2 = 0;
                                    int.TryParse(durationSplit[0].Trim(), out temp1);
                                    int.TryParse(durationSplit[1].Trim(), out temp2);
                                    //int.TryParse(durationSplit[2].Trim(), out seconds);
                                    if (temp1 <= 12 || temp1 <= 23)
                                    {
                                        hour = temp1;
                                    }
                                    else if (temp1 < 60)
                                    {
                                        min = temp1;
                                    }
                                    if (temp2 <= 59)
                                    {
                                        min = temp1;
                                    }
                                }
                                else if (durationSplit.Length == 1)
                                {
                                    int temp1 = 0;
                                    int.TryParse(durationSplit[0].Trim(), out temp1);
                                    //int.TryParse(durationSplit[1].Trim(), out temp2);
                                    //int.TryParse(durationSplit[2].Trim(), out seconds);
                                    if (temp1 <= 12 || temp1 <= 23)
                                    {
                                        hour = temp1;
                                    }
                                    else if (temp1 < 60)
                                    {
                                        min = temp1;
                                    }
                                }
                            }
                            tsDuration = new TimeSpan(hour, min, seconds);
                            string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                            string lockdate = null;
                            string isloc = "0";
                            if (chklock.Checked == true)
                            {
                                isloc = "1";
                                string ldate = txtldate.Text.ToString();
                                if (ldate.Trim() != "")
                                {
                                    string[] spl = ldate.Split('/');
                                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                                    lockdate = dtl.ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    lblexamerror.Visible = true;
                                    lblexamerror.Text = "Please Enter Lock Date And Then Proceed";
                                    return;
                                }
                            }
                            string newminmark = txtminmark.Text.ToString();
                            string newmaxmark = txtmaxmark.Text.ToString();
                            if (newminmark.Trim() == "")
                            {
                                lblexamerror.Visible = true;
                                lblexamerror.Text = "Please Enter Min.Mark And Then Proceed";
                                return;
                            }
                            if (newmaxmark.Trim() == "")
                            {
                                lblexamerror.Visible = true;
                                lblexamerror.Text = "Please Enter Max.Mark And Then Proceed";
                                return;
                            }
                            if (minmark.Trim() == "")
                            {
                                minmark = newminmark;
                            }
                            if (maxmark.Trim() == "")
                            {
                                maxmark = newmaxmark;
                            }

                            int visib = 0;
                            if (CheckBox2.Checked)
                                visib = 1;
                            if (criteriaCode.Trim() == "0" || string.IsNullOrEmpty(criteriaCode))
                            {
                                string strcriteriainsert = "if exists (select * from CriteriaForInternal where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "')";
                                strcriteriainsert = strcriteriainsert + " update CriteriaForInternal set max_mark='" + maxmark + "',min_mark='" + minmark + "',LastDate='" + lockdate + "' where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'";
                                strcriteriainsert = strcriteriainsert + " else";
                                strcriteriainsert = strcriteriainsert + " insert into CriteriaForInternal (syll_code,criteria,max_mark,min_mark,LastDate) values('" + syllCode + "','" + ddlcriteria.SelectedItem.ToString() + "','" + maxmark + "','" + minmark + "','" + lockdate + "')";
                                insupdate = da.update_method_wo_parameter(strcriteriainsert, "text");
                                criteriaCode = da.GetFunction("select criteria_no from CriteriaForInternal where syll_code='" + syllCode + "' and criteria='" + ddlcriteria.SelectedItem.ToString() + "'");
                            }
                            string insupdatevalues = "if not exists(select * from Exam_type where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "')";
                            insupdatevalues = insupdatevalues + " insert into Exam_type (criteria_no,subject_no,exam_date,batch_year,max_mark,min_mark,sections,new_minmark,new_maxmark,islock,elockdate,durationNew,duration,examFromTime,examToTime,Visiblity)";//examFromTime,examToTime
                            insupdatevalues = insupdatevalues + " values('" + criteriaCode + "','" + subno + "','" + dte.ToString("MM/dd/yyyy") + "','" + ddlbatch.SelectedValue.ToString() + "','" + newmaxmark + "','" + newminmark + "','" + sections + "','" + newminmark + "','" + newmaxmark + "','" + isloc + "','" + lockdate + "','" + newduartion + "','" + newduartion + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + visib + "')";
                            insupdatevalues = insupdatevalues + " else";
                            insupdatevalues = insupdatevalues + " update Exam_type set max_mark='" + newmaxmark + "',min_mark='" + newminmark + "',new_maxmark='" + newmaxmark + "',new_minmark='" + newminmark + "',exam_date='" + dte.ToString("MM/dd/yyyy") + "',islock='" + isloc + "', elockdate='" + lockdate + "',durationNew='" + newduartion + "',duration='" + newduartion + "',examFromTime='" + dtStartDateTime.ToString("HH:mm") + "',examToTime='" + dtEndDateTime.ToString("HH:mm") + "',Visiblity='" + visib + "' where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'";
                            insupdate = da.update_method_wo_parameter(insupdatevalues, "text");
                            string examcode = dirAcc.selectScalarString("select exam_code from Exam_type where criteria_no='" + criteriaCode + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and subject_no='" + subno + "' and sections='" + sections + "'");
                            savefunnew(examcode, GridView1);
                        }
                    }
                }
                else
                {
                    lblexamerror.Visible = true;
                    lblexamerror.Text = "please Check  Any one!!";
                }
                ViewState.Clear();
                btngo_Click(sender, e);
                div5.Visible = true;
                div4.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Saved successfully";
               // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                //divSubCatagory.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblexamerror.Visible = true;
            lblexamerror.Text = ex.ToString();
        }
    }

    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div4.Visible = false;
        div5.Visible = false;

    }
    //added on 15/9/17
    protected void savefunnew(string examcode, GridView gd)
    {
        //txtedate,txtDuration,txtminmark,txtmaxmark
        DataTable dt = (DataTable)gd.DataSource;
        string qry;
        double mainsubjectminmarks = 0, mainsubjectmaxmarks = 0;
        //mainsubjectmaxmarks = double.Parse(txtmaxmark.Text);
        //mainsubjectminmarks = double.Parse(txtminmark.Text);
        double.TryParse(txtmaxmark.Text.Trim(), out mainsubjectmaxmarks);
        double.TryParse(txtminmark.Text.Trim(), out mainsubjectminmarks);
        string ddlselected = string.Empty;
        double max = 0, min = 0;
        bool IsSuccess = false;
        lblexamerror.Text = string.Empty; ;
        lblexamerror.Visible = false;
        ArrayList arrSubSubjectList = new ArrayList();
        string coNo = Convert.ToString(ddlCo.SelectedValue);
        if (string.IsNullOrEmpty(coNo))
            coNo = "0";
        if (subctgrychckbox.Checked)
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                DropDownList ddlSubSubjectName = new DropDownList();
                ddlSubSubjectName = (DropDownList)gvRow.Cells[0].FindControl("ddlsubcriteria");
                box1 = (TextBox)gvRow.Cells[1].FindControl("txtminmarks");
                box2 = (TextBox)gvRow.Cells[2].FindControl("txtmaxmarks");
                ddlselected = ddlSubSubjectName.SelectedValue;
                if (!arrSubSubjectList.Contains(ddlselected))
                    arrSubSubjectList.Add(ddlselected);
                double minimum = 0;
                double maximum = 0;
                double.TryParse(box2.Text.Trim(), out maximum);
                double.TryParse(box1.Text.Trim(), out minimum);
                max = max + maximum;
                min = min + minimum;
            }
        }
        qry = "select swm.appNo,swm.subjectId,swm.testMark,std.examCode,std.textCode,std.subSubjectName from subSubjectWiseMarkEntry swm,subsubjectTestDetails std where std.subjectId=swm.subjectId and std.examCode='" + examcode + "' order by swm.appNo";
        DataTable dtStudentMarks = dirAcc.selectDataTable(qry);
        string errMsg = string.Empty;
        if (arrSubSubjectList.Count < GridView1.Rows.Count)
        {
            //lblexamerror.Text = "You Are Selected Dublicate Criteria";
            //lblexamerror.Visible = true;
            //return;
        }
        //if (max <= mainsubjectmaxmarks && min <= mainsubjectminmarks)  // && arrSubSubjectList.Count == GridView1.Rows.Count
        //{
        foreach (GridViewRow gvRow in GridView1.Rows)
        {
            TextBox box1 = new TextBox();
            TextBox box2 = new TextBox();
            DropDownList ddlSubSubjectName = new DropDownList();
            ddlSubSubjectName = (DropDownList)gvRow.Cells[0].FindControl("ddlsubcriteria");
            box1 = (TextBox)gvRow.Cells[1].FindControl("txtminmarks");
            box2 = (TextBox)gvRow.Cells[2].FindControl("txtmaxmarks");
            Label lblSubjectId = (Label)gvRow.Cells[0].FindControl("lblSubjectId");
            int res = 0;
            DataView dvMark = new DataView();
            dtStudentMarks.DefaultView.RowFilter = "textCode='" + Convert.ToString(ddlSubSubjectName.SelectedValue).Trim() + "'";
            dvMark = dtStudentMarks.DefaultView;
            if (subctgrychckbox.Checked)
            {
                qry = "if exists(select subjectId from  subsubjectTestDetails where textCode='" + ddlSubSubjectName.SelectedValue + "' and examCode='" + examcode + "') update subsubjectTestDetails set minMark='" + box1.Text + "',maxMark='" + box2.Text + "',subSubjectName='" + ddlSubSubjectName.SelectedItem + "',CoNo='" + coNo + "' where  textCode='" + ddlSubSubjectName.SelectedValue + "' and examCode='" + examcode + "' else insert into subsubjectTestDetails (subSubjectName,textCode,examCode,minMark,maxMark,CoNo) values ('" + ddlSubSubjectName.SelectedItem + "','" + ddlSubSubjectName.SelectedValue + "','" + examcode + "','" + box1.Text + "','" + box2.Text + "','" + coNo + "')";
                res = dirAcc.insertData(qry);
                if (res != 0)
                    IsSuccess = true;
            }
            else
            {
                if (dvMark.Count == 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(lblSubjectId.Text).Trim()))
                    {
                        qry = "delete from  subsubjectTestDetails where textCode='" + ddlSubSubjectName.SelectedValue + "' and examCode='" + examcode + "' and subjectId='" + Convert.ToString(lblSubjectId.Text).Trim() + "'";
                        res = dirAcc.insertData(qry);
                        if (res != 0)
                            IsSuccess = true;
                    }
                }
                else
                {
                    errMsg += "You are Already Entered the Marks for " + ddlSubSubjectName.SelectedItem.Text + "\n";
                }
            }
            if (res != 0)
                IsSuccess = true;
        }
        if (!string.IsNullOrEmpty(errMsg))
        {
            lblexamerror.Text = errMsg;
            lblexamerror.Visible = true;
        }
        //}
        //else
        //{
        //    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Error saving the details')", true);
        //    lblexamerror.Text = "Please Check Minimum And Maximum Marks";
        //    lblexamerror.Visible = true;
        //    return;
        //}
        //if (IsSuccess)
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
        //}
    }

    protected void txtEndTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string[] frm = new string[5];
            string[] to = new string[5];
            fromtime = txtStartTime.Text;
            totime = txtEndTime.Text;
            frm = fromtime.Split(':');
            to = totime.Split(':');
            string[] startdurationSplit = fromtime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string[] EnddurationSplit = totime.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (startdurationSplit.Length > 0 && EnddurationSplit.Length > 0)
            {
                if (startdurationSplit.Length == 2 && EnddurationSplit.Length == 2)
                {
                    int temp1 = 0;
                    int temp2 = 0;
                    int temp3 = 0;
                    int temp4 = 0;
                    int.TryParse(startdurationSplit[0].Trim(), out temp1);
                    int.TryParse(startdurationSplit[1].Trim(), out temp2);
                    int.TryParse(EnddurationSplit[0].Trim(), out temp3);
                    int.TryParse(EnddurationSplit[1].Trim(), out temp4);
                    //int.TryParse(durationSplit[2].Trim(), out seconds);
                    if (temp1 <= 12 || temp1 <= 23 && temp2 <= 59)
                    {
                        if (temp3 <= 12 || temp3 <= 23 && temp4 <= 59)
                        {
                            if (Convert.ToInt32(frm[0]) < Convert.ToInt32(to[0]))
                            {
                                lblerr.Visible = false;
                                DateTime dt1 = Convert.ToDateTime(txtStartTime.Text);
                                DateTime dt2 = Convert.ToDateTime(txtEndTime.Text);
                                TimeSpan dtresult = dt2 - dt1;
                                string dur = dtresult.ToString();
                                txtDuration.Text = dur;
                            }
                            else
                            {
                                lblerr.Visible = true;
                                lblerr.ToolTip = "Please insert the End time greater than beginning time";
                                lblerr.ForeColor = Color.Red;
                                txtEndTime.Text = String.Empty;
                                lblerr.Text = "*";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Time Should be Entered in 24hrs Format')", true);
                                txtDuration.Text = "00:00:00";
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.ToolTip = "Please Enter the End time ";
        }
    }

    protected void btnPopConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int insupdate = 0;
            string ExamCode = string.Empty;
            string critreiano = string.Empty;
            string syllcode = string.Empty;
            string subno = string.Empty;
            string sections = string.Empty;
            if (dictstdets.Count > 0)
            {
                foreach (KeyValuePair<int, string> dtval in dictstdets)
                {
                    string details = dtval.Value;
                    string[] det = details.Split(';');
                    ExamCode = Convert.ToString(det[0]);
                    critreiano = Convert.ToString(det[4]);
                    int sylcd = dtval.Key;
                    syllcode = Convert.ToString(sylcd);
                    subno = Convert.ToString(det[5]);
                    sections = Convert.ToString(det[3]);

                }
            }
            string critreia = ddlcriteria.SelectedItem.ToString().Trim();
            string batchYear = ddlbatch.SelectedValue.ToString().Trim();
            if (!string.IsNullOrEmpty(critreiano) && !string.IsNullOrEmpty(syllcode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(critreia) && !string.IsNullOrEmpty(ExamCode))
            {
                string delResult = "Delete from Result where exam_code='" + ExamCode + "'";
                insupdate = da.update_method_wo_parameter(delResult, "text");
                insupdate = 0;
                string delExamType = "Delete from exam_type where criteria_no='" + critreiano + "' and batch_year='" + batchYear + "' and subject_no='" + subno + "' and sections='" + sections + "' and exam_code='" + ExamCode + "'";
                insupdate = da.update_method_wo_parameter(delExamType, "text");
                //insupdate = 0;
                //string delCriteria = "Delete from CriteriaForInternal where Criteria_no='" + critreiano + "' and syll_code='" + syllcode + "' and criteria='" + critreia + "' ";
                //insupdate = da.update_method_wo_parameter(delCriteria, "text");

                if (insupdate > 0)
                {
                    //ViewState.Clear();  
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                    //PExamdetails.Visible = false;
                    //btngo_Click(sender, e);
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Deleted successfully";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Exam not Delete";
                    divPopAlert.Visible = true;
                    return;
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Exam not Delete')", true);
                    //btngo_Click(sender, e);
                }
            }
        }
        catch
        {

        }
        //lblAlertMsg.Visible = true;
        //lblAlertMsg.Text = "No Hall Were Found";
        //divPopAlert.Visible = true;
        //return;
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

    protected void chkcommonforall_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkcommonforall.Checked)
            chkSameSub.Checked = false;
    }

    protected void chkSameSub_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkSameSub.Checked)
            chkcommonforall.Checked = false;
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void gridview2_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridView2.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView2.Rows[rowIndex];
                GridViewRow previousRow = GridView2.Rows[rowIndex + 1];


                string l3 = row.Cells[5].Text;
                string l4 = (previousRow.Cells[5].Text);
                if (l3 == l4)
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                           previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }
                string l5 = (row.Cells[3].Text);
                string l6 = (previousRow.Cells[3].Text);
                if (l5 == l6)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                    string l1 = (row.Cells[4].Text);
                    string l2 = (previousRow.Cells[4].Text);
                    if (l1 == l2)
                    {
                        row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                               previousRow.Cells[4].RowSpan + 1;
                        previousRow.Cells[4].Visible = false;
                    }
                }
                string l7 = (row.Cells[1].Text);
                string l8 = (previousRow.Cells[1].Text);
                if (l7 == l8)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }


            }
        }

        catch
        {
        }
    }

    protected void SelectedIndexChanged(object sender, EventArgs e)
    {
        Label1.Text = "";
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        ddlCo.ClearSelection();
        chkcommonforall.Checked = false;
        PExamdetails.Visible = true;
        ViewState["CurrentTable"] = null;
        if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
        {
            string stime = GridView2.Rows[rowIndex].Cells[19].Text;
            if (stime.Trim() == "&nbsp;")
                stime = "";

            if (stime == "" || stime == null)
            {
                txtStartTime.Text = "00:00";
            }
            else
            {
                TimeSpan ts = TimeSpan.Parse(stime);
                txtStartTime.Text = ts.ToString(@"hh\:mm");

            }
            string Etime = GridView2.Rows[rowIndex].Cells[20].Text;
            if (Etime.Trim() == "&nbsp;")
                Etime = "";
            if (Etime == "" || Etime == null)
            {
                txtEndTime.Text = "00:00";
            }
            else
            {
                TimeSpan ts1 = TimeSpan.Parse(Etime);
                txtEndTime.Text = ts1.ToString(@"hh\:mm");
            }
            string elck_dat = string.Empty;
            string sub_cod = GridView2.Rows[rowIndex].Cells[7].Text;
            if (sub_cod.Trim() == "&nbsp;")
                sub_cod = string.Empty ;
            txtscode.Text = sub_cod;
            string sub_nam = GridView2.Rows[rowIndex].Cells[9].Text;
            if (sub_nam.Trim() == "&nbsp;")
                sub_nam = string.Empty;
            txtsname.Text = sub_nam;
            string edate = GridView2.Rows[rowIndex].Cells[11].Text;
            if (edate.Trim() == "&nbsp;")
                edate = "";
            txtedate.Text = edate;
            string examCode = GridView2.Rows[rowIndex].Cells[2].Text;
            if (examCode.Trim() == "&nbsp;")
                examCode = "";
            txtedate.Text = edate;
            string examDuration = GridView2.Rows[rowIndex].Cells[12].Text;
            if (examDuration.Trim() == "&nbsp;")
                examDuration = "";
            txtDuration.Text = examDuration;
            string elock = GridView2.Rows[rowIndex].Cells[13].Text;
            if (elock.Trim() == "&nbsp;")
                elock = "";
            //added
            if (!string.IsNullOrEmpty(examCode) && examCode != "0")
            {
                string qry = "select subSubjectName as Category,minMark [Min Marks],maxMark as [Max Marks],textCode as [CategoryCode],subjectId,cono from subsubjectTestDetails where examCode='" + examCode + "'";
                ds = da.select_method_wo_parameter(qry, "text");
            }
            divSubCatagory.Visible = false;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCo.ClearSelection();
                string cono = Convert.ToString(ds.Tables[0].Rows[0]["cono"]);
                if (!string.IsNullOrEmpty(cono) && cono != "0")
                    ddlCo.Items.FindByValue(cono).Selected = true;

                GridView1.DataSource = ds.Tables[0];
                ViewState["CurrentTable"] = ds.Tables[0];
                GridView1.Visible = true;
                bindddl();
                GridView1.DataBind();
                divSubCatagory.Visible = true;
                subctgrychckbox.Checked = true;
                subctgrychckbox.Enabled = true;
            }
            else
            {
                addnewrow(sender, e);
                subctgrychckbox.Checked = false;
                subctgrychckbox.Enabled = true;
            }
            if (elock.Trim().ToLower() == "yes")
            {
                chklock.Checked = true;
                txtldate.Enabled = true;
                elck_dat = GridView2.Rows[rowIndex].Cells[14].Text;
                if (elck_dat.Trim() == "&nbsp;")
                    elck_dat = "";
                txtldate.Text = elck_dat;
            }
            else
            {
                chklock.Checked = false;
                txtldate.Text = string.Empty;
                txtldate.Enabled = false;
            }
            string mimrk = GridView2.Rows[rowIndex].Cells[16].Text;
            if (mimrk.Trim() == "&nbsp;")
                mimrk = "";
            txtminmark.Text = mimrk;
            string maxmk = GridView2.Rows[rowIndex].Cells[18].Text;
            if (maxmk.Trim() == "&nbsp;")
                maxmk = "";
            txtmaxmark.Text = maxmk;
            //CheckBox2.Checked = true;


            Label1.Text = Convert.ToString(ddlcriteria.SelectedItem.Text);
            string degree = GridView2.Rows[rowIndex].Cells[1].Text;
            if (degree.Trim() == "&nbsp;")
                degree = string.Empty;
            string branch = GridView2.Rows[rowIndex].Cells[3].Text;
            branch = branch.Replace("amp;", string.Empty);
            if (branch.Trim() == "&nbsp;")
                branch = string.Empty;
            string section = GridView2.Rows[rowIndex].Cells[4].Text;
            if (section.Trim() == "&nbsp;")
                section = string.Empty;
            string criteraino = GridView2.Rows[rowIndex].Cells[6].Text;
            if (criteraino.Trim() == "&nbsp;")
                criteraino = string.Empty;
            string subjectno = GridView2.Rows[rowIndex].Cells[8].Text;
            if (subjectno.Trim() == "&nbsp;")
                subjectno = string.Empty;
            string syllcode = GridView2.Rows[rowIndex].Cells[10].Text;
            if (syllcode.Trim() == "&nbsp;")
                syllcode = string.Empty;
            string sect = string.Empty;
            if (!string.IsNullOrEmpty(section))
                sect = "  and sections='" + section + "'";
            string visiblity = da.GetFunction("select Visiblity from exam_type  where criteria_no='" + criteraino + "' and subject_no='" + subjectno + "' " + sect + "");
            if (visiblity.Trim() == "1" || visiblity.Trim().ToLower() == "true")
                CheckBox2.Checked = true;
            else
                CheckBox2.Checked = false;

            if (syllcode.Trim() == "&nbsp;")
                syllcode = "";
            int syll_code = Convert.ToInt32(syllcode);
            string dicdetails = examCode + ";" + degree + ";" + branch + ";" + section + ";" + criteraino + ";" + subjectno + ";" + sub_cod + ";" + sub_nam + ";" + edate + ";" + examDuration + ";" + elock + ";" + elck_dat + ";" + mimrk + ";" + maxmk;
            dictstdets.Clear();
            dictstdets.Add(syll_code, dicdetails);

        }
    }

    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[15].Visible = false;
            e.Row.Cells[17].Visible = false;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[14].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[15].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[16].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[17].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[18].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[19].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[20].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

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


    #endregion
}