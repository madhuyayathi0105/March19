using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Configuration;
public partial class SubjectWiseExternalRanklist : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable has = new Hashtable();

    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string group_code = string.Empty;
    string bran = string.Empty;
    string buildvalue = string.Empty;
    string build = string.Empty;
    int cout = 0;

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
            lbl_err.Visible = false;
            if (!IsPostBack)
            {
                clear();
                loadtype();
                loadedu();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem1();
                loadsubtype();
                loadsubject();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = da.select_method_wo_parameter(Master, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
                rbwoarrear.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadtype()
    {
        try
        {
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            ddlstream.Items.Clear();
            string strquery = "select distinct ltrim(ltrim(isnull(type,''))) as type from course where college_code='" + collegecode + "' and type is not null and ltrim(ltrim(isnull(type,'')))<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
                ddlstream.Items.Insert(0, "All");
                ddlstream.SelectedIndex = 0;
            }
            else
            {
                ddlstream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void loadedu()
    {
        try
        {
            chklsedu.Items.Clear();
            chkedu.Checked = false;
            txtedu.Text = "---Select---";
            string coursetype = string.Empty;
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            if (ddlstream.Items.Count > 0)
            {
                if (Convert.ToString(ddlstream.SelectedItem).Trim().ToLower() != "all" && Convert.ToString(ddlstream.SelectedItem).Trim().ToLower() != "")
                {
                    coursetype = " and ltrim(rtrim(isnull(type,'')))='" + Convert.ToString(ddlstream.SelectedItem).Trim() + "'";
                }
            }
            string strquery = "select distinct Edu_Level from course where college_code='" + collegecode + "' " + coursetype + "";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsedu.DataSource = ds;
                chklsedu.DataTextField = "Edu_Level";
                chklsedu.DataValueField = "Edu_Level";
                chklsedu.DataBind();
                for (int i = 0; i < chklsedu.Items.Count; i++)
                {
                    chklsedu.Items[i].Selected = true;
                }
                chkedu.Checked = true;
                txtedu.Text = "Edu (" + ds.Tables[0].Rows.Count + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindbatch()
    {
        try
        {
            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds = da.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    Chklst_batch.DataSource = ds;
                    Chklst_batch.DataTextField = "batch_year";
                    Chklst_batch.DataValueField = "batch_year";
                    Chklst_batch.DataBind();
                    for (int i = 0; i < Chklst_batch.Items.Count; i++)
                    {
                        Chklst_batch.Items[i].Selected = true;
                    }
                    if (count > 0)
                    {
                        txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
                        if (Chklst_batch.Items.Count == count)
                        {
                            Chk_batch.Checked = true;
                        }
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

    public void binddegree()
    {
        try
        {
            txt_degree.Text = "---Select---";
            chk_degree.Checked = false;
            Chklst_degree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            string type = string.Empty;
            if (ddlstream.Enabled == true)
            {
                if (ddlstream.Items.Count > 0)
                {
                    if (Convert.ToString(ddlstream.SelectedItem).ToLower().Trim() != "all" && Convert.ToString(ddlstream.SelectedItem).Trim().ToLower() != "")
                    {
                        type = " and ltrim(rtrim(isnull(course.type,'')))='" + Convert.ToString(ddlstream.SelectedItem).Trim() + "'";
                    }
                }
            }
            string codevalues = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]).Trim();
                }
                codevalues = " and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = " and user_code='" + usercode + "'";
            }
            string stredu = string.Empty;
            for (int i = 0; i < chklsedu.Items.Count; i++)
            {
                if (chklsedu.Items[i].Selected == true)
                {
                    if (stredu.Trim() == "")
                    {
                        stredu = "'" + Convert.ToString(chklsedu.Items[i].Text).Trim() + "'";
                    }
                    else
                    {
                        stredu = stredu + ",'" + Convert.ToString(chklsedu.Items[i].Text).Trim() + "'";
                    }
                }
            }
            if (stredu.Trim() != "")
            {
                stredu = " and course.Edu_Level in(" + stredu + ")";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + type + "" + stredu + "";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();
                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    Chklst_degree.Items[h].Selected = true;
                }
                txt_degree.Text = "Degree" + "(" + Chklst_degree.Items.Count + ")";
                chk_degree.Checked = true;
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
            string degreecode = string.Empty;
            txt_branch.Text = "---Select---";
            chk_branch.Checked = false;
            chklst_branch.Items.Clear();
            for (int h = 0; h < Chklst_degree.Items.Count; h++)
            {
                if (Chklst_degree.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = Chklst_degree.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + Chklst_degree.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degreecode, collegecode = Session["collegecode"].ToString(), Session["usercode"].ToString());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();
                    for (int h = 0; h < chklst_branch.Items.Count; h++)
                    {
                        chklst_branch.Items[h].Selected = true;
                    }
                    txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
                    chk_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindsem1()
    {
        try
        {
            chklssem.Items.Clear();
            txtsem.Text = "---Select---";
            chksem.Checked = false;
            int duration = 0;
            int i = 0;
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string degreecode = string.Empty;
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                degreecode = " and degree_code in(" + degreecode + ")";
            }
            string strsql = "select Max(Duration) from Degree where college_code='" + collegecode + "' " + degreecode + " order by Max(Duration) ";
            DataSet ds = da.select_method_wo_parameter(strsql, "TExt");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    chklssem.Items.Add(i.ToString());
                }
                for (int h = 0; h < chklssem.Items.Count; h++)
                {
                    chklssem.Items[h].Selected = true;
                }
                txtsem.Text = "Sem (" + duration + ")";
                chksem.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void loadsubtype()
    {
        try
        {
            chklssubtype.Items.Clear();
            txtsubtype.Text = "---Select---";
            chksubtype.Checked = false;
            string batchyear = string.Empty;
            for (int h = 0; h < Chklst_batch.Items.Count; h++)
            {
                if (Chklst_batch.Items[h].Selected == true)
                {
                    if (batchyear == "")
                    {
                        batchyear = Chklst_batch.Items[h].Value;
                    }
                    else
                    {
                        batchyear = batchyear + ',' + Chklst_batch.Items[h].Value;
                    }
                }
            }
            if (batchyear.Trim() != "")
            {
                batchyear = " and sy.batch_year in(" + batchyear + ")";
            }
            string degreecode = string.Empty;
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                degreecode = " and sy.degree_code in(" + degreecode + ")";
            }
            string semval = string.Empty;
            for (int i = 0; i < chklssem.Items.Count; i++)
            {
                if (chklssem.Items[i].Selected == true)
                {
                    if (semval.Trim() == "")
                    {
                        semval = "'" + chklssem.Items[i].Text.ToString() + "'";
                    }
                    else
                    {
                        semval = semval + ",'" + chklssem.Items[i].Text.ToString() + "'";
                    }
                }
            }
            if (semval.Trim() != "")
            {
                semval = " and sy.semester in(" + semval + ")";
            }
            string strquery = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1";
            strquery = strquery + " " + batchyear + " " + degreecode + " " + semval + "";
            DataSet ds = da.select_method_wo_parameter(strquery, "TExt");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklssubtype.DataSource = ds;
                chklssubtype.DataTextField = "subject_type";
                chklssubtype.DataBind();
                for (int h = 0; h < chklssubtype.Items.Count; h++)
                {
                    chklssubtype.Items[h].Selected = true;
                }
                txtsubtype.Text = "Subject Type (" + ds.Tables[0].Rows.Count + ")";
                chksubtype.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void loadsubject()
    {
        try
        {
            chklssubject.Items.Clear();
            txtsubject.Text = "---Select---";
            chksubject.Checked = false;
            string batchyear = string.Empty;
            for (int h = 0; h < Chklst_batch.Items.Count; h++)
            {
                if (Chklst_batch.Items[h].Selected == true)
                {
                    if (batchyear == "")
                    {
                        batchyear = Chklst_batch.Items[h].Value;
                    }
                    else
                    {
                        batchyear = batchyear + ',' + Chklst_batch.Items[h].Value;
                    }
                }
            }
            if (batchyear.Trim() != "")
            {
                batchyear = " and sy.batch_year in(" + batchyear + ")";
            }
            string degreecode = string.Empty;
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                degreecode = " and sy.degree_code in(" + degreecode + ")";
            }
            string subtype = string.Empty;
            for (int h = 0; h < chklssubtype.Items.Count; h++)
            {
                if (chklssubtype.Items[h].Selected == true)
                {
                    if (subtype == "")
                    {
                        subtype = "'" + chklssubtype.Items[h].Text + "'";
                    }
                    else
                    {
                        subtype = subtype + ",'" + chklssubtype.Items[h].Text + "'";
                    }
                }
            }
            if (subtype.Trim() != "")
            {
                subtype = " and ss.subject_type in(" + subtype + ")";
            }
            string semval = string.Empty;
            int semcc = 0;
            string semcccc = string.Empty;
            for (int i = 0; i < chklssem.Items.Count; i++)
            {
                if (chklssem.Items[i].Selected == true)
                {
                    semcc++;
                    semcccc = chklssem.Items[i].Text.ToString();
                    if (semval.Trim() == "")
                    {
                        semval = "'" + chklssem.Items[i].Text.ToString() + "'";
                    }
                    else
                    {
                        semval = semval + ",'" + chklssem.Items[i].Text.ToString() + "'";
                    }
                }
            }
            Span5.InnerHtml = string.Empty;
            if (semcc == 1)
            {
                Span5.InnerHtml = semcccc;
            }
            if (semval.Trim() != "")
            {
                semval = " and sy.semester in(" + semval + ")";
            }
            string strquery = "select distinct ss.subject_type,s.subject_code,s.subject_name,s.subject_code+' - '+s.subject_name as subname from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1";
            strquery = strquery + " " + batchyear + " " + degreecode + " " + semval + " " + subtype + " order by ss.subject_type,s.subject_name,s.subject_code";
            DataSet ds = da.select_method_wo_parameter(strquery, "TExt");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklssubject.DataSource = ds;
                chklssubject.DataTextField = "subname";
                chklssubject.DataValueField = "subject_code";
                chklssubject.DataBind();
                for (int h = 0; h < chklssubject.Items.Count; h++)
                {
                    chklssubject.Items[h].Selected = true;
                }
                txtsubject.Text = "Subject (" + ds.Tables[0].Rows.Count + ")";
                chksubject.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void clear()
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnmasterprint.Visible = false; btnPrint.Visible = false;
        FpSpread1.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadedu();
        bindbatch();
        binddegree();
        bindbranch();
        bindsem1();
        loadsubtype();
        loadsubject();
    }

    protected void chkedu_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkedu.Checked == true)
            {
                for (int i = 0; i < chklsedu.Items.Count; i++)
                {
                    chklsedu.Items[i].Selected = true;
                }
                txtedu.Text = "Edu (" + (chklsedu.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsedu.Items.Count; i++)
                {
                    chklsedu.Items[i].Selected = false;
                }
                txtedu.Text = "--Select--";
            }
            bindbatch();
            binddegree();
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chklsedu_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtedu.Text = "--Select--";
            int seatcount = 0;
            chkedu.Checked = false;
            for (int i = 0; i < chklsedu.Items.Count; i++)
            {
                if (chklsedu.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount > 0)
            {
                txtedu.Text = "Edu (" + seatcount.ToString() + ")";
                if (seatcount == chklsedu.Items.Count)
                {
                    chkedu.Checked = true;
                }
            }
            bindbatch();
            binddegree();
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (Chk_batch.Checked == true)
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
            binddegree();
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_batch.Text = "--Select--";
            int seatcount = 0;
            Chk_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = Chklst_batch.Items[i].Value.ToString();
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
            if (seatcount > 0)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                if (seatcount == Chklst_batch.Items.Count)
                {
                    Chk_batch.Checked = true;
                }
            }
            binddegree();
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_degree.Text = "--Select--";
            int seatcount = 0;
            chk_degree.Checked = false;
            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_degree.Text = "--Select--";
                    build = Chklst_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + build;
                    }
                }
            }
            if (seatcount > 0)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                if (seatcount == Chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string Chklstbatchvalue = string.Empty;
            string bind1 = string.Empty;
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    if (chk_degree.Checked == true)
                    {
                        Chklst_degree.Items[i].Selected = true;
                        bind1 = Chklst_degree.Items[i].Value.ToString();
                        if (Chklstbatchvalue == "")
                        {
                            Chklstbatchvalue = bind1;
                        }
                        else
                        {
                            Chklstbatchvalue = Chklstbatchvalue + "," + bind1;
                        }
                    }
                }
                txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                    Chklst_degree.ClearSelection();
                }
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
                chk_branch.Checked = false;
            }
            bindbranch();
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            int seatcount = 0;
            chk_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount > 0)
            {
                txt_branch.Text = "Branch(" + seatcount.ToString() + ")";
                if (seatcount == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
            bindsem1();
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chksem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chksem.Checked == true)
            {
                for (int i = 0; i < chklssem.Items.Count; i++)
                {
                    chklssem.Items[i].Selected = true;
                }
                txtsem.Text = "Sem (" + (chklssem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklssem.Items.Count; i++)
                {
                    chklssem.Items[i].Selected = false;
                }
                txtsem.Text = "--Select--";
            }
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chklssem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int seatcount = 0;
            chksem.Checked = false;
            txtsem.Text = "--Select--";
            int semcc = 0;
            string semccc = string.Empty;
            for (int i = 0; i < chklssem.Items.Count; i++)
            {
                if (chklssem.Items[i].Selected == true)
                {
                    semccc = chklssem.Items[i].Text.ToString();
                    semcc++;
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount > 0)
            {
                txtsem.Text = "Sem (" + seatcount.ToString() + ")";
                if (seatcount == chklssem.Items.Count)
                {
                    chksem.Checked = true;
                }
            }
            Span5.InnerHtml = string.Empty;
            if (semcc == 1)
            {
                Span5.InnerHtml = semccc;
            }
            loadsubtype();
            loadsubject();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    //protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    loadsubtype();
    //    loadsubject();
    //}

    protected void chksubtype_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chksubtype.Checked == true)
        {
            for (int i = 0; i < chklssubtype.Items.Count; i++)
            {
                chklssubtype.Items[i].Selected = true;
            }
            txtsubtype.Text = "Subject Type (" + (chklssubtype.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklssubtype.Items.Count; i++)
            {
                chklssubtype.Items[i].Selected = false;
            }
            txtsubtype.Text = "--Select--";
        }
        loadsubject();
    }

    protected void chklssubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int seatcount = 0;
        chksubtype.Checked = false;
        txtsubtype.Text = "--Select--";
        for (int i = 0; i < chklssubtype.Items.Count; i++)
        {
            if (chklssubtype.Items[i].Selected == true)
            {
                seatcount = seatcount + 1;
            }
        }
        if (seatcount > 0)
        {
            txtsubtype.Text = "Subject Type (" + seatcount.ToString() + ")";
            if (seatcount == chklssubtype.Items.Count)
            {
                chksubtype.Checked = true;
            }
        }
        loadsubject();
    }

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject (" + (chklssubject.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = false;
            }
            txtsubject.Text = "--Select--";
        }
    }

    protected void chklssubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int seatcount = 0;
        chksubject.Checked = false;
        txtsubject.Text = "--Select--";
        for (int i = 0; i < chklssubject.Items.Count; i++)
        {
            if (chklssubject.Items[i].Selected == true)
            {
                seatcount = seatcount + 1;
            }
        }
        if (seatcount > 0)
        {
            txtsubject.Text = "Subject (" + seatcount.ToString() + ")";
            if (seatcount == chklssubject.Items.Count)
            {
                chksubject.Checked = true;
            }
        }
    }

    protected void Radiochange(object sender, EventArgs e)
    {
        clear();
    }

    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "Rank List Report";
        string pagename = "SubjectWiseExternalRanklist.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreport(FpSpread1, reportname);
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

    protected void btngo1(object sender, EventArgs e)
    {
        try
        {
            clear();
            string getrollno = string.Empty;
            string batchyear = string.Empty;
            string batchyeartt = string.Empty;
            int batchcount = 0;
            for (int h = 0; h < Chklst_batch.Items.Count; h++)
            {
                if (Chklst_batch.Items[h].Selected == true)
                {
                    batchcount++;
                    //Span1.InnerHtml = Chklst_batch.Items[h].Text;
                    if (batchyear == "")
                    {
                        batchyeartt = Chklst_batch.Items[h].Value;
                        batchyear = "'" + Chklst_batch.Items[h].Value + "'";
                    }
                    else
                    {
                        batchyeartt = batchyeartt + "," + Chklst_batch.Items[h].Value;
                        batchyear += ",'" + Chklst_batch.Items[h].Value + "'";
                    }
                }
            }
            string degreecode = string.Empty;
            int degreecount = 0;
            string degreename = string.Empty;
            ArrayList avoidsamedegree = new ArrayList();
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (!avoidsamedegree.Contains(chklst_branch.Items[h].Text.Trim().ToLower()))
                    {
                        degreecount++;
                        avoidsamedegree.Add(chklst_branch.Items[h].Text.Trim().ToLower());
                    }
                    if (degreecode == "")
                    {
                        degreename = chklst_branch.Items[h].Text;
                        degreecode = "'" + chklst_branch.Items[h].Value + "'";
                    }
                    else
                    {
                        degreecode += ",'" + chklst_branch.Items[h].Value + "'";
                    }
                }
            }
            if (batchyear.Trim() != "")
            {
                //getrollno = "  SELECT STUFF((SELECT ''',''' + convert(nvarchar(max),[Roll_No])  FROM Registration sy   where sy.batch_year in(" + batchyear + ") and sy.degree_code in (" + degreecode + ") ";
                getrollno = "  SELECT STUFF((SELECT ''',''' + convert(nvarchar(max),[Roll_No])  FROM Registration sy   where sy.batch_year in(" + batchyear + ") and sy.degree_code in (" + degreecode + ") ";
                getrollno = getrollno + "and sy.degree_code in(" + degreecode + ")    FOR XML PATH('')),1,1,'''') as [Roll_No]";
                Span1.InnerHtml = batchyeartt;
                batchyear = " and sy.batch_year in(" + batchyear + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Batch Year And Then Proceed";
                return;
            }
            Span2.InnerHtml = string.Empty;
            co2.InnerHtml = string.Empty;
            if (degreecount == 1)
            {
                Span3.InnerHtml = degreename;
            }
            else
            {
                Span3.InnerHtml = "ALL BRANCHES";
            }
            Span5.InnerHtml = "1";
            if (Span5.InnerHtml.Trim() == "")
            {
                Span4.InnerHtml = "Semester";
                co4.InnerHtml = string.Empty;
            }
            else
            {
                Span4.InnerHtml = "Semester";
                co4.InnerHtml = ":";
            }
            if (batchcount == 1 && Span5.InnerHtml.Trim() != "")
            {
                string edulevel = da.GetFunctionv("select Edu_Level from Course where Course_Id in (select Course_Id from Degree where Degree_Code in (" + degreecode + "))");
                Span1.InnerHtml = edulevel + " - " + batchyeartt;
                //  string currentsem = da.GetFunctionv("select top 1 current_semester from registration where degree_code='" + degreecode + "' and batch_year='" + batchyeartt + "' and DelFlag=0 and Exam_Flag<>'debar' and cc=0");
                string currentsem = Span5.InnerHtml;
                string examno = da.GetFunctionv("select (CONVERT(nvarchar(50), Exam_Month)+' - '+CONVERT(nvarchar(50), Exam_year)) from Exam_Details where batch_year=" + batchyeartt + " and current_semester='" + currentsem + "'");
                string[] spitmm = examno.Split('-');
                if (spitmm.Length == 2)
                {
                    string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(spitmm[0].ToString()));
                    strMonthName = strMonthName + " / " + spitmm[1].ToString();
                    Span6.InnerHtml = "Month & Year";
                    Span10.InnerHtml = ":";
                    Span11.InnerHtml = strMonthName;
                }
            }
            else
            {
                Span6.InnerHtml = string.Empty;
                Span10.InnerHtml = string.Empty;
                Span11.InnerHtml = string.Empty;
            }
            if (degreecode.Trim() != "")
            {
                degreecode = " and sy.degree_code in(" + degreecode + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Degree and Branch And Then Proceed";
                return;
            }
            string subtype = string.Empty;
            for (int h = 0; h < chklssubtype.Items.Count; h++)
            {
                if (chklssubtype.Items[h].Selected == true)
                {
                    if (chklssubtype.Items[h].Text.Trim().ToUpper() == "PART III")
                    {
                        Span9.InnerHtml = "MAJOR AND ALLIED";
                    }
                    else
                    {
                        Span9.InnerHtml = string.Empty;
                    }
                    if (subtype == "")
                    {
                        subtype = "'" + chklssubtype.Items[h].Text + "'";
                    }
                    else
                    {
                        subtype = subtype + ",'" + chklssubtype.Items[h].Text + "'";
                    }
                }
            }
            if (subtype.Trim() != "")
            {
                subtype = " and ss.subject_type in(" + subtype + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Subject Type And Then Proceed";
                return;
            }
            string subject = string.Empty;
            string subjectname = string.Empty;
            int subcount = 0;
            for (int h = 0; h < chklssubject.Items.Count; h++)
            {
                if (chklssubject.Items[h].Selected == true)
                {
                    subcount++;
                    subjectname = chklssubject.Items[h].Text;
                    if (subject == "")
                    {
                        subject = "'" + chklssubject.Items[h].Value + "'";
                    }
                    else
                    {
                        subject = subject + ",'" + chklssubject.Items[h].Value + "'";
                    }
                }
            }
            string getsubject_code = string.Empty;
            if (subtype.Trim() != "")
            {
                getsubject_code = da.GetFunctionv("	 SELECT STUFF((SELECT ',' + convert(nvarchar(max),[subject_no])  FROM subject sy   where sy.subject_code in (" + subject + ")   FOR XML PATH('')),1,1,'') as [subject_no]");
                if (subcount == 1)
                {
                    string subjectnamecoe = da.GetFunctionv("select subject_name from subject where subject_code=" + subject + "");
                    Span9.InnerHtml = subjectnamecoe.ToUpper();
                }
                subject = " and s.subject_code in(" + subject + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Subject Type And Then Proceed";
                return;
            }
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 8;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Register No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Name of the Student";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Average";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rank";
            FpSpread1.Sheets[0].Columns[0].Width = 30;
            FpSpread1.Sheets[0].Columns[1].Width = 250;
            FpSpread1.Sheets[0].Columns[2].Width = 100;
            FpSpread1.Sheets[0].Columns[3].Width = 100;
            FpSpread1.Sheets[0].Columns[4].Width = 100;
            FpSpread1.Sheets[0].Columns[5].Width = 150;
            FpSpread1.Sheets[0].Columns[6].Width = 60;
            FpSpread1.Sheets[0].Columns[6].Width = 30;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].Visible = false;
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            if (Session["Rollflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (Session["Studflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Name = "Book Antiqua";
            style1.Font.Bold = false;
            FpSpread1.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            string strwitharrear = "and r.Roll_No not in(select m1.roll_no from mark_entry m1 where m.exam_code=m1.exam_code and m.subject_no=m1.subject_no and m.roll_no=m1.roll_no and m1.result<>'pass')";
            if (rbwoarrear.Checked == true)
            {
                strwitharrear = string.Empty;
            }
            int topval = 0;
            if (txt_top.Text.ToString().Trim() != "")
            {
                // topval = "  top " + txt_top.Text.ToString().Trim() + "";
                topval = Convert.ToInt32(txt_top.Text.ToString().Trim());
            }
            string markval = string.Empty;
            double percen = 0;
            markval = string.Empty;
            getrollno = da.GetFunctionv(getrollno);
            getrollno = getrollno.Remove(0, 2);
            string sqladde = string.Empty;
            getrollno = getrollno + "'";
            getrollno = " SELECT STUFF((SELECT ''',''' + convert(nvarchar(max),[Roll_No])  FROM mark_entry sy   where  sy.roll_no in (" + getrollno + ") and sy.subject_no in (" + getsubject_code + ") and sy.result in ('Fail','AAA')   FOR XML PATH('')),1,1,'''') as [Roll_No]";
            getrollno = da.GetFunctionv(getrollno);
            if (getrollno != "")
            {
                getrollno = getrollno.Remove(0, 2);
                getrollno = getrollno + "'";
                sqladde = sqladde + " and m.roll_no not in (" + getrollno + ") ";
            }
            //  getrollno = "       SELECT STUFF((SELECT ''',''' + convert(nvarchar(max),[Roll_No])  FROM mark_entry sy   where  sy.roll_no in (" + getrollno + ") and sy.result='Fail'   FOR XML PATH('')),1,1,'''') as [Roll_No]";
            string strquery = "select dense_rank() over(order by (sum(m.total)/count(m.subject_no)) desc) as rank,r.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,r.Roll_No,r.Reg_No,r.Stud_Type,r.Stud_Name,(sum(m.total)/count(m.subject_no)) as mark from Registration r,mark_entry m,syllabus_master sy,sub_sem ss,subject s,Degree d,Department de,Course c ";
            strquery = strquery + " where r.Roll_No=m.roll_no and m.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and s.syll_code=sy.syll_code and s.syll_code=ss.syll_code and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code ";
            // strquery = strquery + " " + strwitharrear + " "+markval+" and m.result='Pass' " + batchyear + " " + degreecode + " and sy.semester='" + ddlsem.Text.ToString() + "' " + subject + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,r.Roll_No,r.Reg_No,r.Stud_Type,r.Stud_Name order by rank";
            strquery = strquery + " " + strwitharrear + " " + markval + " and m.result='Pass' " + sqladde + " " + batchyear + " " + degreecode + "  " + subject + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,r.Roll_No,r.Reg_No,r.Stud_Type,r.Stud_Name order by rank";
            //if (rbsubtype.Checked == true)
            //{
            //    strquery = "select dense_rank() over(order by (sum(m.total)/count(m.subject_no)) desc) as rank,r.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,r.Roll_No,r.Reg_No,r.Stud_Type,r.Stud_Name,ss.subject_type,(sum(m.total)/count(m.subject_no)) as mark from Registration r,mark_entry m,syllabus_master sy,sub_sem ss,subject s,Degree d,Department de,Course c ";
            //    strquery = strquery + " where r.Roll_No=m.roll_no and m.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and s.syll_code=sy.syll_code and s.syll_code=ss.syll_code and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code ";
            //    strquery = strquery + " " + strwitharrear + " " + markval + " and m.result='Pass' " + batchyear + " " + degreecode + " and sy.semester='" + ddlsem.Text.ToString() + "' " + subtype + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,r.Roll_No,r.Reg_No,r.Stud_Type,r.Stud_Name,ss.subject_type order by sum(m.total) desc";
            //}
            markval = txtminimunpercent.Text.ToString();
            if (markval.Trim() != "")
            {
                percen = Convert.ToDouble(markval);
            }
            DataSet ds = da.select_method_wo_parameter(strquery, "Text");
            DataView dv = new DataView();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (markval.Trim() != "")
                {
                    ds.Tables[0].DefaultView.RowFilter = "mark>='" + markval + "'";
                    dv = ds.Tables[0].DefaultView;
                }
                else
                {
                    dv = ds.Tables[0].DefaultView;
                }
            }
            if (dv.Count > 0)
            {
                if (topval == 0)
                {
                    topval = dv.Count;
                }
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = true;
                btnPrint.Visible = true; btnmasterprint.Visible = true;
                int srno = 0;
                for (int r = 0; r < dv.Count; r++)
                {
                    string rank = ds.Tables[0].Rows[r]["rank"].ToString();
                    if (topval >= Convert.ToInt32(rank))
                    {
                        string rollno = dv[r]["Roll_No"].ToString();
                        string regno = dv[r]["Reg_No"].ToString();
                        string name = dv[r]["Stud_Name"].ToString();
                        string subtyp = dv[r]["Stud_Type"].ToString();
                        string batch = dv[r]["Batch_Year"].ToString();
                        string degree = dv[r]["Course_Name"].ToString();
                        string dept = dv[r]["Dept_Name"].ToString();
                        string mark = dv[r]["mark"].ToString();
                        if (mark.Trim() != "")
                        {
                            mark = Convert.ToString(Math.Round(Convert.ToDouble(mark), 2, MidpointRounding.AwayFromZero));
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        srno++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batch + " - " + degree + " - " + dept;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = subtyp;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = name;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = mark.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = rank;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height = FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height + 10;
                    }
                }
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Records Found";
            }
            FpSpread1.SaveChanges();
            FpSpread1.Width = 900;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

}