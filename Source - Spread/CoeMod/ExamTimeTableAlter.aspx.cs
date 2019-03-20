using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Collections.Generic;
using System.Configuration;

public partial class ExamTimeTableAlter : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    string CollegeCode = string.Empty;
    string newcollegeCode = string.Empty;
    Boolean cellfalsg = false;

    string qrycollege = string.Empty;
    int selcollege = 0;
    Hashtable hat = new Hashtable();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    InsproDirectAccess dir = new InsproDirectAccess();

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
            txtpdate.Attributes.Add("Readonly", "Readonly");
            CollegeCode = Session["collegecode"].ToString();
            lblerror.Visible = false;
            lblperror.Visible = false;
            //magesh 2/2/18
            TextBox1.Visible = false;//magesh 2/2/18
            if (!IsPostBack)
            {
                loadedate();
                ddlBatchYear.Enabled = false;
                ddlSubjectName.Enabled = false;
                ddlDepartment.Enabled = false;
                ddlCourse.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                Fptimetable.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                treepanel.Visible = false;
                //magesh 2/2/18
                TextBox1.Visible = false;//magesh 2/2/18
                chckdeletesubjects.Visible = false;
                lblmedate.Visible = false;
                ddlmedate.Visible = false;
                lblmesession.Visible = false;
                ddlmesession.Visible = false;
                btnmemove.Visible = false;
                Bindcollege();
                chkCollege.Checked = false;
                txtCollege.Text = "--Select--";
                txtCollege.Enabled = false;

                txtpdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                ddltype.Items.Clear();
                string strtypequery = "select distinct type from course where isnull(type,'')>''";
                DataSet dstype = da.select_method_wo_parameter(strtypequery, "text");
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    ddltype.DataSource = dstype;
                    ddltype.DataTextField = "type";
                    ddltype.DataBind();

                    ddltype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
                }
                else
                {
                    ddltype.Enabled = false;
                }

                loadyear();
                if (ddlYear.Items.Count > 0)
                {
                    loadmonth();
                    if (ddlMonth.Items.Count > 0)
                    {
                        loadbatch();
                        loaddegree();
                        loaddepartment();
                        loadsubject();

                        btnView.Enabled = true;
                        DateTime now = DateTime.Now;
                        txtFromDate.Text = now.Date.ToString("dd/MM/yyyy");
                        txtToDate.Text = now.Date.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        cbBatchYear.Visible = false;
                        cbCourse.Visible = false;
                        cbDate.Visible = false;
                        cbDepartment.Visible = false;
                        cbSubject.Visible = false;
                        ddlBatchYear.Visible = false;
                        ddlCourse.Visible = false;
                        ddlDepartment.Visible = false;
                        ddlSubjectName.Visible = false;
                        txtFromDate.Visible = false;
                        txtToDate.Visible = false;
                        btnView.Enabled = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No Exam Conducted";
                        btnView.Visible = false;
                        chckdeletesubjects.Visible = false;
                    }
                }
                else
                {
                    cbBatchYear.Visible = false;
                    cbCourse.Visible = false;
                    cbDate.Visible = false;
                    cbDepartment.Visible = false;
                    cbSubject.Visible = false;
                    ddlBatchYear.Visible = false;
                    ddlCourse.Visible = false;
                    ddlDepartment.Visible = false;
                    ddlSubjectName.Visible = false;
                    txtFromDate.Visible = false;
                    txtToDate.Visible = false;
                    lblerror.Visible = true;
                    btnView.Enabled = false;
                    lblerror.Text = "No Exam Conducted";
                    btnView.Visible = false;
                    chckdeletesubjects.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadyear()
    {
        try
        {
            ddlYear.Items.Clear();
            DataSet ds = da.Examyear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
                ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void loadmonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            DataSet ds = new DataSet();
            string year1 = ddlYear.SelectedValue;
            ds = da.Exammonth(year1);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            string group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim() != "TRUE" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = da.select_method("bind_college", hat, "sp");
            cblCollege.Items.Clear();
            txtCollege.Enabled = true;
            chkCollege.Checked = false;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblCollege.DataSource = ds;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
            }
            else
            {
                lblerror.Text = "Set college rights to the staff";
                lblerror.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = Convert.ToString(ex);
            lblerror.Visible = true;
        }
    }

    public void loadbatch()
    {
        try
        {
            newcollegeCode = string.Empty;
            qrycollege = string.Empty;//ex.coll_code in(" + newcollegeCode + ")
            selcollege = 0;
            if (chkCollege.Checked)
            {
                if (cblCollege.Items.Count > 0)
                {
                    foreach (ListItem li in cblCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selcollege++;
                            if (string.IsNullOrEmpty(newcollegeCode.Trim()))
                            {
                                newcollegeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                newcollegeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
            }
            if (selcollege != 0)
            {
                qrycollege = " and ex.coll_code in(" + newcollegeCode + ")";
            }
            ddlBatchYear.Items.Clear();
            string strquery = "select distinct e.batchFrom as BatchYear from exmtt e,exmtt_det ex where   ex.exam_code=e.exam_code  and e.exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + qrycollege + " and e.exam_year='" + ddlYear.SelectedItem.ToString() + "' order by BatchYear desc";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(strquery, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                {
                    ddlBatchYear.Items.Add(ds.Tables[0].Rows[b]["BatchYear"].ToString());
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void loaddegree()
    {
        try
        {
            ddlCourse.Items.Clear();

            newcollegeCode = string.Empty;
            qrycollege = string.Empty;//ex.coll_code in(" + newcollegeCode + ")
            selcollege = 0;
            if (chkCollege.Checked)
            {
                if (cblCollege.Items.Count > 0)
                {
                    foreach (ListItem li in cblCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selcollege++;
                            if (string.IsNullOrEmpty(newcollegeCode.Trim()))
                            {
                                newcollegeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                newcollegeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
            }
            if (selcollege != 0)
            {
                qrycollege = " and ex.coll_code in(" + newcollegeCode + ")";
            }
            string batch = string.Empty;
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + ddlBatchYear.SelectedItem.Text.ToString() + "'";
                }
            }
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }
            string s1 = "select distinct c.course_Name as CourseName,c.course_id as CourseId from exmtt e,exmtt_det ex,department dpt,degree d ,course c,Subject s where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and s.subject_no=ex.subject_no " + qrycollege + " and ex.exam_code=e.exam_code and e.exam_month='" + ddlMonth.SelectedValue.ToString() + "'and e.exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' " + batch + " " + typeval + " order by CourseName";
            DataSet dss1 = da.select_method_wo_parameter(s1, "Text");
            if (dss1.Tables[0].Rows.Count > 0)
            {
                for (int j1 = 0; j1 < dss1.Tables[0].Rows.Count; j1++)
                {
                    ddlCourse.Items.Add(new ListItem(dss1.Tables[0].Rows[j1]["CourseName"].ToString(), dss1.Tables[0].Rows[j1]["CourseId"].ToString()));

                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }

    }

    public void loaddepartment()
    {
        try
        {
            ddlDepartment.Items.Clear();

            newcollegeCode = string.Empty;
            qrycollege = string.Empty;//ex.coll_code in(" + newcollegeCode + ")
            selcollege = 0;
            if (chkCollege.Checked)
            {
                if (cblCollege.Items.Count > 0)
                {
                    foreach (ListItem li in cblCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selcollege++;
                            if (string.IsNullOrEmpty(newcollegeCode.Trim()))
                            {
                                newcollegeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                newcollegeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
            }
            if (selcollege != 0)
            {
                qrycollege = " and ex.coll_code in(" + newcollegeCode + ")";
            }
            string degree = string.Empty;
            string batch = string.Empty;
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + ddlBatchYear.SelectedItem.Text.ToString() + "'";
                }
            }
            if (cbCourse.Checked == true)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    degree = " and c.course_id='" + ddlCourse.SelectedValue.ToString() + "'";
                }
            }
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }

            string s = "select distinct dpt.Dept_Name as DepartmentName,d.degree_Code as DepartmentCode from exmtt e,exmtt_det ex,department dpt,degree d ,course c where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code " + qrycollege + " and ex.exam_code=e.exam_code and ex.exam_code=e.exam_code  and e.exam_month='" + ddlMonth.SelectedValue.ToString() + "'and e.exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' " + batch + " " + degree + " " + typeval + " order by DepartmentName";
            DataSet dss = da.select_method_wo_parameter(s, "Text");
            if (dss.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                {
                    ddlDepartment.Items.Add(new ListItem(dss.Tables[0].Rows[j]["DepartmentName"].ToString(), dss.Tables[0].Rows[j]["DepartmentCode"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void loadsubject()
    {
        try
        {
            ddlSubjectName.Items.Clear();


            newcollegeCode = string.Empty;
            qrycollege = string.Empty;//ex.coll_code in(" + newcollegeCode + ")
            selcollege = 0;
            if (chkCollege.Checked)
            {
                if (cblCollege.Items.Count > 0)
                {
                    foreach (ListItem li in cblCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selcollege++;
                            if (string.IsNullOrEmpty(newcollegeCode.Trim()))
                            {
                                newcollegeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                newcollegeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
            }
            if (selcollege != 0)
            {
                qrycollege = " and ex.coll_code in(" + newcollegeCode + ")";
            }
            string degree = string.Empty;
            string batch = string.Empty;
            string course = string.Empty;
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + ddlBatchYear.SelectedItem.Text.ToString() + "'";
                }
            }
            if (cbCourse.Checked == true)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    course = " and c.course_id='" + ddlCourse.SelectedValue.ToString() + "'";
                }
            }
            if (cbDepartment.Checked == true)
            {
                if (ddlDepartment.Items.Count > 0)
                {
                    degree = " and e.degree_code='" + ddlDepartment.SelectedValue.ToString() + "'";
                }
            }
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }
            string strquery = "select distinct rtrim(s.subject_name) as subjectname,s.subject_code as subjectcode from exmtt e,exmtt_det ex,department dpt,degree d ,course c,subject s where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and ex.exam_code=e.exam_code and s.subject_no=ex.subject_no " + qrycollege + " and e.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_year='" + ddlYear.SelectedItem.ToString() + "' " + batch + " " + course + " " + degree + " " + typeval + " order by subjectname,subjectcode";
            DataSet dssubject = da.select_method_wo_parameter(strquery, "Text");
            if (dssubject.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dssubject.Tables[0].Rows.Count; j++)
                {
                    ddlSubjectName.Items.Add(new ListItem(dssubject.Tables[0].Rows[j]["subjectname"].ToString(), dssubject.Tables[0].Rows[j]["subjectcode"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void chkindegee_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadmonth();
        loadbatch();
        loaddegree();
        loaddepartment();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadbatch();
        loaddegree();
        loaddepartment();
    }

    public void clear()
    {
        Fptimetable.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        lblerror.Visible = false;
        Printcontrol.Visible = false;
        treepanel.Visible = false;
        lblmedate.Visible = false;
        ddlmedate.Visible = false;
        lblmesession.Visible = false;
        ddlmesession.Visible = false;
        btnmemove.Visible = false;
    }

    protected void cbDate_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (cbDate.Checked == true)
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string date1 = txtFromDate.Text.ToString();
        string date2 = txtToDate.Text.ToString();
        string[] spf = date1.Split(new Char[] { '/' });
        string[] spd = date2.Split(new Char[] { '/' });
        DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
        if (dt1 > dt2)
        {
            lblerror.Visible = true;
            lblerror.Text = "From Date Should be Less then To Date";
            txtFromDate.Text = date2;
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string date1 = txtFromDate.Text.ToString();
        string date2 = txtToDate.Text.ToString();
        string[] spf = date1.Split(new Char[] { '/' });
        string[] spd = date2.Split(new Char[] { '/' });
        DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
        if (dt1 > dt2)
        {
            lblerror.Visible = true;
            lblerror.Text = "From Date Should be Less then To Date";
            txtFromDate.Text = date2;
        }
    }

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            clear();
            txtCollege.Enabled = false;
            chkallColleges.Checked = false;
            txtCollege.Text = "--Select--";
            if (chkCollege.Checked)
            {
                if (cblCollege.Items.Count > 0)
                {
                    txtCollege.Enabled = true;
                }
            }
            foreach (ListItem li in cblCollege.Items)
            {
                li.Selected = false;
            }
            loadbatch();
            loaddegree();
            loaddepartment();
            loadsubject();
        }
        catch (Exception ex)
        {
        }

        //ddlBatchYear.Enabled = false;
        //if (cbBatchYear.Checked == true)
        //{
        //    if (ddlBatchYear.Items.Count > 0)
        //    {
        //        ddlBatchYear.Enabled = true;
        //    }
        //    else
        //    {

        //        lblerror.Visible = true;
        //        lblerror.Text = "No Batch Year's Available";
        //    }
        //}

    }

    protected void chkallColleges_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtCollege.Text = "--Select--";
            int count = 0;
            if (chkallColleges.Checked == true)
            {
                count++;
                for (int i = 0; i < cblCollege.Items.Count; i++)
                {
                    cblCollege.Items[i].Selected = true;
                }
                txtCollege.Text = "College (" + (cblCollege.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblCollege.Items.Count; i++)
                {
                    cblCollege.Items[i].Selected = false;
                }
                txtCollege.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtCollege.Text = "--Select--";
            int commcount = 0;
            for (int i = 0; i < cblCollege.Items.Count; i++)
            {
                if (cblCollege.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblCollege.Items.Count)
                {
                    chkallColleges.Checked = true;
                }
                txtCollege.Text = "College (" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbBatchYear_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlBatchYear.Enabled = false;
        if (cbBatchYear.Checked == true)
        {
            if (ddlBatchYear.Items.Count > 0)
            {
                ddlBatchYear.Enabled = true;
            }
            else
            {

                lblerror.Visible = true;
                lblerror.Text = "No Batch Year's Available";
            }
        }
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void ddlBatchYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void cbCourse_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlCourse.Enabled = false;
        if (cbCourse.Checked == true)
        {
            if (ddlCourse.Items.Count > 0)
            {
                ddlCourse.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = " No Course's Available";
            }
        }
        loaddepartment();
        loadsubject();
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddepartment();
        loadsubject();
    }

    protected void cbDepartment_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlDepartment.Enabled = false;
        if (cbDepartment.Checked == true)
        {
            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Department's Available";
            }
        }
        loadsubject();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadsubject();
    }

    protected void cbSubject_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlSubjectName.Enabled = false;
        if (cbSubject.Checked == true)
        {
            if (ddlSubjectName.Items.Count > 0)
            {
                ddlSubjectName.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Subject's Available";
            }
        }
    }

    protected void ddlSubjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            Fptimetable.Sheets[0].ColumnCount = 0;
            Fptimetable.Sheets[0].RowCount = 0;
            Fptimetable.Sheets[0].ColumnCount = 4;

            Fptimetable.CommandBar.Visible = false;
            Fptimetable.RowHeader.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fptimetable.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fptimetable.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fptimetable.Sheets[0].DefaultStyle.Font.Bold = false;

            Fptimetable.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fptimetable.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            Fptimetable.Sheets[0].Columns[0].Width = 50;
            Fptimetable.Sheets[0].Columns[1].Width = 800;
            Fptimetable.Sheets[0].Columns[1].Locked = true;
            Fptimetable.Sheets[0].Columns[0].Locked = true;
            Fptimetable.Sheets[0].Columns[2].Width = 80;
            Fptimetable.Sheets[0].Columns[3].Width = 50;

            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name (Subject Code)";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Count";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";

            Fptimetable.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fptimetable.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fptimetable.Sheets[0].AutoPostBack = false;

            if (ddlYear.Items.Count > 0 && ddlMonth.Items.Count > 0)
            {
                string order = string.Empty;
                string columnvisible = string.Empty;
                if (chkindegee.Checked == true)
                {
                    columnvisible = ",e.batchFrom,dpt.Dept_Name,c.Course_Name,e.degree_code";
                    order = " e.batchFrom desc,e.degree_code,dpt.Dept_Name,c.Course_Name,";
                }

                string dateval = string.Empty;
                if (cbDate.Checked == true)
                {
                    string date1 = txtFromDate.Text.ToString();
                    string date2 = txtToDate.Text.ToString();
                    string[] spf = date1.Split(new Char[] { '/' });
                    string[] spd = date2.Split(new Char[] { '/' });
                    DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                    DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);

                    if (dt1 > dt2)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "From Date Should be Less then To Date";
                        return;
                    }
                    dateval = " and ex.exam_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'";
                }

                newcollegeCode = string.Empty;
                qrycollege = string.Empty;//ex.coll_code in(" + newcollegeCode + ")
                selcollege = 0;
                if (chkCollege.Checked)
                {
                    if (cblCollege.Items.Count > 0)
                    {
                        foreach (ListItem li in cblCollege.Items)
                        {
                            if (li.Selected)
                            {
                                selcollege++;
                                if (string.IsNullOrEmpty(newcollegeCode.Trim()))
                                {
                                    newcollegeCode = "'" + li.Value + "'";
                                }
                                else
                                {
                                    newcollegeCode += ",'" + li.Value + "'";
                                }
                            }
                        }
                        if (selcollege == 0)
                        {
                            Fptimetable.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "Please Select College And Then Proceed";
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            txtexcelname.Text = string.Empty;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                        }
                    }
                    else
                    {
                        Fptimetable.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No College Were Found";
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        txtexcelname.Text = string.Empty;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                    }

                }
                if (selcollege != 0)
                {
                    qrycollege = " and ex.coll_code in(" + newcollegeCode + ")";
                }

                string batch = string.Empty;
                if (cbBatchYear.Checked == true)
                {
                    if (ddlBatchYear.Items.Count > 0)
                    {
                        batch = " and e.batchfrom='" + ddlBatchYear.SelectedItem.ToString() + "'";
                    }
                }

                string course = string.Empty;
                if (cbCourse.Checked == true)
                {
                    if (ddlCourse.Items.Count > 0)
                    {
                        course = " and c.course_id='" + ddlCourse.SelectedValue.ToString() + "'";
                    }
                }

                string degree = string.Empty;
                if (cbDepartment.Checked == true)
                {
                    if (ddlDepartment.Items.Count > 0)
                    {
                        degree = " and e.degree_code='" + ddlDepartment.SelectedValue.ToString() + "'";
                    }
                }

                string subject = string.Empty;
                if (cbSubject.Checked == true)
                {
                    if (ddlSubjectName.Items.Count > 0)
                    {
                        subject = " and rtrim(s.subject_name)='" + ddlSubjectName.SelectedItem.ToString() + "'";
                    }
                }
                string typeval = string.Empty;
                if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
                {
                    if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                    {
                        typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                    }
                }

                Hashtable hatequalsubcode = new Hashtable();
                //and ex.coll_code='" + Session["collegecode"].ToString() + "'
                string strquery = "select distinct s.subject_Name as SubjectName,d.college_code,ex.exam_session as Session,ex.StudentType,convert(Varchar(20),ex.exam_Date,105) as ExamDate,ex.exam_Date,s.subject_code as subjectcode" + columnvisible + " ,isnull(t.Com_Subject_Code,'') as Com_Subject_Code from exmtt e,exmtt_det ex,department dpt,degree d ,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and s.subject_no=ex.subject_no  and ex.exam_code=e.exam_code  and e.exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + qrycollege + " and e.exam_year='" + ddlYear.SelectedItem.ToString() + "' " + dateval + " " + batch + " " + course + " " + degree + " " + subject + " " + typeval + " order by " + order + "ex.exam_Date,Session desc,SubjectName";
                DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    chckdeletesubjects.Visible = true;
                    string getcode = "select distinct t.Com_Subject_Code,s.subject_name,t.Equal_Subject_Code from tbl_equal_paper_Matching t,subject s where t.Com_Subject_Code=s.subject_code order by t.Com_Subject_Code,t.Equal_Subject_Code desc,s.subject_name";
                    DataSet dsequalcode = da.select_method_wo_parameter(getcode, "text");
                    string tempcode = string.Empty;
                    string seteqvalus = string.Empty;
                    for (int eq = 0; eq < dsequalcode.Tables[0].Rows.Count; eq++)
                    {
                        string comcode = dsequalcode.Tables[0].Rows[eq]["Com_Subject_Code"].ToString();
                        string subname = dsequalcode.Tables[0].Rows[eq]["subject_name"].ToString();
                        string seteqsubno = dsequalcode.Tables[0].Rows[eq]["Equal_Subject_Code"].ToString();
                        if (comcode != tempcode)
                        {
                            seteqvalus = seteqsubno;
                            tempcode = comcode;
                        }

                        if (!hatequalsubcode.Contains(comcode))
                        {
                            hatequalsubcode.Add(comcode, seteqvalus);
                        }
                        else
                        {
                            string setval = hatequalsubcode[comcode].ToString();
                            setval = setval + " , " + seteqsubno;
                            hatequalsubcode[comcode] = setval;
                        }

                    }

                    Fptimetable.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    lblmedate.Visible = true;
                    ddlmedate.Visible = true;
                    lblmesession.Visible = true;
                    ddlmesession.Visible = true;
                    btnmemove.Visible = true;
                    loadmdatesession();

                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

                    int sno = 0;
                    int startrow = 0;
                    string tempdegree = string.Empty;

                    Hashtable hatalreadyset = new Hashtable();
                    ArrayList AddComSubArray = new ArrayList();
                    bool GCSubject = false;
                    Hashtable hat = new Hashtable();
                    Dictionary<string, string> dicsemcol = new Dictionary<string, string>();
                    Dictionary<string, string> dicGCSubject = new Dictionary<string, string>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string getdate = ds.Tables[0].Rows[i]["ExamDate"].ToString();
                        string[] spDate = getdate.Split(new Char[] { '-' });
                        DateTime DateVal =Convert.ToDateTime(spDate[1] + "/" + spDate[2] + "/" + spDate[0]);
                        string examsession = ds.Tables[0].Rows[i]["Session"].ToString();
                        string comsub = ds.Tables[0].Rows[i]["Com_Subject_Code"].ToString();
                        if (comsub.Trim() != "" && comsub != null)
                        {
                            //rajkumar
                            DataTable dtGC = new DataTable();
                            String strGC = string.Empty;
                            string selectQ = "select distinct subject_type from sub_sem ss,subject s where s.syll_code=s.syll_code and s.subType_no=ss.subType_no  and s.subject_code='" + comsub + "'";
                            dtGC = dir.selectDataTable(selectQ);
                            if (dtGC.Rows.Count > 0)
                            {
                                strGC = Convert.ToString(dtGC.Rows[0]["subject_type"]).Trim();
                            }
                            if (strGC != "General Course" && !string.IsNullOrEmpty(comsub))
                            {

                                if (!AddComSubArray.Contains(comsub.Trim()))
                                {
                                    AddComSubArray.Add(comsub.Trim());
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                GCSubject = true;
                            }
                            //
                        }
                        if (chkindegee.Checked == true)
                        {
                            string Degree = ds.Tables[0].Rows[i]["batchFrom"].ToString() + " - " + ds.Tables[0].Rows[i]["Course_Name"].ToString() + " - " + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                            if (tempdegree != Degree)
                            {
                                Fptimetable.Sheets[0].RowCount++;
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = Degree;
                                Fptimetable.Sheets[0].SpanModel.Add(Fptimetable.Sheets[0].RowCount - 1, 0, 1, 3);
                                tempdegree = Degree;
                            }
                        }
                        if (!hatalreadyset.Contains(getdate + '-' + examsession))
                        {
                            sno++;
                            Fptimetable.Sheets[0].RowCount++;
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["ExamDate"].ToString() + " - " + examsession;
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fptimetable.Sheets[0].SpanModel.Add(Fptimetable.Sheets[0].RowCount - 1, 1, 1, 3);
                            Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                            hatalreadyset.Add(getdate + '-' + examsession, getdate + '-' + examsession);
                            startrow = Fptimetable.Sheets[0].RowCount;
                        }
                        //171ps1G02,121PS1G02
                        string subjectval = ds.Tables[0].Rows[i]["subjectcode"].ToString() + " - " + ds.Tables[0].Rows[i]["SubjectName"].ToString();

                        Boolean setflag = false;
                        // string comsub = ds.Tables[0].Rows[i]["Com_Subject_Code"].ToString();
                        //if (comsub.Trim() != "" && comsub != null)
                        //{
                        //    if (comsub.Trim().ToLower() != ds.Tables[0].Rows[i]["subjectcode"].ToString().Trim().ToLower())
                        //    {
                        //        setflag = true;
                        //    }
                        //}
                        string getsucode = string.Empty;
                        if (comsub.Trim() != "" && comsub != null)
                        {
                            if (hatequalsubcode.Contains(comsub))
                            {
                                subjectval = hatequalsubcode[comsub].ToString();
                                string[] sps = subjectval.Split(',');
                                for (int de = 0; de <= sps.GetUpperBound(0); de++)
                                {
                                    if (getsucode.Trim() == "")
                                    {
                                        getsucode = "'" + sps[de].ToString().Trim() + "'";
                                    }
                                    else
                                    {
                                        getsucode = getsucode + ",'" + sps[de].ToString().Trim() + "'";
                                    }
                                }
                            }
                        }
                        else
                        {
                            subjectval = ds.Tables[0].Rows[i]["subjectcode"].ToString();
                            getsucode = "'" + subjectval.Trim() + "'";
                        }
                        if (!hat.ContainsKey(getsucode + "$" + examsession))
                        {
                            if (setflag == false)
                            {
                                string sesva = ds.Tables[0].Rows[i]["Session"].ToString().Trim().ToLower();
                                Fptimetable.Sheets[0].RowCount++;
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["SubjectName"].ToString();
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["exam_date"].ToString();
                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Note = examsession;


                                int strebth = 0;
                                string StudentType = Convert.ToString(ds.Tables[0].Rows[i]["StudentType"]);
                                string stregth = string.Empty;
                                string cat = string.Empty;
                                DataTable dtGCStudCount = new DataTable();
                                if (getsucode.Trim() != "")
                                {

                                    if (GCSubject == true)
                                    {
                                        //if (!dicGCSubject.ContainsKey(getsucode))
                                        //{
                                        //    dicGCSubject.Add(getsucode, subjectval);
                                        if (StudentType == "0")
                                        {
                                            cat = "(Regular)";
                                        }
                                        if (StudentType == "1")
                                        {
                                            cat = "(Arrear)";
                                        }
                                        if (StudentType == "2")
                                        {
                                            cat = "(Regular/Arrear)";
                                        }
                                        bool semwise = false;//

                                        if (!hat.ContainsKey(getsucode + "$" + examsession))
                                        {
                                           // stregth = "select count(ea.roll_no) as totalCount,sy.semester from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,syllabus_master sy where r.Roll_No=ea.roll_no and  s.syll_code=sy.syll_code and  r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.exam_year='" + ddlYear.SelectedItem.ToString() + "' and s.subject_code in(" + getsucode + ")  group by sy.semester";
                                            stregth = "select count(ea.roll_no) as totalCount,sy.semester from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,syllabus_master sy,exmtt_det ex where r.Roll_No=ea.roll_no and s.subject_no=ex.subject_no and  s.syll_code=sy.syll_code and  r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and   ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.exam_year='" + ddlYear.SelectedItem.ToString() + "' and s.subject_code in(" + getsucode + ")  and exam_session='" + examsession + "' and ex.exam_Date between '" + DateVal.ToString("MM/dd/yyyy") + "' and '" + DateVal.ToString("MM/dd/yyyy") + "'  group by sy.semester";

                                            dtGCStudCount = dir.selectDataTable(stregth);
                                            if (dtGCStudCount.Rows.Count > 0)
                                            {
                                                //if (!dicsemcol.ContainsValue(getsucode))
                                                //{
                                                //    dicsemcol.Clear();
                                                //}
                                                string Semester = string.Empty;
                                                string studentCounts = string.Empty;

                                                foreach (DataRow dr in dtGCStudCount.Rows)
                                                {
                                                    //bool ODDsemwise = false;
                                                    //if (dtGCStudCount.Rows.Count == 1)
                                                    //    ODDsemwise = true;
                                                    if (semwise == false)
                                                    {
                                                        Semester = Convert.ToString(dr["semester"]).Trim();
                                                        studentCounts = Convert.ToString(dr["totalCount"]).Trim();
                                                        if (!hat.ContainsKey(getsucode))
                                                        {
                                                            if (!dicsemcol.ContainsKey(Semester + "$" + getsucode + "$" + examsession))
                                                            {
                                                                string strSem = "(" + Semester + "Semester" + ")";
                                                                semwise = true;
                                                                dicsemcol.Add(Semester + "$" + getsucode + "$" + examsession, examsession);
                                                                hat.Add(getsucode + "$" + examsession, examsession);
                                                                stregth = studentCounts;
                                                                if (stregth.Trim() != "" && stregth != null)
                                                                {
                                                                    strebth = Convert.ToInt32(stregth);
                                                                }

                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["SubjectName"].ToString() + " ( " + subjectval + ")" + " " + strSem + " " + cat;//+"(" + cat+ ")"
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Tag = subjectval;

                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Text = strebth.ToString();
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                                Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].CellType = chk;
                                                            }
                                                            //break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                 
                                    //
                                    else
                                    {
                                        //if (!hat.ContainsKey(getsucode + "$" + examsession))
                                        //{
                                        hat.Add(getsucode + "$" + examsession, examsession);

                                        stregth = da.GetFunction("select count(ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.exam_year='" + ddlYear.SelectedItem.ToString() + "' and s.subject_code in(" + getsucode + ")");
                                        if (stregth.Trim() != "" && stregth != null)
                                        {
                                            strebth = Convert.ToInt32(stregth);
                                        }
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["SubjectName"].ToString() + " ( " + subjectval + ")";//+"(" + cat+ ")"
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Tag = subjectval;

                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Text = strebth.ToString();
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].CellType = chk;
                                        //}
                                        //else
                                        //{
                                        //    Fptimetable.Sheets[0].RowCount = Fptimetable.Sheets[0].RowCount - 1;
                                        //}

                                    }
                                }

                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["SubjectName"].ToString() + " ( " + subjectval + ")";//+"(" + cat+ ")"
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Tag = subjectval;

                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Text = strebth.ToString();
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                //Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].CellType = chk;

                            }
                        }
             
                        }
                    }
                
                else
                {
                    Fptimetable.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No Record(s) Found";
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    txtexcelname.Text = string.Empty;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                }
            }
            else
            {
                Fptimetable.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Exam Conducted";
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }

            Fptimetable.Sheets[0].PageSize = Fptimetable.Sheets[0].RowCount;
            Fptimetable.Width = 1000;

            Double heighva = 20;
            if (Fptimetable.Sheets[0].RowCount > 500)
            {
                heighva = 1000;
            }
            else
            {
                heighva = Fptimetable.Sheets[0].RowCount * 20 + 40;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            Fptimetable.Height = Convert.ToInt32(heighva);

            Fptimetable.Sheets[0].PageSize = Fptimetable.Sheets[0].RowCount;
            Fptimetable.SaveChanges();


        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }

    }

    protected void Fptimetable_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellfalsg = true;
    }

    protected void Fptimetable_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellfalsg == true)
            {

            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnmissingsubject_Click(object sender, EventArgs e)
    {
        try
        {
            loadspread();
            loadmdatesession();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void loadspread()
    {
        try
        {
            Fptimetable.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpMissingSubject.Visible = false;
            FpMissingSubject.Sheets[0].SheetCorner.ColumnCount = 0;
            FpMissingSubject.Sheets[0].RowCount = 0;
            FpMissingSubject.Sheets[0].ColumnCount = 0;
            FpMissingSubject.Visible = false;
            FpMissingSubject.Sheets[0].SheetCorner.ColumnCount = 0;
            FpMissingSubject.Sheets[0].RowCount = 0;
            FpMissingSubject.Sheets[0].ColumnCount = 0;
            loadedate();

            FpMissingSubject.Sheets[0].RowCount = 0;
            FpMissingSubject.Sheets[0].ColumnCount = 5;
            FpMissingSubject.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpMissingSubject.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpMissingSubject.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Details";
            FpMissingSubject.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
            FpMissingSubject.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
            FpMissingSubject.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            FpMissingSubject.Sheets[0].Columns[0].Width = 50;
            FpMissingSubject.Sheets[0].Columns[1].Width = 300;
            FpMissingSubject.Sheets[0].Columns[2].Width = 100;
            FpMissingSubject.Sheets[0].Columns[3].Width = 250;
            FpMissingSubject.Sheets[0].Columns[4].Width = 50;
            FpMissingSubject.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }
            string getsubjectdetail = "select distinct s.subject_no,ed.batch_year,ed.degree_code,sy.semester,s.subject_name,ss.subject_type,s.subject_code,d.college_code,ss.ElectivePap,c.Edu_Level,c.Course_Name,de.Dept_Name ";
            getsubjectdetail = getsubjectdetail + " from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,syllabus_master sy,Department de where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and de.Dept_Code=d.Dept_Code and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(ss.lab,'0')=0 and isnull(s.sub_lab,'0')=0 ";
            getsubjectdetail = getsubjectdetail + " and sy.syll_code=ss.syll_code  and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + typeval + "  and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' )";
            getsubjectdetail = getsubjectdetail + " order by s.subject_name";

            DataSet dsmisssubject = da.select_method_wo_parameter(getsubjectdetail, "Text");

            if (dsmisssubject.Tables.Count > 0 && dsmisssubject.Tables[0].Rows.Count > 0)
            {
                treepanel.Visible = true;
                FpMissingSubject.Visible = true;
                FpMissingSubject.Sheets[0].RowCount++;
                FpMissingSubject.Sheets[0].Cells[0, 0].Text = string.Empty;
                FpMissingSubject.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                FpMissingSubject.Sheets[0].Cells[0, 4].CellType = chkall;
                int srno = 0;
                for (int i = 0; i < dsmisssubject.Tables[0].Rows.Count; i++)
                {
                    string sbatch = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["batch_year"]).Trim();
                    string sdegreecode = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["degree_code"]).Trim();
                    string ssem = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["semester"]).Trim();
                    string sdegree = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["Course_Name"]).Trim();
                    string sdepartment = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["Dept_Name"]).Trim();
                    string ssubjectname = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["subject_name"]).Trim();
                    string ssubjectcode = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["subject_code"]).Trim();
                    string ssubno = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["subject_no"]).Trim();
                    string collegeCodeNew = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["college_code"]).Trim();

                    string sdegreedetails = Convert.ToString(dsmisssubject.Tables[0].Rows[i]["batch_year"]).Trim() + "-" + Convert.ToString(dsmisssubject.Tables[0].Rows[i]["degree_code"]).Trim() + "-" + Convert.ToString(dsmisssubject.Tables[0].Rows[i]["semester"]).Trim();

                    FpMissingSubject.Sheets[0].RowCount++;
                    srno++;
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 1].Text = sbatch + " - " + sdegree + " - " + sdepartment + " - " + ssem;
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 1].Tag = sdegreedetails;
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 2].Text = ssubjectcode.ToString();
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 2].Tag = ssubno;
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 3].Text = ssubjectname.ToString();
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 3].Tag = collegeCodeNew.Trim();
                    FpMissingSubject.Sheets[0].Cells[FpMissingSubject.Sheets[0].RowCount - 1, 4].CellType = chk;
                }
            }
            else
            {
                treepanel.Visible = false;
                FpMissingSubject.Visible = false;
                lblerror.Visible = true;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Text = "No Subject(s) Available";
            }

            FpMissingSubject.Width = 800;
            Double heighva = 20;
            if (FpMissingSubject.Sheets[0].RowCount > 500)
            {
                heighva = 1000;
            }
            else
            {
                heighva = FpMissingSubject.Sheets[0].RowCount * 20 + 25;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            FpMissingSubject.Height = Convert.ToInt32(heighva);

            FpMissingSubject.Width = 800;

            FpMissingSubject.Sheets[0].PageSize = FpMissingSubject.Sheets[0].RowCount;
            FpMissingSubject.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnalter_Click(object sender, EventArgs e)
    {
        try
        {
            FpMissingSubject.SaveChanges();
            Boolean flag = false;
            for (int r = 1; r < FpMissingSubject.Sheets[0].RowCount; r++)
            {
                int isval = Convert.ToInt32(FpMissingSubject.Sheets[0].Cells[r, 4].Value);
                if (isval == 1)
                {
                    flag = true;
                    string gertsh = FpMissingSubject.Sheets[0].Cells[r, 1].Tag.ToString();
                    string[] spd = gertsh.Split('-');
                    string byear = string.Empty;
                    string dcode = string.Empty;
                    string sem = string.Empty;
                    if (spd.GetUpperBound(0) == 2)
                    {
                        byear = spd[0].ToString();
                        dcode = spd[1].ToString();
                        sem = spd[2].ToString();

                        string subno = FpMissingSubject.Sheets[0].Cells[r, 2].Tag.ToString();
                        string collegenew = Convert.ToString(FpMissingSubject.Sheets[0].Cells[r, 3].Tag).Trim();
                        string exm = "if not exists(select * from exmtt where degree_code='" + dcode.ToString() + "' and batchFrom='" + byear.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dcode.ToString() + "','" + byear.ToString() + "','" + byear.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + sem.ToString() + "') select * from exmtt where degree_code='" + dcode.ToString() + "' and batchFrom='" + byear.ToString() + "'   and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";//and Semester='" + sem.ToString() + "'
                        int s = da.update_method_wo_parameter(exm, "text");
                        DataSet ds2 = new DataSet();
                        ds2 = da.select_method_wo_parameter(exm, "text");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            string getdate = ddlesession.SelectedValue.ToString();
                            string[] spse = getdate.Split('@');
                            string startdate = spse[0].ToString();
                            string enddate = spse[1].ToString();

                            string save = "if exists(select * from exmtt_det where subject_no='" + subno + "' and exam_date='" + ddledate.SelectedValue.ToString() + "'and exam_session='" + ddlesession.SelectedItem.ToString() + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegenew + "' and exam_type='Univ') update exmtt_det set subject_no='" + subno + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + ddledate.SelectedValue.ToString() + "', exam_session='" + ddlesession.SelectedItem.ToString() + "' , exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + collegenew + "' , exam_type='Univ' where subject_no='" + subno + "' and exam_date='" + ddledate.SelectedValue.ToString() + "'and exam_session='" + ddlesession.SelectedItem.ToString() + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegenew + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + subno + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + ddledate.SelectedValue.ToString() + "','" + ddlesession.SelectedItem.ToString() + "' ,'" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + collegenew + "' ,'Univ')";
                            int v = da.update_method_wo_parameter(save, "text");
                        }
                    }
                }
            }
            if (flag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subject Successfully on " + ddledate.SelectedItem.ToString() + " and " + ddlesession.SelectedItem.ToString() + " Session ')", true);
                loadspread();
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Subject And Then Proceed";
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();

        }
    }

    public void loadedate()
    {
        try
        {
            ddledate.Items.Clear();
            string getdate = "select distinct Convert(nvarchar(15),et.exam_date,101) edate,Convert(nvarchar(15),et.exam_date,103) edate1,et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' order by et.exam_date";
            DataSet dnew = da.select_method_wo_parameter(getdate, "text");
            ddledate.DataSource = dnew;
            ddledate.DataTextField = "edate1";
            ddledate.DataValueField = "edate";
            ddledate.DataBind();


            DateTime dtf = DateTime.Now;
            DateTime dtt = DateTime.Now;

            string hol = "select * from examholiday where exammonth='" + ddlMonth.SelectedValue.ToString() + "' and examyear='" + ddlYear.SelectedValue.ToString() + "'";
            ds = da.select_method_wo_parameter(hol, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dtf = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
                dtt = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
            }
            Hashtable hatdate = new Hashtable();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DateTime dthol = Convert.ToDateTime(ds.Tables[0].Rows[i]["holiday_date"].ToString());
                if (!hatdate.Contains(dthol.ToString("MM/dd/yyyy")))
                {
                    hatdate.Add(dthol.ToString("MM/dd/yyyy"), dthol.ToString("MM/dd/yyyy"));
                }
            }

            for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
            {
                if (!hatdate.Contains(dt.ToString("MM/dd/yyyy")))
                {
                    ddlmedate.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dt.ToString("dd/MM/yyyy"), dt.ToString("MM/dd/yyyy")));
                }
            }

            ddlesession.Items.Clear();
            string gethour = "select distinct exam_session,RIGHT(CONVERT(VARCHAR, et.start_time, 100),7)+'@'+RIGHT(CONVERT(VARCHAR, et.end_time, 100),7) as timeval from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
            DataSet dsedate = da.select_method_wo_parameter(gethour, "text");
            ddlesession.DataSource = dsedate;
            ddlesession.DataTextField = "exam_session";
            ddlesession.DataValueField = "timeval";
            ddlesession.DataBind();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();

        }
    }

    protected void loadmdatesession()
    {
        try
        {
            ddlmedate.Items.Clear();
            string getdate = "select distinct Convert(nvarchar(15),et.exam_date,101) edate,Convert(nvarchar(15),et.exam_date,103) edate1,et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' order by et.exam_date";
            DataSet dsedate = da.select_method_wo_parameter(getdate, "text");
            ddlmedate.DataSource = dsedate;
            ddlmedate.DataTextField = "edate1";
            ddlmedate.DataValueField = "edate";
            ddlmedate.DataBind();
            //magesh 2/2/18
            ddlmedate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Others", "Others"));//magesh 2/2/18
            

            //DateTime dtf = DateTime.Now;
            //DateTime dtt = DateTime.Now;

            //string hol = "select * from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "'";
            //ds = da.select_method_wo_parameter(hol, "text");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    dtf = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
            //    dtt = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
            //}
            //Hashtable hatdate = new Hashtable();
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    DateTime dthol = Convert.ToDateTime(ds.Tables[0].Rows[i]["holiday_date"].ToString());
            //    if (!hatdate.Contains(dthol.ToString("MM/dd/yyyy")))
            //    {
            //        hatdate.Add(dthol.ToString("MM/dd/yyyy"), dthol.ToString("MM/dd/yyyy"));
            //    }
            //}

            //for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
            //{
            //    if (!hatdate.Contains(dt.ToString("MM/dd/yyyy")))
            //    {
            //        ddlmedate.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dt.ToString("dd/MM/yyyy"), dt.ToString("MM/dd/yyyy")));
            //    }
            //}

            ddlmesession.Items.Clear();
            // DataSet dsedate = new DataSet();
            dsedate.Dispose();
            string gethour = "select distinct exam_session,RIGHT(CONVERT(VARCHAR, et.start_time, 100),7)+'@'+RIGHT(CONVERT(VARCHAR, et.end_time, 100),7) as timeval from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
            dsedate = da.select_method_wo_parameter(gethour, "text");
            ddlmesession.DataSource = dsedate;
            ddlmesession.DataTextField = "exam_session";
            ddlmesession.DataValueField = "timeval";
            ddlmesession.DataBind();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();

        }
    }

    //magesh 2/2/18
    protected void ddlmedate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlmedate.Items[0].Selected == true)
            {
                TextBox1.Visible = true;

            }
            else
            {
                TextBox1.Visible = false;
               
            }

        }
        catch (Exception ex)
        { 
            da.sendErrorMail(ex, CollegeCode, "ExamTimeTableAlter"); 
        }
        
    }//magesh 2/2/18

    protected void btnmemove_Click(object sender, EventArgs e)
    {
        try
        {
            #region magesh 2/2/18
            string mexamdte = string.Empty;
            if (ddlmedate.Items[0].Selected == true)
            {
                string date1 = TextBox1.Text.ToString();
                string[] spl1 = date1.Split('/');
                DateTime dtl1 = Convert.ToDateTime(spl1[1] + '/' + spl1[0] + '/' + spl1[2]);
                string dodate = dtl1.ToString("dd");
                string dobmonth = dtl1.ToString("MM");
                string dobyear = dtl1.ToString("yyyy");
                date1 = Convert.ToString(dobmonth + "/" + dodate + "/" + dobyear).Trim();
                mexamdte = date1.ToString();
                TextBox1.Visible = false;

            }
            else
            {
                mexamdte = ddlmedate.SelectedValue.ToString();
            }
            #endregion magesh 2/2/18
            Boolean saveflaf = false;
            Fptimetable.SaveChanges();
            // magesh 2/2/18 string mexamdte = ddlmedate.SelectedValue.ToString();
            string msession = ddlmesession.SelectedItem.ToString();

            string getdate = ddlmesession.SelectedValue.ToString();
            string[] spse = getdate.Split('@');
            string startdate = spse[0].ToString();
            string enddate = spse[1].ToString();

            int allowedtsrgeh = 2400;

            for (int r = 0; r < Fptimetable.Sheets[0].RowCount; r++)
            {
                int selval = Convert.ToInt32(Fptimetable.Sheets[0].Cells[r, 3].Value);
                if (selval == 1)
                {
                    string examdate = Fptimetable.Sheets[0].Cells[r, 0].Tag.ToString();
                    string examsession = Fptimetable.Sheets[0].Cells[r, 0].Note.ToString();
                    string subcode = Fptimetable.Sheets[0].Cells[r, 1].Tag.ToString();
                    string newCollegeCode = Convert.ToString(Fptimetable.Sheets[0].Cells[r, 2].Tag).Trim();

                    string setsubcode = string.Empty;
                    string[] spv = subcode.Split(',');
                    ArrayList arrSubjectCode = new ArrayList();
                    for (int s = 0; s <= spv.GetUpperBound(0); s++)
                    {
                        if (spv[s].ToString().Trim() != "")
                        {
                            if (setsubcode == "")
                            {
                                setsubcode = "'" + spv[s].ToString() + "'";
                            }
                            else
                            {
                                setsubcode = setsubcode + ",'" + spv[s].ToString() + "'";
                            }
                            if (!arrSubjectCode.Contains(spv[s].Trim().ToLower()))
                            {
                                arrSubjectCode.Add(spv[s].Trim().ToLower());
                            }
                        }
                    }

                    string getequalsubval = "select Equal_Subject_Code as subjectcode from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code in(" + setsubcode + "))";
                    DataSet dssubval = da.select_method_wo_parameter(getequalsubval, "text");
                    for (int eq = 0; eq < dssubval.Tables[0].Rows.Count; eq++)
                    {
                        string streqsubcode = dssubval.Tables[0].Rows[eq]["subjectcode"].ToString();
                        if (!arrSubjectCode.Contains(streqsubcode.Trim().ToLower()))
                        {
                            arrSubjectCode.Add(streqsubcode.Trim().ToLower());
                        }
                    }

                    string getrollno = "select ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no";
                    getrollno = getrollno + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ead.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no";
                    getrollno = getrollno + " and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and et.exam_date='" + examdate + "' and et.exam_session='" + examsession + "' and s.subject_code in(" + setsubcode + "))";
                    DataSet dsacroll = da.select_method_wo_parameter(getrollno, "Text");

                    string getroll = string.Empty;
                    for (int ro = 0; ro < dsacroll.Tables[0].Rows.Count; ro++)
                    {
                        if (getroll == "")
                        {
                            getroll = "'" + dsacroll.Tables[0].Rows[ro]["roll_no"].ToString() + "'";
                        }
                        else
                        {
                            getroll = getroll + ",'" + dsacroll.Tables[0].Rows[ro]["roll_no"].ToString() + "'";
                        }
                    }
                    int countval = 0;
                    string strgetdetails = da.GetFunction("select isnull(count(ead.subject_no),'0') as stucount from exmtt e,exmtt_det et,subject s,exam_appl_details ead where e.exam_code=et.exam_code and et.subject_no=s.subject_no and ead.subject_no=et.subject_no and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + mexamdte + "' and et.exam_session='" + msession + "'  and ead.appl_no in(select ea.appl_no from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year=" + ddlYear.SelectedValue.ToString() + ") group by et.exam_date,et.exam_session");
                    if (strgetdetails.Trim() == "" || strgetdetails == null)
                    {
                        strgetdetails = "0";
                    }

                    countval = Convert.ToInt32(strgetdetails);
                    if (countval < allowedtsrgeh)
                    {
                        string checkroll = "select isnull(count(ea.roll_no),'0') from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + mexamdte + "' and et.exam_session='" + msession + "' and ea.roll_no in ( " + getroll + ")";
                        int getsubjectstucount = Convert.ToInt32(da.GetFunction(checkroll));
                        if (getsubjectstucount == 0)
                        {
                            string examdetails = "select distinct ed.batch_year,ed.degree_code,ed.current_semester,ead.subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no";
                            examdetails = examdetails + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_code in(" + setsubcode + ")";
                            DataSet dsexamdetaisl = da.select_method_wo_parameter(examdetails, "Text");

                            for (int es = 0; es < dsexamdetaisl.Tables[0].Rows.Count; es++)
                            {
                                string batchyear = dsexamdetaisl.Tables[0].Rows[es]["batch_year"].ToString();
                                string degreecode = dsexamdetaisl.Tables[0].Rows[es]["degree_code"].ToString();
                                string sem = dsexamdetaisl.Tables[0].Rows[es]["current_semester"].ToString();
                                string subno = dsexamdetaisl.Tables[0].Rows[es]["subject_no"].ToString();

                                string exm = "if not exists(select * from exmtt where degree_code='" + degreecode.ToString() + "' and batchFrom='" + batchyear.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + degreecode.ToString() + "','" + batchyear.ToString() + "','" + batchyear.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + sem.ToString() + "') select * from exmtt where degree_code='" + degreecode.ToString() + "' and batchFrom='" + batchyear.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                int s = da.update_method_wo_parameter(exm, "text");
                                DataSet ds2allarr = new DataSet();
                                ds2allarr = da.select_method_wo_parameter(exm, "text");
                                if (ds2allarr.Tables[0].Rows.Count > 0)
                                {
                                    string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                    string save = "if exists(select * from exmtt_det where subject_no='" + subno.ToString() + "' and exam_date='" + mexamdte + "'and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + newCollegeCode + "' and exam_type='Univ')update exmtt_det set subject_no='" + subno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + mexamdte + "', exam_session='" + msession + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + newCollegeCode + "' , exam_type='Univ' where subject_no='" + subno.ToString() + "' and exam_date='" + mexamdte + "'and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + newCollegeCode + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + subno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + mexamdte + "','" + msession + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + newCollegeCode + "' ,'Univ')";
                                    int v = da.update_method_wo_parameter(save, "text");

                                    string delval = "delete from exmtt_det where subject_no='" + subno.ToString() + "' and exam_date='" + examdate + "'and exam_session='" + examsession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + newCollegeCode + "' and exam_type='Univ'";
                                    int delva = da.update_method_wo_parameter(delval, "text");
                                }

                                saveflaf = true;
                            }
                        }
                        else
                        {
                            Hashtable hatsubroll = new Hashtable();
                            string alexistroll = "select s.subject_name,s.subject_code,s.subject_no,r.Reg_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ead.subject_no=s.subject_no and et.subject_no=s.subject_no and ed.batch_year=e.batchFrom and r.Roll_No=ea.roll_no and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + mexamdte + "' and et.exam_session='" + msession + "' and ea.roll_no in ( " + getroll + ") order by s.subject_code,s.subject_name,r.Reg_No";
                            DataSet dsrollno = da.select_method_wo_parameter(alexistroll, "Text");
                            string getrollsc = string.Empty;
                            string getdetails = string.Empty;
                            string alldetails = string.Empty;
                            for (int cr = 0; cr < dsrollno.Tables[0].Rows.Count; cr++)
                            {
                                string sname = dsrollno.Tables[0].Rows[cr]["subject_name"].ToString();
                                string scode = dsrollno.Tables[0].Rows[cr]["subject_code"].ToString();
                                string regno = dsrollno.Tables[0].Rows[cr]["Reg_No"].ToString();
                                if (!hatsubroll.Contains(scode))
                                {
                                    //if (getdetails == "")
                                    //{
                                    getdetails = " <br/><br/><br/>" + scode + '-' + sname + "  <br/>Reg No Are:<br/>";
                                    //}
                                    //else
                                    //{
                                    //    getdetails = getdetails + " <br/>" + scode + '-' + sname + " Reg No Are: ";
                                    //}
                                    if (alldetails == "")
                                    {
                                        alldetails = getdetails;
                                    }
                                    else
                                    {
                                        alldetails = alldetails + getrollsc + getdetails;
                                    }
                                    hatsubroll.Add(scode, scode);
                                }
                                if (getrollsc == "")
                                {
                                    getrollsc = regno;
                                }
                                else
                                {
                                    getrollsc = getrollsc + ", " + regno;
                                }
                            }
                            alldetails = alldetails + getrollsc;
                            lblerror.Visible = true;
                            lblerror.Text = "Following Roll_no Having Alredy Exams <br>" + alldetails + "";
                            return;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Exist The Limits !!!! <br> Already " + countval + " Student's Alterted Exam on That Day";
                        return;
                    }
                }
            }
            if (saveflaf == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subject Exam Date Altered Saved Successfully')", true);
                btnView_Click(sender, e);
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Subject And Then Proceed";
            }
        }
        catch (Exception ex)
        {
          //magesh 2/2/18
            da.sendErrorMail(ex, CollegeCode, "ExamTimeTableAlter"); 
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Clcik(object sender, EventArgs e)
    {
        try
        {
            string strgettime = "select distinct RIGHT(CONVERT(VARCHAR,start_time,100),7) as exstart,RIGHT(CONVERT(VARCHAR,end_time,100),7) exend,exam_session from exmtt_det et,exmtt e where e.exam_code=et.exam_code and e.Exam_month=11 and e.Exam_year=2014";
            DataSet dstime = da.select_method_wo_parameter(strgettime, "text");

            string foorenoontime = string.Empty;
            string afterenoontime = string.Empty;
            for (int t = 0; t < dstime.Tables[0].Rows.Count; t++)
            {
                string getss = dstime.Tables[0].Rows[t]["exam_session"].ToString().ToLower();
                if (getss.Contains("f"))
                {
                    foorenoontime = dstime.Tables[0].Rows[t]["exstart"].ToString() + " TO " + dstime.Tables[0].Rows[t]["exend"].ToString();
                }
                else
                {
                    afterenoontime = dstime.Tables[0].Rows[t]["exstart"].ToString() + " TO " + dstime.Tables[0].Rows[t]["exend"].ToString();
                }
            }

            string getdetails = string.Empty;
            if (cbDate.Checked == true)
            {
                getdetails = "@Date : " + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString() + "";
            }
            if (cbBatchYear.Checked == true)
            {
                if (getdetails == "")
                {
                    getdetails = "@Batch Year : " + ddlBatchYear.SelectedItem.ToString() + "";
                }
                else
                {
                    getdetails = getdetails + "@Batch Year : " + ddlBatchYear.SelectedItem.ToString() + "";
                }
            }
            string gettype = string.Empty;
            if (cbCourse.Checked == true && cbDepartment.Checked == true)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level from Degree d,course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + ddlDepartment.SelectedValue.ToString() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (spt[0].ToString().Trim() != "")
                        {
                            if (spt[0].ToString().Trim().ToLower() == "day")
                            {
                                gettype = "Regular - " + spt[1].ToString() + " -";
                            }
                            else
                            {
                                gettype = spt[0] + " - " + spt[1].ToString() + " -";
                            }
                        }
                    }
                }

                if (getdetails == "")
                {
                    getdetails = "@Degree : " + gettype + " " + ddlCourse.SelectedItem.ToString() + " - " + ddlDepartment.SelectedItem.ToString() + "";
                }
                else
                {
                    getdetails = getdetails + "@Degree : " + gettype + " " + ddlCourse.SelectedItem.ToString() + " - " + ddlDepartment.SelectedItem.ToString() + "";
                }
            }
            if (cbCourse.Checked == true && cbDepartment.Checked == false)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level from Degree d,course c where d.Course_Id=c.Course_Id and c.Course_Id='" + ddlCourse.SelectedValue.ToString() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (spt[0].ToString().Trim() != "")
                        {
                            if (spt[0].ToString().Trim().ToLower() == "day")
                            {
                                gettype = "Regular - " + spt[1].ToString() + " -";
                            }
                            else
                            {
                                gettype = spt[0] + " - " + spt[1].ToString() + " -";
                            }
                        }
                    }
                }
                if (getdetails == "")
                {
                    getdetails = "@Course : " + gettype + " " + ddlCourse.SelectedItem.ToString() + "";
                }
                else
                {
                    getdetails = getdetails + "@Degree : " + gettype + " " + ddlCourse.SelectedItem.ToString() + "";
                }
            }
            if (cbCourse.Checked == false && cbDepartment.Checked == true)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level+'%'+c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + ddlDepartment.SelectedValue.ToString() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (spt[0].ToString().Trim() != "")
                        {
                            if (spt[0].ToString().Trim().ToLower() == "day")
                            {
                                gettype = "Regular - " + spt[1].ToString() + " - " + spt[2].ToString() + " - ";
                            }
                            else
                            {
                                gettype = spt[0] + " - " + spt[1].ToString() + " - " + spt[2].ToString() + " - ";
                            }
                        }
                    }
                }
                if (getdetails == "")
                {
                    getdetails = "@Department : " + gettype + " " + ddlDepartment.SelectedItem.ToString() + "";
                }
                else
                {
                    getdetails = getdetails + "@Department : " + gettype + " " + ddlDepartment.SelectedItem.ToString() + "";
                }
            }
            if (foorenoontime.Trim() != "")
            {
                if (getdetails == "")
                {
                    getdetails = "@FORENOON   [" + foorenoontime + "]";
                }
                else
                {
                    getdetails = getdetails + "@FORENOON   [" + foorenoontime + "]";
                }
            }
            if (afterenoontime.Trim() != "")
            {
                if (getdetails == "")
                {
                    getdetails = "@AFTERNOON [" + afterenoontime + "]";
                }
                else
                {
                    getdetails = getdetails + "@AFTERNOON [" + afterenoontime + "]";
                }
            }

            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = "@Stream :" + ddltype.SelectedItem.ToString() + "";
                }
            }

            string degreedetails = "TIME TABLE ALTER FOR THE EXAMINATION " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " " + typeval + " " + getdetails;
            string pagename = "ExamTimeTablealter.aspx";
            Printcontrol.loadspreaddetails(Fptimetable, pagename, degreedetails);
            Printcontrol.Visible = true;

        }
        catch
        {

        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(Fptimetable, reportname);
                lblerror.Visible = false;

            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch
        {

        }

    }

    protected void FpMissingSubject_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ar = e.EditValues[4].ToString();
            int Act = Convert.ToInt32(e.CommandArgument);
            if (Act == 0)
            {
                int selva = 0;
                if (ar.Trim().ToLower() == "true" || ar.Trim().ToString() == "1")
                {
                    selva = 1;
                }
                for (int j = 1; j < Convert.ToInt16(FpMissingSubject.Sheets[0].RowCount); j++)
                {
                    FpMissingSubject.Sheets[0].Cells[j, 4].Value = selva;
                }
            }
        }
        catch
        {

        }

    }

    protected void btnduplicatecheck_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            Fptimetable.Sheets[0].RowCount = 0;
            Fptimetable.Sheets[0].ColumnCount = 6;

            Fptimetable.CommandBar.Visible = false;
            Fptimetable.RowHeader.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fptimetable.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fptimetable.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fptimetable.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();

            Fptimetable.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fptimetable.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Exam Date";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Session";
            Fptimetable.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code - Subject Name";

            Fptimetable.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            Fptimetable.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

            Fptimetable.Sheets[0].Columns[0].Width = 50;
            Fptimetable.Sheets[0].Columns[1].Width = 100;
            Fptimetable.Sheets[0].Columns[2].Width = 200;
            Fptimetable.Sheets[0].Columns[3].Width = 80;
            Fptimetable.Sheets[0].Columns[4].Width = 50;
            Fptimetable.Sheets[0].Columns[5].Width = 350;

            Fptimetable.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fptimetable.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fptimetable.Sheets[0].AutoPostBack = true;

            string examsubjectds = "select et.exam_date,et.exam_session,et.subject_no,e.batchfrom,e.degree_code,e.semester from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "'";
            DataSet dsexam = da.select_method_wo_parameter(examsubjectds, "Text");

            string strexamsubject = "select s.subject_name,s.subject_code,s.subject_no,ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s";
            strexamsubject = strexamsubject + " where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'";
            DataSet dsexamsubject = da.select_method_wo_parameter(strexamsubject, "Text");

            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    typeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
            }

            string strduplicatequery = "select et.exam_date,et.exam_session,r.Reg_No,ea.roll_no,ed.batch_year,ed.degree_code,ed.current_semester,r.stud_name,count(ead.subject_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Registration r,Degree d,Course c";
            strduplicatequery = strduplicatequery + " where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.subject_no=ead.subject_no and ea.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and et.exam_code in(select e.exam_code from exmtt e where e.exam_code=et.exam_code  and e.degree_code=ed.degree_code ";
            strduplicatequery = strduplicatequery + " and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "') and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' " + typeval + " ";
            strduplicatequery = strduplicatequery + " group by et.exam_date,et.exam_session,r.Reg_No,ea.roll_no,ed.batch_year,ed.degree_code,ed.current_semester,r.stud_name having count(ead.subject_no)>1 order by et.exam_date,et.exam_session,ed.batch_year desc,ed.degree_code,r.Reg_No";
            DataSet dsduplicate = da.select_method_wo_parameter(strduplicatequery, "text");

            if (dsduplicate.Tables[0].Rows.Count > 0)
            {
                int srno = 0;
                Fptimetable.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                for (int i = 0; i < dsduplicate.Tables[0].Rows.Count; i++)
                {
                    string regno = dsduplicate.Tables[0].Rows[i]["Reg_No"].ToString();
                    string roll = dsduplicate.Tables[0].Rows[i]["roll_no"].ToString();
                    DateTime dt = Convert.ToDateTime(dsduplicate.Tables[0].Rows[i]["exam_date"].ToString());
                    string esession = dsduplicate.Tables[0].Rows[i]["exam_session"].ToString();
                    string ebatch = dsduplicate.Tables[0].Rows[i]["batch_year"].ToString();
                    string edegree = dsduplicate.Tables[0].Rows[i]["degree_code"].ToString();
                    string esem = dsduplicate.Tables[0].Rows[i]["current_semester"].ToString();
                    string name = dsduplicate.Tables[0].Rows[i]["stud_name"].ToString();

                    dsexam.Tables[0].DefaultView.RowFilter = "batchfrom='" + ebatch + "' and degree_code='" + edegree + "' and semester='" + esem + "' and exam_date='" + dt.ToString("MM/dd/yyyy") + "' and exam_session='" + esession + "'";
                    DataView dvexam = dsexam.Tables[0].DefaultView;

                    string subno = string.Empty;
                    string subname = string.Empty;
                    for (int ec = 0; ec < dvexam.Count; ec++)
                    {
                        if (subno == "")
                        {
                            subno = "'" + dvexam[ec]["subject_no"].ToString() + "'";
                        }
                        else
                        {
                            subno = subno + ",'" + dvexam[ec]["subject_no"].ToString() + "'";
                        }
                    }
                    if (subno.Trim() != "")
                    {
                        string strsubval = " and subject_no in(" + subno + ")";
                        dsexamsubject.Tables[0].DefaultView.RowFilter = " roll_no='" + roll + "' " + strsubval + "";
                        DataView dvexsub = dsexamsubject.Tables[0].DefaultView;
                        for (int s = 0; s < dvexsub.Count; s++)
                        {
                            if (subname.Trim() == "")
                            {
                                subname = dvexsub[s]["subject_code"].ToString() + " - " + dvexsub[s]["subject_name"].ToString();
                            }
                            else
                            {
                                subname = subname + ", " + dvexsub[s]["subject_code"].ToString() + " - " + dvexsub[s]["subject_name"].ToString();
                            }
                        }
                    }

                    Fptimetable.Sheets[0].RowCount++;
                    srno++;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].Text = regno.ToString();
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].Text = name.ToString();
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].Text = dt.ToString("dd/MM/yyyy");
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 4].Text = esession.ToString();
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 5].Text = subname.ToString();
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.Color.Red;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
                Fptimetable.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }

            Fptimetable.Sheets[0].PageSize = Fptimetable.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        clear();
        if (ddlMonth.Items.Count > 0 && ddlYear.Items.Count > 0)
        {
            PSubAddtt.Visible = true;
            pmode();
            Pedu();
            Psem();
            PSubtype();
            Psubject();

            ddlpsession.Items.Clear();
            DataSet dsedate = new DataSet();
            //  string gethour = "select distinct exam_session,RIGHT(CONVERT(VARCHAR, et.start_time, 100),7)+'@'+RIGHT(CONVERT(VARCHAR, et.end_time, 100),7) as timeval from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
            string gethour = "select distinct exam_session,RIGHT(CONVERT(VARCHAR, et.start_time, 100),7)+'@'+RIGHT(CONVERT(VARCHAR, et.end_time, 100),7) as timeval from exmtt_det et order by exam_session desc";
            dsedate = da.select_method_wo_parameter(gethour, "text");
            ddlpsession.DataSource = dsedate;
            ddlpsession.DataTextField = "exam_session";
            ddlpsession.DataValueField = "timeval";
            ddlpsession.DataBind();
        }
        else
        {
            PSubAddtt.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "Please Select The Exam Month and Year and then proceed";
        }
    }

    public void pmode()
    {
        try
        {
            ddlpmode.Items.Clear();
            string strtypequery = "select distinct type from course where isnull(type,'')>''";
            DataSet dstype = da.select_method_wo_parameter(strtypequery, "text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddlpmode.DataSource = dstype;
                ddlpmode.DataTextField = "type";
                ddlpmode.DataBind();
                ddlpmode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            }
            else
            {
                ddlpmode.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    public void Pedu()
    {
        try
        {
            string strmode = string.Empty;
            if (ddlpmode.Items.Count > 0)
            {
                if (ddlpmode.SelectedItem.ToString().ToUpper().Trim() != "ALL" && ddlpmode.SelectedItem.ToString().Trim() != "")
                {
                    strmode = " WHERE type='" + ddlpmode.SelectedItem.ToString() + "'";
                }
            }
            ddlpedu.Items.Clear();
            string strtypequery = "select distinct edu_level from course " + strmode + "";
            DataSet dstype = da.select_method_wo_parameter(strtypequery, "text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddlpedu.DataSource = dstype;
                ddlpedu.DataTextField = "edu_level";
                ddlpedu.DataBind();
                ddlpedu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            }

        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    public void Psem()
    {
        try
        {
            ddlpsem.Items.Clear();
            string strmode = string.Empty;
            if (ddlpmode.Items.Count > 0)
            {
                if (ddlpmode.SelectedItem.ToString().ToUpper().Trim() != "ALL" && ddlpmode.SelectedItem.ToString().Trim() != "")
                {
                    strmode = " and c.type='" + ddlpmode.SelectedItem.ToString() + "'";
                }
            }

            string stredu = string.Empty;
            if (ddlpedu.Items.Count > 0)
            {
                if (ddlpedu.SelectedItem.ToString().ToUpper().Trim() != "ALL" && ddlpedu.SelectedItem.ToString().Trim() != "")
                {
                    stredu = " and c.edu_level='" + ddlpedu.SelectedItem.ToString() + "'";
                }
            }

            int duration = 0;
            string strquery = "select max(n.ndurations) sem from ndegree n,degree d,course c where n.degree_code=d.degree_code and d.course_id=c.course_id " + strmode + " " + stredu + "";
            string getsem = da.GetFunction(strquery);
            if (getsem.Trim() != "" && getsem != null && getsem != "0")
            {
                duration = Convert.ToInt32(getsem);
            }
            else
            {
                strquery = "select max(duration) sem from degree d,course c where d.course_id=c.course_id " + strmode + " " + stredu + "";
                getsem = da.GetFunction(strquery);
                if (getsem.Trim() != "" && getsem != null && getsem != "0")
                {
                    duration = Convert.ToInt32(getsem);
                }
            }
            for (int i = 1; i <= duration; i++)
            {
                ddlpsem.Items.Add(i.ToString());
            }
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    public void PSubtype()
    {
        try
        {
            ddlsubtype.Items.Clear();
            string strmode = string.Empty;
            if (ddlpmode.Items.Count > 0)
            {
                if (ddlpmode.SelectedItem.ToString().ToUpper().Trim() != "ALL" && ddlpmode.SelectedItem.ToString().Trim() != "")
                {
                    strmode = " and c.type='" + ddlpmode.SelectedItem.ToString() + "'";
                }
            }

            string stredu = string.Empty;
            if (ddlpedu.Items.Count > 0)
            {
                if (ddlpedu.SelectedItem.ToString().ToUpper().Trim() != "ALL" && ddlpedu.SelectedItem.ToString().Trim() != "")
                {
                    stredu = " and c.edu_level='" + ddlpedu.SelectedItem.ToString() + "'";
                }
            }
            string strquery = " select distinct ss.subject_type from exam_details ed,exam_application ea,exam_appl_details ead,syllabus_master sy,sub_sem ss,subject s,degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subtype_no=ss.subtype_no";
            strquery = strquery + " and ss.syll_code=sy.syll_code and ed.degree_code=d.degree_code and d.course_id=c.course_id and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + strmode + " " + stredu + " and sy.semester='" + ddlpsem.SelectedItem.ToString() + "'";
            DataSet dstype = da.select_method_wo_parameter(strquery, "text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = dstype;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    public void Psubject()
    {
        try
        {
            chklssubject.Items.Clear();
            chksubject.Checked = false;
            txtpsubject.Text = "---Select---";
            string strmode = string.Empty;
            string includeGeneratedSubjects = string.Empty;
            if (ddlpmode.Items.Count > 0)
            {
                if (Convert.ToString(ddlpmode.SelectedItem).Trim().ToLower() != "all" && Convert.ToString(ddlpmode.SelectedItem).Trim().ToLower() != "")
                {
                    strmode = " and c.type='" + Convert.ToString(ddlpmode.SelectedItem).Trim() + "'";
                }
            }

            string stredu = string.Empty;
            if (ddlpedu.Items.Count > 0)
            {
                if (Convert.ToString(ddlpedu.SelectedItem).Trim().ToLower() != "all" && Convert.ToString(ddlpedu.SelectedItem).Trim().ToLower() != "")
                {
                    stredu = " and c.edu_level='" + Convert.ToString(ddlpedu.SelectedItem).Trim() + "'";
                }
            }
            if (chkIncludeAlreadyAllotedSubjects.Checked)
            {
                includeGeneratedSubjects = " and s.subject_no not in( select et.subject_no from exmtt_det et,exmtt ed,subject s where et.subject_no=s.subject_no and et.exam_code=ed.exam_code and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "') ";
            }
            if (ddlsubtype.Items.Count > 0)
            {
                string strquery = " select distinct s.subject_code,s.subject_name+' - '+ s.subject_code as subjectname from exam_details ed,exam_application ea,exam_appl_details ead,syllabus_master sy,sub_sem ss,subject s,degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subtype_no=ss.subtype_no";
                strquery = strquery + " and ss.syll_code=sy.syll_code and ed.degree_code=d.degree_code and d.course_id=c.course_id and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' " + strmode + " " + stredu + " and sy.semester='" + ddlpsem.SelectedItem.ToString() + "' " + includeGeneratedSubjects + " order by subjectname,s.subject_code desc";
                DataSet dstype = da.select_method_wo_parameter(strquery, "text");
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    chklssubject.DataSource = dstype;
                    chklssubject.DataTextField = "subjectname";
                    chklssubject.DataValueField = "subject_code";
                    chklssubject.DataBind();
                }
            }
            else
            {
                lblperror.Visible = true;
                lblperror.Text = "Please Select The Subject Type";
            }
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void ddlpmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pedu();
        Psem();
        PSubtype();
        Psubject();
    }

    protected void ddlpedu_SelectedIndexChanged(object sender, EventArgs e)
    {
        Psem();
        PSubtype();
        Psubject();
    }

    protected void ddlpsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        PSubtype();
        Psubject();
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Psubject();
    }

    protected void chksubject_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chksubject.Checked == true)
            {
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = true;
                }
                txtpsubject.Text = "Subject (" + chklssubject.Items.Count + ")";
            }
            else
            {
                txtpsubject.Text = "---Select---";
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = false;
                }
            }

        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void chklssubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chksubject.Checked = false;
            txtpsubject.Text = "---Select---";
            int commcount = 0;
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                if (chklssubject.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtpsubject.Text = "Subject (" + commcount + ")";
                if (chklssubject.Items.Count == commcount)
                {
                    chksubject.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void chkIncludeAlreadyAllotedSubjects_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            PSubtype();
            Psubject();
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }


    protected void chkType_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkType.Checked == true)
            {
                for (int i = 0; i < cblType.Items.Count; i++)
                {
                    cblType.Items[i].Selected = true;
                }
                txtType.Text = "Type (" + cblType.Items.Count + ")";
            }
            else
            {
                txtType.Text = "---Select---";
                for (int i = 0; i < cblType.Items.Count; i++)
                {
                    cblType.Items[i].Selected = false;
                }
            }

        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void cblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkType.Checked = false;
            txtType.Text = "---Select---";
            int commcount = 0;
            for (int i = 0; i < cblType.Items.Count; i++)
            {
                if (cblType.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtType.Text = "Type (" + commcount + ")";
                if (cblType.Items.Count == commcount)
                {
                    chkType.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void btnpset_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean saveflag = false;
            string misssub = string.Empty;
            int commcount = 0;
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                if (chklssubject.Items[i].Selected == true)
                {
                    commcount++;
                }
            }
            if (commcount == 0)
            {
                lblperror.Visible = true;
                lblperror.Text = "Please Select The Subject And Then Proceed";
                return;
            }

            string strmode = string.Empty;
            if (ddlpmode.Items.Count > 0)
            {
                if (ddlpmode.SelectedItem.ToString().Trim().ToLower() != "all" && ddlpmode.SelectedItem.ToString().Trim() != "")
                {
                    strmode = " and c.type='" + ddlpmode.SelectedItem.ToString() + "'";
                }
            }

            string stredu = string.Empty;
            if (ddlpedu.Items.Count > 0)
            {
                if (ddlpedu.SelectedItem.ToString().Trim().ToLower() != "all" && ddlpedu.SelectedItem.ToString().Trim() != "")
                {
                    stredu = " and c.edu_level='" + ddlpedu.SelectedItem.ToString() + "'";
                }
            }

            if (ddlsubtype.Items.Count > 0)
            {
            }
            else
            {
                lblperror.Visible = true;
                lblperror.Text = "Please Select The Subject Type";
            }

            string[] spdate = txtpdate.Text.Split('/');
            DateTime dtt = Convert.ToDateTime(spdate[1] + '/' + spdate[0] + '/' + spdate[2]);
            string msession = ddlpsession.SelectedItem.ToString();

            string getdate = ddlpsession.SelectedValue.ToString();
            string[] spse = getdate.Split('@');
            string startdate = spse[0].ToString();
            string enddate = spse[1].ToString();

            string studentType = string.Empty;
            string val = string.Empty;
            if (chkType.Checked == true)
            {
                val = "2";//Both
            }
            else
            {
                for (int k = 0; k < cblType.Items.Count; k++)
                {
                    if (cblType.Items[k].Selected == true)
                    {
                        val = Convert.ToString(cblType.Items[k].Value);
                    }
                }
            }
            if (val == "0")
            {
                studentType = "and ead.attempts=0";
            }
            if (val == "1")
            {
                studentType = " and ead.attempts>0";
            }

            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                if (chklssubject.Items[i].Selected == true)
                {
                    string subcode = chklssubject.Items[i].Value.ToString();

                    string strquery = " select distinct d.college_code,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_no from exam_details ed,exam_application ea,exam_appl_details ead,syllabus_master sy,sub_sem ss,subject s,degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subtype_no=ss.subtype_no";
                    strquery = strquery + " and ss.syll_code=sy.syll_code and ed.degree_code=d.degree_code and d.course_id=c.course_id and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + strmode + " " + stredu + " and sy.semester='" + ddlpsem.SelectedItem.ToString() + "' and s.subject_code='" + subcode + "';";
                    strquery = strquery + " select ea.roll_no from exam_details ed,exam_application ea,exam_appl_details ead,syllabus_master sy,sub_sem ss,subject s,degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subtype_no=ss.subtype_no";
                    strquery = strquery + " and ss.syll_code=sy.syll_code and ed.degree_code=d.degree_code and d.course_id=c.course_id and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + strmode + " " + stredu + " and sy.semester='" + ddlpsem.SelectedItem.ToString() + "' and s.subject_code='" + subcode + "' "+studentType+"";
                    DataSet dstype = da.select_method_wo_parameter(strquery, "text");

                    string binroll = string.Empty;
                    for (int subr = 0; subr < dstype.Tables[1].Rows.Count; subr++)
                    {
                        if (binroll == "")
                        {
                            binroll = "'" + dstype.Tables[1].Rows[subr]["roll_no"].ToString() + "'";
                        }
                        else
                        {
                            binroll = binroll + ",'" + dstype.Tables[1].Rows[subr]["roll_no"].ToString() + "'";
                        }
                    }
                    //Rajkumar 12/5/2017
                    //string studentType = string.Empty;
                   
                   
                    string checkroll = "select count(ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and et.exam_session='" + msession + "' and ea.roll_no in ( " + binroll + ")";
                   
                    int getsubjectstucount = Convert.ToInt32(da.GetFunction(checkroll));
                    if (getsubjectstucount == 0)
                    {
                        for (int subr = 0; subr < dstype.Tables[0].Rows.Count; subr++)
                        {
                            string batchyear = dstype.Tables[0].Rows[subr]["batch_year"].ToString();
                            string degreecode = dstype.Tables[0].Rows[subr]["degree_code"].ToString();
                            string sem = dstype.Tables[0].Rows[subr]["current_semester"].ToString();
                            string subjectno = dstype.Tables[0].Rows[subr]["subject_no"].ToString();
                            string collegecode = dstype.Tables[0].Rows[subr]["college_code"].ToString();

                            string exm = "if not exists(select * from exmtt where degree_code='" + degreecode.ToString() + "' and batchFrom='" + batchyear.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + degreecode.ToString() + "','" + batchyear.ToString() + "','" + batchyear.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + sem.ToString() + "') select * from exmtt where degree_code='" + degreecode.ToString() + "' and batchFrom='" + batchyear.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                            int s = da.update_method_wo_parameter(exm, "text");
                            DataSet ds2allarr = new DataSet();
                            ds2allarr = da.select_method_wo_parameter(exm, "text");
                            if (ds2allarr.Tables[0].Rows.Count > 0)
                            {
                                string delval = "delete from exmtt_det where subject_no='" + subjectno.ToString() + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and exam_type='Univ' and StudentType='"+val+"'";
                                int delva = da.update_method_wo_parameter(delval, "text");

                                string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                //string save = "if exists(select * from exmtt_det where subject_no='" + subjectno.ToString() + "' and exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegecode + "' and exam_type='Univ') update exmtt_det set subject_no='" + subjectno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt.ToString("MM/dd/yyyy") + "', exam_session='" + msession + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + collegecode + "' , exam_type='Univ' where subject_no='" + subjectno.ToString() + "' and exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegecode + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + subjectno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt.ToString("MM/dd/yyyy") + "','" + msession + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + collegecode + "' ,'Univ')";//Val
                               //Rajkumar 5/12/2017
                                string save = "if exists(select * from exmtt_det where subject_no='" + subjectno.ToString() + "' and exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegecode + "' and exam_type='Univ' and StudentType='"+val+"') update exmtt_det set subject_no='" + subjectno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt.ToString("MM/dd/yyyy") + "', exam_session='" + msession + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + collegecode + "' , exam_type='Univ' where subject_no='" + subjectno.ToString() + "' and exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and exam_session='" + msession + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + collegecode + "' and exam_type='Univ' and StudentType='" + val + "'  else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type,StudentType) values('" + subjectno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt.ToString("MM/dd/yyyy") + "','" + msession + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + collegecode + "' ,'Univ','" + val + "')";
                                int v = da.update_method_wo_parameter(save, "text");
                                saveflag = true;
                            }
                        }
                    }
                    else
                    {
                        if (misssub.Trim() != "")
                        {
                            misssub = misssub + ", " + chklssubject.Items[i].Text.ToString();
                        }
                        else
                        {
                            misssub = chklssubject.Items[i].Text.ToString();
                        }
                    }
                }
            } lblperror.Visible = true;
            string strvalidateerro = string.Empty;
            if (saveflag == true)
            {
                strvalidateerro = "Subject Exam Date Altered Saved Successfully";
            }
            else
            {
                strvalidateerro = "Subject Exam Date Alter is Failed";
            }
            if (misssub.Trim() != "")
            {
                strvalidateerro = strvalidateerro + " <br/> Following Subjects Can't Move : " + misssub + "";
            }
            lblperror.Text = strvalidateerro;
        }
        catch (Exception ex)
        {
            lblperror.Visible = true;
            lblperror.Text = ex.ToString();
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        PSubAddtt.Visible = false;
    }

    #region Deleting Subjects added by Prabha

    protected void chckdeletesubjects_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chckdeletesubjects.Checked)
            {
                btndeletesubject.Visible = true;
                divInsideMove.Visible = false;
            }
            else
            {
                btndeletesubject.Visible = false;
                divInsideMove.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void btndeletesubject_OnClick(object sender, EventArgs e)
    {
        try
        {
            int res = 0;
            int selected = 0;
            Fptimetable.SaveChanges();
            for (int r = 0; r < Fptimetable.Sheets[0].RowCount; r++)
            {
                int selval = Convert.ToInt32(Fptimetable.Sheets[0].Cells[r, 3].Value);
                if (selval == 1)
                {
                    selected++;
                }
            }
            string delqry = string.Empty;
            if (selected > 0)
            {
                for (int r = 0; r < Fptimetable.Sheets[0].RowCount; r++)
                {
                    int selval = Convert.ToInt32(Fptimetable.Sheets[0].Cells[r, 3].Value);
                    if (selval == 1)
                    {
                        string examdate = Fptimetable.Sheets[0].Cells[r, 0].Tag.ToString();
                        DateTime dt = Convert.ToDateTime(examdate);

                        string examsession = Fptimetable.Sheets[0].Cells[r, 0].Note.ToString();
                        string subcode = Fptimetable.Sheets[0].Cells[r, 1].Tag.ToString();
                        string newCollegeCode = Convert.ToString(Fptimetable.Sheets[0].Cells[r, 2].Tag).Trim();

                        if (subcode.Split(',').Length > 1)
                        {
                            string[] SubCodeNew = subcode.Split(',');
                            for (int a = 0; a < SubCodeNew.Length; a++)
                            {
                                string subjCode = Convert.ToString(SubCodeNew[a]);
                                delqry = "delete  from exmtt_det where subject_no in (select subject_no from subject where subject_code in ( '" + subjCode + "')  ) and exam_date ='" + dt.ToString("MM/dd/yyyy") + "' and exam_session='" + examsession + "' and coll_code='" + newCollegeCode + "'";
                                res += dir.deleteData(delqry);
                            }
                        }
                        else
                        {
                            delqry = "delete  from exmtt_det where subject_no in (select subject_no from subject where subject_code = '" + subcode + "'  ) and exam_date ='" + dt.ToString("MM/dd/yyyy") + "' and exam_session='" + examsession + "' and coll_code='" + newCollegeCode + "'";
                            res += dir.deleteData(delqry);
                        }

                       
                    }
                }
                if (res > 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Deleted Successfully";
                    btnView_Click(sender, e);
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Atleast One Student";
            }
        }
        catch
        {

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

    #endregion

}
