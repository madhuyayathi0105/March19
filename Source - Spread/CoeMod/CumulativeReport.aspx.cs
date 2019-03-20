using System;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class CumulativeReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = string.Empty;

    string college = string.Empty;
    string batch = string.Empty;
    string degree = string.Empty;
    string dept = string.Empty;
    string subtype = string.Empty;
    string subname = string.Empty;

    string fromsem = String.Empty;
    string tosem = String.Empty;

    int i, row, commcount;

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    Hashtable hat = new Hashtable();

    DataTable chartdata = new DataTable();

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindclg();
                bindBtch();
                binddeg();
                binddept();
                bindSubtype();
                bindSubname();

            }
        }
        catch
        {
        }
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBtch();
            binddeg();
            binddept();
            bindSubtype();
            bindSubname();
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            //txt_batch.Text = "--Select--";
            //if (cb_batch.Checked == true)
            //{

            //    for (i = 0; i < cbl_batch.Items.Count; i++)
            //    {
            //        cbl_batch.Items[i].Selected = true;
            //    }
            //    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            //}
            //else
            //{
            //    for (i = 0; i < cbl_batch.Items.Count; i++)
            //    {
            //        cbl_batch.Items[i].Selected = false;
            //    }
            //}

            //binddeg();
            //binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_batch.Checked = false;
            //commcount = 0;
            //txt_batch.Text = "--Select--";
            //for (i = 0; i < cbl_batch.Items.Count; i++)
            //{
            //    if (cbl_batch.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_batch.Items.Count)
            //    {
            //        cb_batch.Checked = true;
            //    }
            //    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            //}
            //binddeg();
            //binddept();
        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_degree.Checked = false;
            //commcount = 0;
            //txt_degree.Text = "--Select--";
            //for (i = 0; i < cbl_degree.Items.Count; i++)
            //{
            //    if (cbl_degree.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_degree.Items.Count)
            //    {
            //        cb_degree.Checked = true;
            //    }
            //    txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            //}
            //binddept();
        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_dept.Text = "--Select--";
            //if (cb_dept.Checked == true)
            //{

            //    for (i = 0; i < cbl_dept.Items.Count; i++)
            //    {
            //        cbl_dept.Items[i].Selected = true;
            //    }
            //    txt_dept.Text = "Deartment(" + (cbl_dept.Items.Count) + ")";
            //}
            //else
            //{
            //    for (i = 0; i < cbl_dept.Items.Count; i++)
            //    {
            //        cbl_dept.Items[i].Selected = false;
            //    }
            //}

        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_dept.Checked = false;
            //commcount = 0;
            //txt_dept.Text = "--Select--";
            //for (i = 0; i < cbl_dept.Items.Count; i++)
            //{
            //    if (cbl_dept.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_dept.Items.Count)
            //    {
            //        cb_dept.Checked = true;
            //    }
            //    txt_dept.Text = "Department(" + commcount.ToString() + ")";
            //}
        }
        catch { }
    }
    protected void cb__subtype_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subtype.Text = "--Select--";
            if (cb_subtype.Checked == true)
            {

                for (i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    cbl_subtype.Items[i].Selected = true;
                }
                txt_subtype.Text = "Subject Type(" + (cbl_subtype.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    cbl_subtype.Items[i].Selected = false;
                }
            }
            bindSubname();
        }
        catch { }
    }
    protected void cbl_subtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_subtype.Checked = false;
            commcount = 0;
            txt_subtype.Text = "--Select--";
            for (i = 0; i < cbl_subtype.Items.Count; i++)
            {
                if (cbl_subtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_subtype.Items.Count)
                {
                    cb_subtype.Checked = true;
                }
                txt_subtype.Text = "Subject Type(" + commcount.ToString() + ")";
            }
            bindSubname();
        }
        catch { }
    }
    protected void cb_subname_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subname.Text = "--Select--";
            if (cb_subname.Checked == true)
            {

                for (i = 0; i < cbl_subname.Items.Count; i++)
                {
                    cbl_subname.Items[i].Selected = true;
                }
                txt_subname.Text = "Subject Name(" + (cbl_subname.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_subname.Items.Count; i++)
                {
                    cbl_subname.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void cbl_subname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_subname.Checked = false;
            commcount = 0;
            txt_subname.Text = "--Select--";
            for (i = 0; i < cbl_subname.Items.Count; i++)
            {
                if (cbl_subname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_subname.Items.Count)
                {
                    cb_subname.Checked = true;
                }
                txt_subname.Text = "Subject Name(" + commcount.ToString() + ")";
            }
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            college = "";
            batch = "";
            degree = "";
            dept = "";
            subtype = "";
            subname = "";
            fromsem = "";
            tosem = "";

            Chart1.Series.Clear();
            Chart1.Visible = false;

            if (ddl_college.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_degree.Items.Count > 0 && ddl_dept.Items.Count > 0)
            {
                college = Convert.ToString(ddl_college.SelectedValue);
                batch = Convert.ToString(ddl_batch.SelectedValue);
                degree = Convert.ToString(ddl_degree.SelectedValue);
                dept = Convert.ToString(ddl_dept.SelectedValue);

                fromsem = Convert.ToString(txt_semfrom.Text.Trim());
                tosem = Convert.ToString(txt_semto.Text.Trim());

                ArrayList listsem = new ArrayList();
                for (row = Convert.ToInt32(fromsem); row <= Convert.ToInt32(tosem); row++)
                {
                    listsem.Add(row);
                }

                subtype = "";
                if (cbl_subtype.Items.Count > 0)
                {
                    for (i = 0; i < cbl_subtype.Items.Count; i++)
                    {
                        if (cbl_subtype.Items[i].Selected == true)
                        {
                            if (subtype == "")
                            {
                                subtype = Convert.ToString(cbl_subtype.Items[i].Text);
                            }
                            else
                            {
                                subtype = subtype + "','" + Convert.ToString(cbl_subtype.Items[i].Text);
                            }
                        }
                    }
                }

                //subname = "";
                //if (cbl_subname.Items.Count > 0)
                //{
                //    for (i = 0; i < cbl_subname.Items.Count; i++)
                //    {
                //        if (cbl_subname.Items[i].Selected == true)
                //        {
                //            if (subname == "")
                //            {
                //                subname = Convert.ToString(cbl_subname.Items[i].Value);
                //            }
                //            else
                //            {
                //                subname = subname + "," + Convert.ToString(cbl_subname.Items[i].Value);
                //            }
                //        }
                //    }
                //}

                if (subtype != "")
                {
                    selectQuery = "select ed.Exam_Month,ed.Exam_year,r.Batch_Year,r.degree_code,sy.semester,ss.subject_type,ss.subType_no,count(distinct r.roll_no) as   appear from Registration r,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Exam_Details ed where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and m.subject_no=s.subject_no and m.roll_no=r.Roll_No and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and m.exam_code=ed.exam_code and r.Batch_Year='" + batch + "' and r.degree_code='" + dept + "' and r.DelFlag=0 and ss.subject_type in('" + subtype + "') and sy.semester between '" + fromsem + "' and '" + tosem + "' group by ed.Exam_Month,Exam_year,r.Batch_Year,r.degree_code,sy.semester,ss.subject_type,ss.subType_no order by r.Batch_Year,r.degree_code,ed.Exam_year,ed.Exam_Month ";

                    selectQuery += "select ed.Exam_Month,ed.Exam_year,r.Batch_Year,r.degree_code,sy.semester,ss.subject_type,ss.subType_no,count(distinct r.roll_no) as     fail from Registration r,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Exam_Details ed where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and m.subject_no=s.subject_no and m.roll_no=r.Roll_No and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and m.exam_code=ed.exam_code and r.Batch_Year='" + batch + "' and r.degree_code='" + dept + "' and r.DelFlag=0 and ss.subject_type in('" + subtype + "') and sy.semester between '" + fromsem + "' and '" + tosem + "' and m.result<>'Pass' group by ed.Exam_Month,Exam_year,r.Batch_Year,r.degree_code,sy.semester,ss.subject_type,ss.subType_no order by r.Batch_Year,r.degree_code,ed.Exam_year,ed.Exam_Month ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQuery, "Text");

                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (row = Convert.ToInt32(fromsem); row <= Convert.ToInt32(tosem); row++)
                        {
                            chartdata.Columns.Add(romanLetter(Convert.ToString(row)));
                        }

                        DataRow dr;
                        //Before chartdata loaded 
                        for (row = 0; row < cbl_subtype.Items.Count; row++)
                        {
                            if (cbl_subtype.Items[row].Selected == true)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "subject_type='" + cbl_subtype.Items[row].Text + "'";
                                DataView dv = ds.Tables[0].DefaultView;

                                ds.Tables[1].DefaultView.RowFilter = "subject_type='" + cbl_subtype.Items[row].Text + "'";
                                DataView dv1 = ds.Tables[1].DefaultView;

                                if (dv.Count > 0)
                                {
                                    Chart1.Series.Add(Convert.ToString(cbl_subtype.Items[row].Text));
                                    Chart1.Series[0].BorderWidth = 2;

                                    dr = chartdata.NewRow();
                                    foreach (int index in listsem)
                                    {
                                        dv.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                        dv1.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                        double percent = 0;
                                        double fail = 0;
                                        double total = 0;

                                        if (dv.Count > 0)
                                        {
                                            DataTable dt_new = dv.ToTable();
                                            DataTable dt_new1 = dv1.ToTable();

                                            string ap = Convert.ToString(dt_new.Rows[0]["appear"]);
                                            if (ap.Trim() != "")
                                            {
                                                total = Convert.ToInt32(ap);
                                            }
                                            if (dv1.Count > 0)
                                            {
                                                string fai = Convert.ToString(dt_new1.Rows[0]["fail"]);
                                                if (fai.Trim() != "")
                                                {
                                                    fail = Convert.ToInt32(fai);
                                                }
                                            }

                                            percent = Math.Round(((total - fail) / total) * 100, 2);

                                            dr[listsem.IndexOf(index)] = Convert.ToString(percent);
                                        }
                                    }
                                    chartdata.Rows.Add(dr);

                                }

                            }
                        }

                        //After chartdata loaded
                        if (chartdata.Rows.Count > 0)
                        {
                            for (int chart_i = 0; chart_i < chartdata.Columns.Count; chart_i++)
                            {
                                for (int chart_j = 0; chart_j < chartdata.Rows.Count; chart_j++)
                                {
                                    string subnncode = Convert.ToString(chartdata.Columns[chart_i]);
                                    string m1 = chartdata.Rows[chart_j][chart_i].ToString();
                                    Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                    Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                    Chart1.Series[chart_j].IsXValueIndexed = true;
                                }
                            }
                            Chart1.Visible = true;
                        }
                    }
                }

            }
        }
        catch
        {
        }
        finally
        {
            if (Chart1.Visible)
            {
                lbl_error.Visible = false;
            }
            else
            {
                lbl_error.Text = "No Records Found";
                lbl_error.Visible = true;
            }
        }
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();

            }


        }
        catch { }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            if (batch != "")
            {
                ds.Clear();
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_degree.DataSource = ds;
                    ddl_degree.DataTextField = "course_name";
                    ddl_degree.DataValueField = "course_id";
                    ddl_degree.DataBind();

                }
            }
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            ddl_dept.Items.Clear();
            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "dept_name";
                    ddl_dept.DataValueField = "degree_code";
                    ddl_dept.DataBind();
                }
            }
        }
        catch { }
    }
    public void bindSubtype()
    {
        try
        {
            txt_subtype.Text = "---Select---";
            cbl_subtype.Items.Clear();
            cb_subtype.Checked = false;
            subtype = "";

            batch = "";
            if (ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            }

            dept = "";
            if (ddl_dept.Items.Count > 0)
            {
                dept = Convert.ToString(ddl_dept.SelectedValue.ToString());
            }

            if (batch != "" && dept != "")
            {
                //selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.current_semester=s.semester and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";
                selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (subtype == "")
                        {
                            subtype = Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                        else
                        {
                            subtype = subtype + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                    }

                    DataSet ds2 = new DataSet();
                    //string selectQuery1 = "select distinct subject_type,subType_no from sub_sem where syll_code in ('" + subtype + "')";
                    string selectQuery1 = "select distinct subject_type from sub_sem where syll_code in ('" + subtype + "')";
                    ds2.Clear();
                    ds2 = d2.select_method_wo_parameter(selectQuery1, "Text");

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        cbl_subtype.DataSource = ds2;
                        cbl_subtype.DataTextField = "subject_type";
                        cbl_subtype.DataValueField = "subject_type";
                        cbl_subtype.DataBind();

                        if (cbl_subtype.Items.Count > 0)
                        {
                            for (i = 0; i < cbl_subtype.Items.Count; i++)
                            {
                                cbl_subtype.Items[i].Selected = true;
                            }
                            txt_subtype.Text = "Subject Type(" + cbl_subtype.Items.Count + ")";
                            cb_subtype.Checked = true;
                        }
                    }

                }
            }
        }
        catch { }
    }
    public void bindSubname()
    {
        try
        {
            txt_subname.Text = "---Select---";
            cbl_subname.Items.Clear();
            cb_subname.Checked = false;
            subname = "";

            batch = "";
            if (ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            }

            dept = "";
            if (ddl_dept.Items.Count > 0)
            {
                dept = Convert.ToString(ddl_dept.SelectedValue.ToString());
            }


            subtype = "";
            if (cbl_subtype.Items.Count > 0)
            {
                for (i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    if (cbl_subtype.Items[i].Selected == true)
                    {
                        if (subtype == "")
                        {
                            subtype = Convert.ToString(cbl_subtype.Items[i].Value);
                        }
                        else
                        {
                            subtype = subtype + "," + Convert.ToString(cbl_subtype.Items[i].Value);
                        }
                    }
                }
            }

            if (batch != "" && dept != "" && subtype != "")
            {

                selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.current_semester=s.semester and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    subname = "";
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (subname == "")
                        {
                            subname = Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                        else
                        {
                            subname = subname + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                    }

                    DataSet ds2 = new DataSet();
                    string selectQuery1 = "select distinct subject_name from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in (" + dept + ") and registration.batch_year in ('" + batch + "') and subject.syll_code in ('" + subname + "') and sub_sem.subType_no in (" + subtype + ") order by subject_name";
                    ds2.Clear();
                    ds2 = d2.select_method_wo_parameter(selectQuery1, "Text");

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        cbl_subname.DataSource = ds2;
                        cbl_subname.DataTextField = "subject_name";
                        cbl_subname.DataValueField = "subject_name";
                        cbl_subname.DataBind();

                        if (cbl_subname.Items.Count > 0)
                        {
                            for (i = 0; i < cbl_subname.Items.Count; i++)
                            {
                                cbl_subname.Items[i].Selected = true;
                            }
                            txt_subname.Text = "Subject Name(" + cbl_subname.Items.Count + ")";
                            cb_subname.Checked = true;
                        }
                    }

                }

            }
        }
        catch { }
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "Semester - I";
                    break;
                case "2":
                    romanLettervalue = "Semester - II";
                    break;
                case "3":
                    romanLettervalue = "Semester - III";
                    break;
                case "4":
                    romanLettervalue = "Semester - IV";
                    break;
                case "5":
                    romanLettervalue = "Semester - V";
                    break;
                case "6":
                    romanLettervalue = "Semester - VI";
                    break;
                case "7":
                    romanLettervalue = "Semester - VII";
                    break;
                case "8":
                    romanLettervalue = "Semester - VIII";
                    break;
                case "9":
                    romanLettervalue = "Semester - IX";
                    break;
                case "10":
                    romanLettervalue = "Semester - X";
                    break;

            }
        }
        return romanLettervalue;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void ddl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        bindSubtype();
        bindSubname();
    }
    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
        bindSubtype();
        bindSubname();
    }
    protected void ddl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindSubtype();
        bindSubname();
    }
}