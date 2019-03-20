using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Globalization;
using System.Configuration;

public partial class Foil_Sheet_for_Internal_External : System.Web.UI.Page
{
    Hashtable has = new Hashtable();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    static ArrayList addr = new ArrayList();
    ArrayList addr1 = new ArrayList();
    ArrayList addarray = new ArrayList();
    DataSet dsgrid = new DataSet();
    DataTable dtgrid = new DataTable();
    string collegecode = "";
    string sec = "";
    string sectionset = "";
    string name = "";
    string actvalue = "";
    string subject = "";
    string subjectcode = "";
    int count = 0;
    string sec1 = "";
    int valuesec = 0;
    string valuesection = "";
    int count1 = 0;
    string sw = "";
    string valuecount = "";
    string course = "";

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
            collegecode = Session["collegecode"].ToString();
            Iblerror.Visible = false;
            chkconsolidate.Enabled = true;
            if (!IsPostBack)
            {
                chkmergecoll.Checked = true;
                ddlpdateexam.Enabled = false;
                showreport.Visible = false;
                Panel1.Visible = false;
                showreport.Visible = false;
                showgrid.Visible = false;
                Excel.Visible = false;
                addr.Clear();
                loadedu();
                loadyear();
                loadmonth();
                loadmdatesession();
                loadbatch();
                bindcourse();
                binddepartment();
                bindsem();
                loadSubjecttype();
                loadSubjectName();
                loadbundle();
                rbform1.Checked = true;
                chkbatch.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadedu()
    {
        try
        {
            dropcoursefilter.Items.Clear();

            string strquery = "select distinct Edu_Level from course where college_code='" + collegecode + "'";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dropcoursefilter.DataSource = ds;
                dropcoursefilter.DataTextField = "Edu_Level";
                dropcoursefilter.DataBind();
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
            DropExamyear.Items.Clear();
            DataSet ds = da.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropExamyear.DataSource = ds;
                DropExamyear.DataTextField = "Exam_year";
                DropExamyear.DataValueField = "Exam_year";
                DropExamyear.DataBind();
                DropExamyear.SelectedIndex = DropExamyear.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    public void loadmonth()
    {
        try
        {
            DropExammonth.Items.Clear();
            if (DropExamyear.Items.Count > 0)
            {
                DataSet ds = new DataSet();
                ds = da.Exammonth(DropExamyear.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DropExammonth.DataSource = ds;
                    DropExammonth.DataTextField = "monthName";
                    DropExammonth.DataValueField = "Exam_month";
                    DropExammonth.DataBind();
                    DropExammonth.SelectedIndex = DropExammonth.Items.Count - 1;
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    public void loadbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            if (DropExamyear.Items.Count > 0 && DropExammonth.Items.Count > 0 && chkbatch.Checked == true)
            {
                if (DropExammonth.Text != "")
                {
                    string strquery = "select distinct e.batchFrom as BatchYear from exmtt e,exmtt_det ex where  ex.coll_code='" + collegecode + "' and ex.exam_code=e.exam_code  and e.exam_month='" + DropExammonth.SelectedValue.ToString() + "' and e.exam_year='" + DropExamyear.SelectedItem.ToString() + "' order by BatchYear desc";
                    ds.Reset();
                    ds.Dispose();
                    ds = da.select_method_wo_parameter(strquery, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                        {
                            ddlbatch.Items.Add(ds.Tables[0].Rows[b]["BatchYear"].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    public void bindcourse()
    {
        try
        {
            ddldegree.Items.Clear();
            if (DropExamyear.Items.Count > 0 && DropExammonth.Items.Count > 0 && chksubwise.Checked == true)
            {
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    string group_user = Session["group_code"].ToString();
                    if (group_user.Contains(';'))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    grouporusercode = " and dp.group_code=" + group_user.Trim() + "";
                }
                else
                {
                    grouporusercode = " and dp.user_code=" + Session["usercode"].ToString().Trim() + "";
                }
                string addcollegecode = "and d.college_code='" + Session["collegecode"].ToString() + "'";
                if (chkmergecoll.Checked == true)
                {
                    addcollegecode = "";
                }
                string strdegrquery = "select distinct c.course_name,d.course_id  from degree d,department de,course c,deptprivilages dp,exmtt e where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code=dp.degree_code and d.college_code=c.college_code and d.Degree_Code=e.degree_code and e.Exam_month='" + DropExammonth.SelectedValue.ToString() + "' and e.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' " + addcollegecode + " " + grouporusercode + "";

                if (chkbatch.Checked == true)
                {
                    strdegrquery = strdegrquery + " and e.batchFrom='" + ddlbatch.SelectedValue.ToString() + "' ";
                }
                ds = da.select_method_wo_parameter(strdegrquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    public void binddepartment()
    {
        try
        {
            DropCourse.Items.Clear();
            if (DropExamyear.Items.Count > 0 && DropExammonth.Items.Count > 0 && chksubwise.Checked == true)
            {
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    string group_user = Session["group_code"].ToString();
                    if (group_user.Contains(';'))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    grouporusercode = " and dp.group_code=" + group_user.Trim() + "";
                }
                else
                {
                    grouporusercode = " and dp.user_code=" + Session["usercode"].ToString().Trim() + "";
                }

                string addcollegecode = "and d.college_code='" + Session["collegecode"].ToString() + "'";
                if (chkmergecoll.Checked == true)
                {
                    addcollegecode = "";
                }
                string strdegrquery = "select distinct d.degree_code,de.dept_name  from degree d,department de,course c,deptprivilages dp,exmtt e where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code=dp.degree_code and d.college_code=c.college_code and d.Degree_Code=e.degree_code and e.Exam_month='" + DropExammonth.SelectedValue.ToString() + "' and e.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and d.course_id='" + ddldegree.SelectedValue.ToString() + "' " + addcollegecode + " " + grouporusercode + "";
                if (chkbatch.Checked == true)
                {
                    strdegrquery = strdegrquery + " and e.batchFrom='" + ddlbatch.SelectedValue.ToString() + "' ";
                }
                ds = da.select_method_wo_parameter(strdegrquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DropCourse.DataSource = ds;
                    DropCourse.DataTextField = "dept_name";
                    DropCourse.DataValueField = "degree_code";
                    DropCourse.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
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
            DataSet ds = new DataSet();
            string strsql = "select Max(ndurations),first_year_nonsemester from ndegree where college_code=" + collegecode + " group by first_year_nonsemester order by Max(ndurations) ";
            ds = da.select_method_wo_parameter(strsql, "TExt");
            if (chkbatch.Checked == true && chksubwise.Checked == true && DropCourse.Items.Count > 0)
            {
                ds = da.BindSem(DropCourse.SelectedValue.ToString(), ddlbatch.SelectedValue.ToString(), collegecode);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    public void loadSubjecttype()
    {
        try
        {
            ddlsubtype.Items.Clear();
            ddlsubtype.Enabled = false;
            if (ddlsem.Items.Count > 0)
            {
                string sql = "select distinct ss.subject_type from exam_details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy where s.subject_no=ead.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and sy.degree_code=ed.degree_code and sy.Batch_Year=ed.batch_year and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and s.subject_no=ead.subject_no and sy.semester='" + ddlsem.SelectedValue.ToString() + "'and ed.exam_Month='" + DropExammonth.SelectedValue.ToString() + "'  and ed.Exam_Year='" + DropExamyear.SelectedValue.ToString() + "' ";
                if (chksubwise.Checked == true)
                {
                    sql = sql + " and ed.degree_code= '" + DropCourse.SelectedValue.ToString() + "'";
                }
                if (chkbatch.Checked == true)
                {
                    sql = sql + " and ed.batch_year= '" + ddlbatch.SelectedValue.ToString() + "'";
                }
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubtype.Enabled = true;
                    ddlsubtype.DataSource = ds;
                    ddlsubtype.DataTextField = "subject_type";
                    ddlsubtype.DataValueField = "subject_type";
                    ddlsubtype.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    public void loadSubjectName()
    {
        try
        {
            dropsubject.Items.Clear();
            dropsubject.Enabled = false;
            if (ddlsubtype.Items.Count > 0)
            {
                string sql = "select distinct s.subject_code,s.subject_name+' - '+s.subject_code as subname,s.subject_name from exam_details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy where s.subject_no=ead.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and sy.degree_code=ed.degree_code and sy.Batch_Year=ed.batch_year and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and s.subject_no=ead.subject_no and sy.semester='" + ddlsem.SelectedValue.ToString() + "'and ed.exam_Month='" + DropExammonth.SelectedValue.ToString() + "'  and ed.Exam_Year='" + DropExamyear.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";
                if (chksubwise.Checked == true)
                {
                    sql = sql + " and ed.degree_code= '" + DropCourse.SelectedValue.ToString() + "'";
                }
                if (chkbatch.Checked == true)
                {
                    sql = sql + " and ed.batch_year= '" + ddlbatch.SelectedValue.ToString() + "'";
                }
                sql = sql + " order by s.subject_name,s.subject_code desc";
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dropsubject.Enabled = true;
                    dropsubject.DataSource = ds;
                    dropsubject.DataTextField = "subname";
                    dropsubject.DataValueField = "subject_code";
                    dropsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    public void loadbundle()
    {
        try
        {
            Dropbundle.Items.Clear();
            string sql = "select distinct es.bundle_no from exmtt e,exmtt_det et,exam_seating es,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and es.subject_no=s.subject_no and e.exam_month='" + DropExammonth.SelectedItem.Value + "' and e.exam_year='" + DropExamyear.SelectedItem.Value + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "' and es.bundle_no is not null and es.bundle_no<>''";
            if (chkbatch.Checked == true)
            {
                sql = sql + " and e.batchfrom='" + ddlbatch.SelectedValue.ToString() + "'";
            }
            if (chksubwise.Checked == true)
            {
                sql = sql + " and e.degree_code='" + DropCourse.SelectedValue + "'";
            }
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Dropbundle.Enabled = true;
                Dropbundle.DataSource = ds;
                Dropbundle.DataTextField = "bundle_no";
                Dropbundle.DataBind();
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }

    }
    public void clear()
    {
        AttSpreadfoil.Visible = false;
        txtexcelname.Visible = false;
        IblRpt.Visible = false;
        printpdf.Visible = false;
        printexcel.Visible = false;
        showreport.Visible = false;
        Excel.Visible = false;
        btnpdf.Visible = false;
        Iblerr.Visible = false;
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadmonth();
        loadbatch();
        bindcourse();
        binddepartment();
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadmdatesession();
        loadbatch();
        bindcourse();
        binddepartment();
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        loadbatch();
        if (chksubwise.Checked == false)
        {
            bindcourse();
            binddepartment();
        }
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void chksubwise_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            loadbatch();
        }
        if (chksubwise.Checked == true)
        {
            bindcourse();
            binddepartment();
        }
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void ddlbatch_selected(object sender, EventArgs e)
    {
        bindcourse();
        binddepartment();
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void ddldegree_selected(object sender, EventArgs e)
    {
        binddepartment();
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void dropcourse_selected(object sender, EventArgs e)
    {
        bindsem();
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void ddlsem_selected(object sender, EventArgs e)
    {
        loadSubjecttype();
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void ddlsubtype_selected(object sender, EventArgs e)
    {
        loadSubjectName();
        loadbundle();
        clear();
    }
    protected void dropsubject_selected(object sender, EventArgs e)
    {
        loadbundle();
        clear();
    }
    protected void dropbundle_selected(object sender, EventArgs e)
    {
        clear();
    }

    protected void closepanel(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        showgrid.Visible = false;
    }

    protected void linksetting(object sender, EventArgs e)
    {
        try
        {
            Panel1.Visible = true;
            txt_section.Visible = true;
            txtSerials.Visible = true;
            modelpopsetting.Show();
            showreport.Visible = false;
            btnsubmit.Visible = false;
            FpFoilSetting.Visible = false;
            printpdf.Visible = false;
            printexcel.Visible = false;
            Excel.Visible = false;
            btnpdf.Visible = false;
            btnok.Visible = false;
        }

        catch (Exception ex)
        {
        }
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
        }
    }

    protected void buttongoClick(object sender, EventArgs e)
    {
        try
        {
            modelpopsetting.Show();
            Panel1.Visible = true;
            btnsubmit.Visible = false;
            showgrid.Visible = true;
            CreateGridView();
            printpdf.Visible = false;
            printexcel.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    public void CreateGridView()
    {
        try
        {
            valuesec = Convert.ToInt32(txt_section.Text.Trim());
            valuesection = valuesec.ToString();
            sec = txtSerials.Text.Trim();
            addr.Add("A");
            addr.Add("B");
            addr.Add("C");
            addr.Add("D");
            addr.Add("E");
            addr.Add("F");
            addr.Add("G");
            addr.Add("H");
            addr.Add("I");
            addr.Add("J");
            addr.Add("K");
            addr.Add("L");
            addr.Add("M");
            addr.Add("O");
            addr.Add("P");
            addr.Add("Q");
            addr.Add("R");
            addr.Add("S");
            addr.Add("T");
            addr.Add("U");
            addr.Add("V");
            addr.Add("W");
            addr.Add("X");
            addr.Add("Y");
            addr.Add("Z");
            count1 = addr.Count;
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("serials", typeof(string));
            dt.Columns.Add("columns", typeof(int));

            for (int i = 0; i < valuesec; i++)
            {
                sec1 = Convert.ToString(addr[i]);
                sectionset = sec + "-" + sec1;
                dt.Rows.Add("", sectionset, null);

                btnok.Visible = true;
            }

            showgrid.DataSource = dt;
            showgrid.DataBind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            printexcel.Visible = false;
            printpdf.Visible = false;
            btnsubmit.Visible = false;
            modelpopsetting.Show();
            FpFoilSetting.Visible = false;
            FpFoilSetting.CommandBar.Visible = false;
            string totrow = txt_section.Text.ToString();
            if (totrow.Trim() != "")
            {
                int tova = Convert.ToInt32(totrow);
                if (tova > 0)
                {
                    FpFoilSetting.Sheets[0].RowCount = tova;

                    int Totcol = 0;
                    for (int val = 0; val < showgrid.Rows.Count; val++)
                    {
                        name = ((showgrid.Rows[val].FindControl("txt_column") as TextBox).Text);
                        if (name.Trim() != "" && name != null)
                        {
                            if (Totcol < Convert.ToInt32(name))
                            {
                                Totcol = Convert.ToInt32(name);
                            }
                        }
                    }
                    if (Totcol > 0)
                    {
                        Totcol++;
                        FpFoilSetting.Sheets[0].ColumnCount = Totcol;
                        FpFoilSetting.Visible = true;
                        btnsubmit.Visible = true;
                        for (int val = 0; val < showgrid.Rows.Count; val++)
                        {

                            string marow = ((showgrid.Rows[val].FindControl("txt_column") as TextBox).Text);
                            string secname = ((showgrid.Rows[val].FindControl("Iblsection") as Label).Text);


                            FpFoilSetting.Sheets[0].Cells[val, 0].Locked = true;
                            FpFoilSetting.Sheets[0].Cells[val, 0].Text = secname;

                            if (marow.Trim() != "" && marow != null)
                            {
                                int maxrowcol = Convert.ToInt32(marow);
                                for (int i = 1; i < FpFoilSetting.Sheets[0].ColumnCount; i++)
                                {
                                    if (maxrowcol >= i)
                                    {
                                        FpFoilSetting.Sheets[0].Cells[val, i].Locked = false;
                                        FpFoilSetting.Sheets[0].Cells[val, i].BackColor = System.Drawing.Color.White;
                                    }
                                    else
                                    {
                                        FpFoilSetting.Sheets[0].Cells[val, i].Locked = true;
                                        FpFoilSetting.Sheets[0].Cells[val, i].BackColor = System.Drawing.Color.Violet;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btngo_grid(object sender, EventArgs e)
    {
        try
        {
            FpFoilSetting.SaveChanges();
            string type = dropcoursefilter.SelectedItem.ToString();
            string delset = "delete from tbl_foil_card where type='" + type + "' and college_code='" + Session["collegecode"].ToString() + "'";
            int insorupd = da.update_method_wo_parameter(delset, "text");
            for (int i = 0; i < FpFoilSetting.Rows.Count; i++)
            {
                string strsecname = FpFoilSetting.Sheets[0].Cells[i, 0].Text;
                string strvalu = "";
                string column = ((showgrid.Rows[i].FindControl("txt_column") as TextBox).Text);
                for (int j = 1; j < FpFoilSetting.Columns.Count; j++)
                {
                    if (FpFoilSetting.Sheets[0].Cells[i, j].Locked == false)
                    {
                        valuecount = FpFoilSetting.Sheets[0].Cells[i, j].Text;

                        if (strvalu == "")
                        {
                            strvalu = valuecount;
                        }
                        else
                        {
                            strvalu = strvalu + ',' + valuecount;
                        }

                    }
                }

                string strinset = "insert into tbl_foil_card(type,section_name,no_col,value,college_code) values('" + type + "','" + strsecname + "','" + column + "','" + strvalu + "','" + Session["collegecode"].ToString() + "')";
                insorupd = da.update_method_wo_parameter(strinset, "text");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            if (chkconsolidate.Checked == false)
            {
                AttSpreadfoil.Visible = false;
                printexcel.Visible = false;
                printpdf.Visible = false;
                DataView dtview = new DataView();
                DataView dtview1 = new DataView();
                DataSet dnewset = new DataSet();
                Panel1.Visible = false;
                Excel.Visible = false;
                DataRow dr = null;
                printpdf.Visible = false;
                printexcel.Visible = false;

                bool isRandom = false;
                bool isSubjectWise = false;
                string userCode = string.Empty;
                if (Session["usercode"] != null)
                {
                    userCode = Convert.ToString(Session["usercode"]).Trim();
                }
                string dummyType = string.Empty;
                string dummyMode = string.Empty;

                //Added by jairam 02-05-2017 
                bool ShowDummy = false;
                string saveDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowDummyNumberOnMarkEntryCOE' and user_code ='" + userCode + "'  ").Trim();
                if (saveDummy == "1")
                {
                    ShowDummy = true;
                }
                Session["DummyCheck"] = ShowDummy;
                if (ShowDummy)
                {
                    if (!string.IsNullOrEmpty(userCode))
                    {
                        dummyType = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and user_code ='" + userCode + "'");//and college_code ='13' 
                        dummyMode = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and user_code ='" + userCode + "'");//and college_code ='13'
                    }
                    if (string.IsNullOrEmpty(dummyMode.Trim()) || dummyMode.Trim() == "0")
                    {
                        isRandom = false;
                    }
                    else
                    {
                        isRandom = true;
                    }
                    if (string.IsNullOrEmpty(dummyType.Trim()) || dummyType.Trim() == "0")
                    {
                        isSubjectWise = false;
                    }
                    else
                    {
                        isSubjectWise = true;
                    }
                    string qryDummyNo = string.Empty;
                    string qryDummyMode = string.Empty;
                    string qryDummyType = string.Empty;
                    if (isRandom)
                    {
                        qryDummyMode = " dummy_type='1'";
                    }
                    else
                    {
                        qryDummyMode = " dummy_type='0'";
                    }

                    dnewset = da.select_method_wo_parameter("select subject,roll_no,regno,Dummy_no,subject_no from dummynumber where " + qryDummyMode + " and exam_month='" + DropExammonth.SelectedItem.Value + "' and Exam_year='" + DropExamyear.SelectedItem.Text + "'", "Text");

                }

                dt.Columns.Add("S.No", typeof(string));
                if (!ShowDummy)
                {
                    dt.Columns.Add("Reg No", typeof(string));
                }
                else
                {
                    dt.Columns.Add("Dummy No", typeof(string));
                }
                dt.Columns.Add("Total Marks", typeof(string));
                if (rbform1.Checked == true)
                {
                    dt.Columns.Add("Marks in Words", typeof(string));
                }

                string gettype = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "'");
                string strgetset = "select type,section_name,no_col,value,college_code from tbl_foil_card where type='" + gettype + "' order by section_name";
                DataSet dssetiin = da.select_method_wo_parameter(strgetset, "text");
                if (dssetiin.Tables[0].Rows.Count > 0)
                {
                    dssetiin.Tables[0].DefaultView.RowFilter = "type='" + gettype + "'";
                    dtview = dssetiin.Tables[0].DefaultView;
                    if (dtview.Count > 0)
                    {
                        for (int i = 0; i < dtview.Count; i++)
                        {
                            string noofcolum = dtview[i]["no_col"].ToString();
                            string strgetcolvalue = dtview[i]["value"].ToString();
                            string[] spva = strgetcolvalue.Split(',');
                            for (int inv = 0; inv <= spva.GetUpperBound(0); inv++)
                            {
                                actvalue = spva[inv].ToString();
                                dt.Columns.Add(actvalue);
                            }
                        }
                    }

                    string query = "";
                    string fromdate = "";
                    if (rbform1.Checked == true)
                    {
                        fromdate = "select distinct Convert(nvarchar(15),et.exam_date,103) from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + DropExammonth.SelectedValue.ToString() + "' and e.Exam_year='" + DropExamyear.SelectedValue.ToString() + "' and s.subject_code=''" + dropsubject.SelectedValue.ToString() + "''";
                        if (chkbatch.Checked == true)
                        {
                            fromdate = fromdate + " and e.batchfrom='" + ddlbatch.SelectedValue.ToString() + "'";
                        }
                        if (chksubwise.Checked == true)
                        {
                            fromdate = fromdate + " and e.degree_code='" + DropCourse.SelectedValue.ToString() + "'";
                        }
                        fromdate = da.GetFunction(fromdate);
                    }
                    dr = dt.NewRow();
                    subject = dropsubject.SelectedValue;
                    string strbundle = "";
                    if (Dropbundle.Items.Count > 0 && Dropbundle.Enabled == true)
                    {
                        if (Dropbundle.SelectedItem.ToString() != "")
                        {
                            strbundle = " and es.bundle_no='" + Dropbundle.SelectedItem.ToString() + "'";
                        }
                    }
                    string Exam_date = da.GetFunction("select distinct top 1 convert(varchar(10), exam_date,101) as examNEw,exam_date  from exmtt t,exmtt_det tt where t.exam_code=tt.exam_code and t.exam_month=" + DropExammonth.SelectedValue.ToString() + " and t.exam_year=" + DropExamyear.SelectedItem.Text.ToString() + " order by exam_date asc");

                    if (chkIIIval.Checked == false)
                    {
                        if (Cb_CheckBox.Checked == true)
                        {
                            query = "select r.Reg_No,es.roomno,es.seat_no,s.subject_code,s.max_ext_marks from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r,exam_seating es,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and es.regno=r.Reg_No and es.subject_no=ead.subject_no and s.subject_no=es.subject_no and s.subject_no=ead.subject_no and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "' " + strbundle + "";
                            if (chkbatch.Checked == true)
                            {
                                query = query + " and ed.batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
                            }
                            if (chksubwise.Checked == true)
                            {
                                query = query + " and ed.degree_code='" + DropCourse.SelectedValue.ToString() + "'";
                            }
                            if (Exam_date.Trim() != "" && Exam_date.Trim() != "0")
                            {
                                query = query + " and edate >='" + Exam_date + "'";
                            }
                            query = query + " order by ed.batch_year desc,ed.degree_code,r.Reg_No,es.roomno,es.seat_no";
                        }
                        if (Cb_CheckBox.Checked == false)
                        {
                            query = " select r.Reg_No,s.subject_code,s.max_ext_marks from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and s.subject_code='" + dropsubject.SelectedValue.ToString() + "' and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "'";
                            if (chkbatch.Checked == true)
                            {
                                query = query + " and ed.batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
                            }
                            if (chksubwise.Checked == true)
                            {
                                query = query + " and ed.degree_code='" + DropCourse.SelectedValue.ToString() + "'";
                            }
                            query = query + " order by ed.batch_year desc,ed.degree_code,r.Reg_No";
                        }
                        query = query + " select collname from collinfo";
                        ds = da.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            subjectcode = Convert.ToString(ds.Tables[0].Rows[0]["subject_code"]);
                            for (int temp = 0; temp < ds.Tables[0].Rows.Count; temp++)
                            {
                                dr = dt.NewRow();
                                string str5 = Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]);
                                if (!addarray.Contains(str5))
                                {
                                    count++;
                                    addarray.Add(str5);
                                }
                                string RegNoNew = string.Empty;
                                RegNoNew = Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]);
                                if (ShowDummy == true)
                                {

                                    if (dnewset.Tables.Count > 0 && dnewset.Tables[0].Rows.Count > 0)
                                    {
                                        if (!isSubjectWise)
                                        {
                                            dnewset.Tables[0].DefaultView.RowFilter = "regno='" + Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]) + "'";
                                        }
                                        else
                                        {
                                            dnewset.Tables[0].DefaultView.RowFilter = "regno='" + Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]) + "' and subject='" + dropsubject.SelectedValue.ToString() + "'";
                                        }
                                        DataView dv = dnewset.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            RegNoNew = Convert.ToString(dv[0]["Dummy_no"]);
                                        }

                                    }

                                }
                                dr[0] = Convert.ToString(count);
                                dr[1] = RegNoNew.ToString();
                                dt.Rows.Add(dr);
                            }

                            Panel1.Visible = false;
                            modelpopsetting.Hide();
                            showreport.DataSource = dt;
                            showreport.DataBind();
                            AttSpreadfoil.Visible = false;
                            printpdf.Visible = false;
                            printexcel.Visible = false;
                            showreport.Visible = true;
                            Excel.Visible = true;
                            btnpdf.Visible = true;
                        }
                        else
                        {

                            Iblerror.Text = "No Records Found";
                            Iblerror.Visible = true;

                        }
                    }
                    else
                    {
                        Double Mark_Difference1 = 0;
                        string Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
                        if (Mark_Difference != "")
                        {
                            Mark_Difference1 = Convert.ToDouble(Mark_Difference);
                        }
                        else
                        {
                            Mark_Difference1 = 0;
                        }
                        query = "select r.batch_year,r.degree_code,r.Reg_No,s.subject_code,s.max_ext_marks,m.evaluation1,m.evaluation2,m.evaluation3 from Exam_Details ed,Registration r,subject s,mark_entry m where m.exam_code=ed.exam_code  and m.roll_no=r.Roll_No and m.subject_no=s.subject_no and isnull(m.evaluation1,'0')>=0 and isnull(m.evaluation2,'0')>=0 and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "'";
                        if (chkbatch.Checked == true)
                        {
                            query = query + " and ed.batch_year='" + ddlbatch.Text + "'";
                        }
                        if (chksubwise.Checked == true)
                        {
                            query = query + " and ed.degree_code='" + DropCourse.Text + "'";
                        }
                        query = query + " order by r.batch_year desc,r.degree_code,r.Reg_No";
                        query = query + " select collname from collinfo";
                        ds = da.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            subjectcode = Convert.ToString(ds.Tables[0].Rows[0]["subject_code"]);
                            for (int temp = 0; temp < ds.Tables[0].Rows.Count; temp++)
                            {
                                //magesh 23.5.18
                                // Double ev1 = Convert.ToDouble(ds.Tables[0].Rows[temp]["evaluation1"]);
                               // Double ev2 = Convert.ToDouble(ds.Tables[0].Rows[temp]["evaluation2"]);
                                string es = Convert.ToString(ds.Tables[0].Rows[temp]["evaluation1"]);
                                Double ev1 = 0.00;
                              
                                double.TryParse(es, out ev1);
                                string es1 = Convert.ToString(ds.Tables[0].Rows[temp]["evaluation2"]);
                                Double ev2 = 0.00;

                                double.TryParse(es1, out ev2);//magesh 23.5.18
                               
                                Double evdif = ev1 - ev2;
                                evdif = Math.Abs(evdif);
                               // if (evdif >= Mark_Difference1 && Mark_Difference1 > 0 || ev1 == 0 && ev1 > Mark_Difference1 && ev1 > Mark_Difference1 || ev2==0)
                                if (evdif >= Mark_Difference1 && Mark_Difference1 > 0  && ev1 > Mark_Difference1 && ev1 > Mark_Difference1 ) //modified by Mullai
                                {
                                    dr = dt.NewRow();
                                    string str5 = Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]);
                                    if (!addarray.Contains(str5))
                                    {
                                        count++;
                                        addarray.Add(str5);
                                    }
                                    string RegNoNew = string.Empty;
                                    RegNoNew = Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]);
                                    if (ShowDummy == true)
                                    {

                                        if (dnewset.Tables.Count > 0 && dnewset.Tables[0].Rows.Count > 0)
                                        {
                                            if (!isSubjectWise)
                                            {
                                                dnewset.Tables[0].DefaultView.RowFilter = "regno='" + Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]) + "'";
                                            }
                                            else
                                            {
                                                dnewset.Tables[0].DefaultView.RowFilter = "regno='" + Convert.ToString(ds.Tables[0].Rows[temp]["reg_no"]) + "' and subject='" + dropsubject.SelectedValue.ToString() + "'";
                                            }
                                            DataView dv = dnewset.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                RegNoNew = Convert.ToString(dv[0]["Dummy_no"]);
                                            }
                                        }
                                    }

                                    dr[0] = Convert.ToString(count);
                                    dr[1] = RegNoNew.ToString();
                                    dt.Rows.Add(dr);
                                }
                            }

                            Panel1.Visible = false;
                            modelpopsetting.Hide();
                            showreport.DataSource = dt;
                            showreport.DataBind();
                            AttSpreadfoil.Visible = false;
                            printpdf.Visible = false;
                            printexcel.Visible = false;
                            showreport.Visible = true;
                            Excel.Visible = true;
                            btnpdf.Visible = true;
                        }
                        else
                        {

                            Iblerror.Text = "No Records Found";
                            Iblerror.Visible = true;

                        }
                    }
                }
                else
                {
                    Iblerror.Text = "Please Enter Settings";
                    Iblerror.Visible = true;
                }

                //}
                //else
                //{
                //    Iblerror.Text = "Please Allot Bundle No";
                //    Iblerror.Visible = true;
                //}
            }
            else if (chkconsolidate.Checked == true)
            {
                display();
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    public void display()
    {
        try
        {
            AttSpreadfoil.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            AttSpreadfoil.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            showreport.Visible = false;
            btnpdf.Visible = false;
            Excel.Visible = false;
            AttSpreadfoil.Sheets[0].RowCount = 0;
            AttSpreadfoil.Sheets[0].ColumnCount = 0;
            AttSpreadfoil.Sheets[0].ColumnCount = 8;
            AttSpreadfoil.Sheets[0].ColumnHeader.RowCount = 1;
            AttSpreadfoil.CommandBar.Visible = false;
            AttSpreadfoil.Sheets[0].SheetCorner.ColumnCount = 0;
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 3].Text = "  Degree - Branch";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Bundle No";
            AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total";
            AttSpreadfoil.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            AttSpreadfoil.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpreadfoil.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            AttSpreadfoil.Sheets[0].DefaultStyle.Font.Bold = false;
            AttSpreadfoil.Sheets[0].Columns[0].Width = 50;
            AttSpreadfoil.Sheets[0].Columns[1].Width = 50;
            AttSpreadfoil.Sheets[0].Columns[2].Width = 80;
            AttSpreadfoil.Sheets[0].Columns[3].Width = 382;
            AttSpreadfoil.Sheets[0].Columns[4].Width = 100;
            AttSpreadfoil.Sheets[0].Columns[5].Width = 130;
            AttSpreadfoil.Sheets[0].Columns[6].Width = 80;

            Boolean reportflag = false;

            AttSpreadfoil.Sheets[0].Columns[5].Visible = false;
            string collgr = Session["collegecode"].ToString();
            string examdate = ddlpdateexam.SelectedItem.Text.ToString();
            string[] dsplit = examdate.Split('/');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();

            string sessiond = "";
            //if (Dropsession.SelectedItem.Text == "Both")
            //{
            //    sessiond = "";
            //}
            //else
            //{
            //    sessiond = "  and es.ses_sion='" + Dropsession.SelectedItem.Text + "'";
            //}

            string strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = da.select_method_wo_parameter(strquery, "Text");
            string sml = da.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            if (sml != null && sml.Trim() != "" && sml.Trim() != "0")
            {
                string addcollegecode = "and r.college_code='" + collegecode + "'";
                if (chkmergecoll.Checked == true)
                {
                    addcollegecode = "";
                }
                string query1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,course_name,dp.dept_name,r.batch_year,es.subject_no, es.bundle_no,(select subject_code from subject s where  es.subject_no=s.subject_no) as subjectcode  from registration r,exam_details ed,exam_application ea, exam_appl_details ead,exam_seating as es,degree d,course c,department dp where  ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no  and r.exam_flag<>'Debar' and es.regno=r.Reg_No   and ead.subject_no=es.subject_no   and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and    dp.dept_code=d.dept_code and d.college_code=r.college_code and d.Course_Id = c.Course_Id and ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "' " + addcollegecode + " and es.edate='" + examdate + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,course_name,  dp.dept_name,r.batch_year,es.subject_no,es.bundle_no order by subjectcode ";
                DataSet dset1 = new DataSet();

                dset1.Clear();

                dset1 = da.select_method_wo_parameter(query1, "Text");

                string hallno = "";
                string dept = "";
                string subcode = "";
                string strngth = "";
                string deptnam = "";
                string bundleno = "";
                int sno = 1;

                FarPoint.Web.Spread.CheckBoxCellType cheselectall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType cheall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cheselectall1.AutoPostBack = true;
                if (dset1.Tables[0].Rows.Count > 0)
                {



                    AttSpreadfoil.Width = 850;
                    AttSpreadfoil.Height = 500;
                    AttSpreadfoil.Visible = true;
                    txtexcelname.Visible = true;
                    IblRpt.Visible = true;
                    reportflag = true;
                    AttSpreadfoil.Sheets[0].RowCount++;
                    AttSpreadfoil.Sheets[0].Cells[0, 1].CellType = cheselectall1;
                    AttSpreadfoil.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    //AttSpreadfoil.Sheets[0].SpanModel.Add(0, 2, 0, 2);


                    for (int mm = 0; mm < dset1.Tables[0].Rows.Count; mm++)
                    {
                        AttSpreadfoil.Sheets[0].RowCount++;
                        hallno = dset1.Tables[0].Rows[mm]["roomno"].ToString();
                        dept = dset1.Tables[0].Rows[mm]["dept_name"].ToString();
                        subcode = dset1.Tables[0].Rows[mm]["subject_no"].ToString();
                        strngth = dset1.Tables[0].Rows[mm]["strength"].ToString();
                        deptnam = dset1.Tables[0].Rows[mm]["course_name"].ToString();
                        bundleno = dset1.Tables[0].Rows[mm]["bundle_no"].ToString();
                        if (bundleno == "692")
                        {
                        }
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 1].CellType = cheall1;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 1].Value = 0;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 2].Text = hallno;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 3].Text = deptnam + " - " + dept;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 3].Tag = dset1.Tables[0].Rows[mm]["degree_code"].ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 6].Text = bundleno;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 4].Text = dset1.Tables[0].Rows[mm]["subjectcode"].ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 4].Tag = dset1.Tables[0].Rows[mm]["subject_no"].ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        //if (subcode != "")
                        //{
                        //    string subname = "select subject_code from subject where subject_no='" + subcode + "'";
                        //    DataSet dssubname = da.select_method_wo_parameter(subname, "text");
                        //    string subname1 = dssubname.Tables[0].Rows[0]["subject_code"].ToString();
                        //    AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 4].Text = subname1;
                        //    AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //}
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 7].Text = strngth;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        sno++;
                    }
                    AttSpreadfoil.SaveChanges();
                    AttSpreadfoil.Sheets[0].PageSize = AttSpreadfoil.Sheets[0].Rows.Count;
                    //AttSpreadfoil.Height = AttSpreadfoil.Sheets[0].Rows.Count;
                    printexcel.Visible = true;
                    printpdf.Visible = true;
                }
                else
                {
                    Iblerror.Text = "No Records Found";
                    Iblerror.Visible = true;
                    printpdf.Visible = false;
                    printexcel.Visible = false;
                    AttSpreadfoil.Visible = false;
                    txtexcelname.Visible = false;
                    IblRpt.Visible = false;
                }
            }
            else
            {
                AttSpreadfoil.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                AttSpreadfoil.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                showreport.Visible = false;
                btnpdf.Visible = false;
                Excel.Visible = false;
                AttSpreadfoil.Sheets[0].RowCount = 0;
                AttSpreadfoil.Sheets[0].ColumnCount = 4;
                AttSpreadfoil.Sheets[0].ColumnHeader.RowCount = 1;
                AttSpreadfoil.CommandBar.Visible = false;
                AttSpreadfoil.Sheets[0].SheetCorner.ColumnCount = 0;

                FarPoint.Web.Spread.CheckBoxCellType cheall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType cheselectall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cheselectall1.AutoPostBack = true;
                AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                AttSpreadfoil.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total";
                AttSpreadfoil.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                AttSpreadfoil.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                AttSpreadfoil.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                AttSpreadfoil.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                AttSpreadfoil.Sheets[0].DefaultStyle.Font.Bold = false;
                AttSpreadfoil.Sheets[0].Columns[0].Width = 60;
                AttSpreadfoil.Sheets[0].Columns[1].Width = 100;
                AttSpreadfoil.Sheets[0].Columns[2].Width = 100;
                AttSpreadfoil.Sheets[0].Columns[3].Width = 100;
                string collgr1 = Session["collegecode"].ToString();

                string addcollegecode = "and r.college_code='" + collegecode + "'";
                if (chkmergecoll.Checked == true)
                {
                    addcollegecode = "";
                }
                string query1 = " select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,dp.dept_name,r.batch_year,es.subject_no  from registration r,exam_details ed,exam_application ea, exam_appl_details ead,exam_seating as es,degree d,department dp where  ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no  and r.exam_flag<>'Debar' and es.regno=r.Reg_No   and ead.subject_no=es.subject_no   and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and       ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and    dp.dept_code=d.dept_code       and d.college_code=r.college_code  and    ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "' " + addcollegecode + " group by es.roomno,es.ses_sion,es.edate ,r.degree_code,dp.dept_name,r.batch_year,es.subject_no";

                DataSet dset21 = new DataSet();
                dset21 = da.select_method_wo_parameter(query1, "Text");

                string hallno1 = "";
                string strngth1 = "";
                int sno1 = 1;

                if (dset21.Tables[0].Rows.Count > 0)
                {
                    AttSpreadfoil.Width = 400;
                    AttSpreadfoil.Height = 200;
                    AttSpreadfoil.Visible = true;
                    txtexcelname.Visible = true;
                    IblRpt.Visible = true;
                    reportflag = true;
                    AttSpreadfoil.Sheets[0].RowCount++;
                    AttSpreadfoil.Sheets[0].Cells[0, 1].CellType = cheselectall1;
                    AttSpreadfoil.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    for (int mm = 0; mm < dset21.Tables[0].Rows.Count; mm++)
                    {
                        AttSpreadfoil.Sheets[0].RowCount++;
                        AttSpreadfoil.Sheets[0].AutoPostBack = false;
                        hallno1 = dset21.Tables[0].Rows[mm]["roomno"].ToString();
                        strngth1 = dset21.Tables[0].Rows[mm]["strength"].ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 0].Text = sno1.ToString();
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 1].CellType = cheall1;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 2].Text = hallno1;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 3].Text = strngth1;
                        AttSpreadfoil.Sheets[0].Cells[AttSpreadfoil.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        sno1++;
                    }
                    AttSpreadfoil.Sheets[0].PageSize = AttSpreadfoil.Sheets[0].Rows.Count;
                    printexcel.Visible = true;
                    printpdf.Visible = true;
                }
                else
                {
                    Iblerror.Text = "No Records Found";
                    Iblerror.Visible = true;
                    printpdf.Visible = false;
                    printexcel.Visible = false;
                    AttSpreadfoil.Visible = false;
                    txtexcelname.Visible = false;
                    IblRpt.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    protected void gridview_created(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string gettype = da.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + DropCourse.SelectedValue.ToString() + "'");
            DataView dftview = new DataView();
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell1;
                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "";
                HeaderCell1.ColumnSpan = 4;
                if (rbform2.Checked == true)
                {
                    HeaderCell1.ColumnSpan = 3;
                }
                HeaderCell1.HorizontalAlign = HorizontalAlign.Left;
                HeaderGridRow.Cells.Add(HeaderCell1);
                showreport.Controls[0].Controls.AddAt(0, HeaderGridRow);
                string strgetset = "select * from tbl_foil_card where type='" + gettype + "' order by section_name";
                DataSet dssetiin = da.select_method_wo_parameter(strgetset, "text");
                {
                    for (int i = 0; i < dssetiin.Tables[0].Rows.Count; i++)
                    {
                        int c = addr.Count;
                        HeaderCell1 = new TableCell();
                        HeaderCell1.Text = dssetiin.Tables[0].Rows[i]["section_name"].ToString();
                        HeaderCell1.ColumnSpan = Convert.ToInt32(dssetiin.Tables[0].Rows[i]["no_col"]);
                        HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
                        HeaderGridRow.Cells.Add(HeaderCell1);
                        showreport.Controls[0].Controls.AddAt(0, HeaderGridRow);
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                HeaderGrid = (GridView)sender;
                HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                string submaxmrk = "";
                if (rbform1.Checked == true)
                {
                    HeaderCell1 = new TableCell();
                    //HeaderCell1.Text = "DATE OF EXAMINATION" + "&nbsp;" + ":" + "&nbsp;&nbsp;" + dropdate.SelectedItem.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "FN/AN:" + "&nbsp;" + Dropsession.SelectedItem.Text;
                    HeaderCell1.Text = "DATE OF EXAMINATION";
                    HeaderCell1.ColumnSpan = 10;
                    HeaderCell1.HorizontalAlign = HorizontalAlign.Left;
                    HeaderGridRow.Cells.Add(HeaderCell1);
                    showreport.Controls[0].Controls.AddAt(0, HeaderGridRow);

                    submaxmrk = da.GetFunction("select max_ext_marks from subject where subject_no='" + dropsubject.SelectedValue + "'");
                }
                else
                {
                    submaxmrk = da.GetFunction("select value from COE_Master_Settings where settings='MaxExternalMark " + gettype + "'");
                }
                TableCell HeaderCell4 = new TableCell();
                HeaderCell4.Text = "MAXIMUM MARKS" + "&nbsp;" + ":" + submaxmrk;
                HeaderCell4.ColumnSpan = 10;
                HeaderCell4.HorizontalAlign = HorizontalAlign.Left;
                HeaderGridRow.Cells.Add(HeaderCell4);
                showreport.Controls[0].Controls.AddAt(0, HeaderGridRow);

                TableCell HeaderCell5 = new TableCell();
                HeaderCell5.Text = "FOIL/BUNDLE NO :" + "<br/>";
                HeaderCell5.ColumnSpan = e.Row.Cells.Count;
                HeaderCell5.HorizontalAlign = HorizontalAlign.Left;
                HeaderGridRow.Cells.Add(HeaderCell5);
                showreport.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
        catch (Exception ex)
        {

        }
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid1 = (GridView)sender;
                    GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell HeaderCell1 = new TableCell();
                    if (chksubwise.Checked == true)
                    {
                        HeaderCell1.Text = "DEGREE & BRANCH" + " &nbsp;" + ":" + DropCourse.SelectedItem.Text;
                    }
                    else
                    {
                        HeaderCell1.Text = "DEGREE & BRANCH";
                    }
                    HeaderCell1.ColumnSpan = 7;
                    HeaderCell1.HorizontalAlign = HorizontalAlign.Left;
                    HeaderGridRow1.Cells.Add(HeaderCell1);
                    showreport.Controls[0].Controls.AddAt(0, HeaderGridRow1);

                    TableCell HeaderCell2 = new TableCell();
                    HeaderCell2.Text = "SUBJECT CODE" + "&nbsp;" + ":" + subjectcode;
                    HeaderCell2.ColumnSpan = 10;
                    HeaderCell2.HorizontalAlign = HorizontalAlign.Left;
                    HeaderGridRow1.Cells.Add(HeaderCell2);
                    showreport.Controls[0].Controls.AddAt(0, HeaderGridRow1);
                    string subname = da.GetFunction("select subject_name from subject where subject_code='" + dropsubject.SelectedValue.ToString() + "'");

                    TableCell HeaderCell3 = new TableCell();
                    HeaderCell3.Text = "TITLE OF THE PAPER" + "&nbsp;" + ":" + subname;
                    HeaderCell3.ColumnSpan = e.Row.Cells.Count;
                    HeaderCell3.HorizontalAlign = HorizontalAlign.Left;
                    HeaderGridRow1.Cells.Add(HeaderCell3);
                    showreport.Controls[0].Controls.AddAt(0, HeaderGridRow1);
                }
                e.Row.Cells[1].Width = new Unit("75px");
                e.Row.Cells[2].Width = new Unit("10px");
                if (rbform1.Checked == true)
                {
                    e.Row.Cells[3].Width = new Unit("180px");
                }
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    GridView HeaderGrid1 = (GridView)sender;
                    GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell21 = new TableCell();
                    string collname = " select collname from collinfo";
                    if (rbform2.Checked == true)
                    {
                        collname = " select collname+' ('+category+')' as collname from collinfo";
                    }
                    collname = da.GetFunction(collname);
                    HeaderCell21.Text = collname + "<br/>" + "FOIL SHEET - EXTERNAL/INTERNAL" + "<br/>" + "SEMESTER EXAMINATIONS" + "&nbsp;&nbsp;&nbsp;" + DropExammonth.SelectedItem.Text + " " + "-" + DropExamyear.SelectedItem.Text;
                    HeaderCell21.ColumnSpan = e.Row.Cells.Count;
                    HeaderCell21.HorizontalAlign = HorizontalAlign.Center;
                    HeaderGridRow1.Cells.Add(HeaderCell21);
                    showreport.Controls[0].Controls.AddAt(0, HeaderGridRow1);
                }
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }

    protected void Exportexcel_click(object sender, EventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition",
                "attachment;filename=FoilSheet.xls");
            Response.ContentType = "applicatio/excel";
            btngo_click(sender, e);
            showreport.DataBind();
            StringWriter sw = new StringWriter();
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            showreport.RenderControl(htm);
            showreport.DataBind();
            Label lb = new Label();
            lb.Text = "<br/><br/>" + "SIGNATURE OF THE EXAMINER:" + "<br/><br/><br/>" + " NAME OF THE EXAMINER" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "CAMP OFFICER";
            lb.Style.Add("height", "1000px");
            lb.Style.Add("text-decoration", "none");
            lb.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            lb.Style.Add("font-size", "14px");
            lb.Style.Add("text-align", "left");
            lb.RenderControl(htm);
            Response.Write(sw.ToString());
            Response.End();
            Response.Clear();
        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void btnpdf_generate(object sender, EventArgs e)
    {
        try
        {
            string regno = "";
            btnpdf.Visible = true;
            string subject = dropsubject.SelectedValue;
            string strbundle = "";
            if (Dropbundle.Items.Count > 0 && Dropbundle.Enabled == true)
            {
                if (Dropbundle.SelectedItem.ToString() != "")
                {
                    strbundle = " and es.bundle_no='" + Dropbundle.SelectedItem.ToString() + "'";
                }
            }
            btngo_click(sender, e);
            ds.Dispose();
            ds.Reset();
            //string query = "select r.Reg_No,es.roomno,es.seat_no,s.subject_code,s.max_ext_marks from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r,exam_seating es,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and es.regno=r.Reg_No and es.subject_no=ead.subject_no and s.subject_no=es.subject_no and s.subject_no=ead.subject_no and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "' " + strbundle + "";
            //if (chkbatch.Checked == true)
            //{
            //    query = query + " and ed.batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
            //}
            //if (chksubwise.Checked == true)
            //{
            //    query = query + " and ed.degree_code='" + ddlbatch.SelectedValue.ToString() + "'";
            //}
            //query = query + " order by ed.batch_year desc,ed.degree_code,r.Reg_No,es.roomno,es.seat_no";
            string query = "select collname,category from collinfo  where college_code='" + collegecode + "'";
            ds = da.select_method_wo_parameter(query, "Text");

            string strsubvan = "select max_ext_marks,subject_name from subject where subject_code='" + dropsubject.SelectedValue.ToString() + "'";
            DataSet dasubn = da.select_method_wo_parameter(strsubvan, "Text");
            string submax = dasubn.Tables[0].Rows[0]["max_ext_marks"].ToString();
            string subname = dasubn.Tables[0].Rows[0]["subject_name"].ToString();
            string subjectcode1 = dropsubject.SelectedValue.ToString();
            string collname = ds.Tables[0].Rows[0]["collname"].ToString();

            string gettype = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + DropExammonth.SelectedValue.ToString() + "' and ed.Exam_year='" + DropExamyear.SelectedItem.ToString() + "' and s.subject_code='" + dropsubject.SelectedValue.ToString() + "'");

            if (rbform2.Checked == true)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString() + " (" + ds.Tables[0].Rows[0]["category"].ToString() + ")";
                submax = da.GetFunction("select value from COE_Master_Settings where settings='MaxExternalMark " + gettype + "'");
            }

            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 22, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font fonttwelve = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Regular);
            System.Drawing.Font Fonttablehead = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            System.Drawing.Font fontboldset = new System.Drawing.Font("Book Antiqua", 18, FontStyle.Bold);

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14, 8.5));

            Gios.Pdf.PdfPage mypdfpage;

            Gios.Pdf.PdfTable tablepage3a;

            int noofrow = showreport.Rows.Count;
            int noofpage = noofrow / 16;
            if (noofrow % 16 > 0)
            {
                noofpage++;
            }
            int strateow = 0;
            int endrow = 0;
            for (int pagno = 0; pagno < noofpage; pagno++)
            {

                mypdfpage = mydoc.NewPage();
                PdfArea pa1 = new PdfArea(mydoc, 14, 12, 980, 590);

                PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                mypdfpage.Add(pr3);

                int coltop = 20;
                PdfTextArea pdf101 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 850, 50), System.Drawing.ContentAlignment.TopCenter, collname);
                mypdfpage.Add(pdf101);

                coltop = coltop + 25;
                PdfTextArea pdf103 = new PdfTextArea(fontboldset, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 850, 50), System.Drawing.ContentAlignment.TopCenter, "SEMESTER EXAMINATIONS" + "  " + DropExammonth.SelectedItem.Text + "  " + DropExamyear.SelectedItem.Text);
                mypdfpage.Add(pdf103);

                coltop = coltop + 20;
                PdfTextArea pdf102 = new PdfTextArea(Fonttablehead, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 850, 50), System.Drawing.ContentAlignment.TopCenter, "FOIL SHEET FOR INTERNAL / EXTERNAL");
                if (rbform2.Checked == true)
                {
                    if (chkIIIval.Checked == true)
                    {
                        pdf102 = new PdfTextArea(Fonttablehead, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 850, 50), System.Drawing.ContentAlignment.TopCenter, "THIRD VALUATION");
                    }
                    else
                    {
                        pdf102 = new PdfTextArea(Fonttablehead, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 850, 50), System.Drawing.ContentAlignment.TopCenter, "VALUATION");
                    }
                }
                mypdfpage.Add(pdf102);

                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 20, 20, 450);
                }
                else
                {
                    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                    mypdfpage.Add(LogoImage2, 450, 96, 270);
                }

                coltop = coltop + 40;

                PdfArea prheader = new PdfArea(mydoc, 14, 92, 330, 30);
                PdfRectangle prheadertop = new PdfRectangle(mydoc, prheader, Color.Black);
                mypdfpage.Add(prheadertop);

                PdfTextArea pdf105 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "DEGREE & BRANCH  :");
                if (chksubwise.Checked == true)
                {
                    pdf105 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "DEGREE & BRANCH  : " + " " + DropCourse.SelectedItem.Text);
                }
                mypdfpage.Add(pdf105);

                PdfArea prheadersub1 = new PdfArea(mydoc, 344, 92, 209, 30);
                PdfRectangle prheadersecond1 = new PdfRectangle(mydoc, prheadersub1, Color.Black);
                mypdfpage.Add(prheadersecond1);

                PdfTextArea pdf107 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 345, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "SUBJECT CODE : " + subjectcode1);
                mypdfpage.Add(pdf107);

                PdfArea prheadersub3 = new PdfArea(mydoc, 553, 92, 441, 30);
                PdfRectangle prheadersecond3 = new PdfRectangle(mydoc, prheadersub3, Color.Black);
                mypdfpage.Add(prheadersecond3);

                PdfTextArea pdf109 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 555, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "TITLE OF THE PAPER : " + subname);
                mypdfpage.Add(pdf109);


                coltop = coltop + 30;

                if (rbform1.Checked == true)
                {
                    PdfArea prheadersub4 = new PdfArea(mydoc, 14, 122, 980, 30);
                    PdfRectangle prheadersecond4 = new PdfRectangle(mydoc, prheadersub4, Color.Black);
                    mypdfpage.Add(prheadersecond4);

                    PdfTextArea pdf111 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "DATE OF THE EXAMINATION :");
                    mypdfpage.Add(pdf111);

                    PdfArea prheadersub5 = new PdfArea(mydoc, 411, 122, 254, 30);
                    PdfRectangle prheadersecond5 = new PdfRectangle(mydoc, prheadersub5, Color.Black);
                    mypdfpage.Add(prheadersecond5);
                }
                else
                {
                    PdfArea prheadersub5 = new PdfArea(mydoc, 14, 122, 980, 30);
                    PdfRectangle prheadersecond5 = new PdfRectangle(mydoc, prheadersub5, Color.Black);
                    mypdfpage.Add(prheadersecond5);
                }
                PdfTextArea pdf112 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "MAXIMUM MARKS : " + " " + submax);
                if (rbform2.Checked == true)
                {
                    pdf112 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "MAXIMUM MARKS : " + " " + submax);
                }
                mypdfpage.Add(pdf112);

                if (rbform1.Checked == true)
                {
                    if (Dropbundle.Items.Count > 0 && Dropbundle.Enabled == true)
                    {
                        if (Dropbundle.SelectedItem.ToString() != "")
                        {
                            PdfTextArea pdf113 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 675, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "FOIL / BUNDLE NO :");
                            mypdfpage.Add(pdf113);
                        }
                    }
                }

                int tbalerowset = 18;
                int totorow = (pagno + 1) * 16;
                endrow = 16;
                if (tbalerowset - 2 < noofrow)
                {
                    if (totorow > noofrow)
                    {
                        totorow = pagno * 16;
                        tbalerowset = noofrow - totorow;
                        tbalerowset = tbalerowset + 2;
                        endrow = noofrow;
                    }
                    else
                    {
                        endrow = (pagno + 1) * 16;
                    }
                }
                else
                {
                    tbalerowset = noofrow + 2;
                    endrow = noofrow;
                }

                tablepage3a = mydoc.NewTable(Fontsmall, tbalerowset, showreport.Rows[0].Cells.Count, 2);
                tablepage3a.VisibleHeaders = false;

                tablepage3a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                tablepage3a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tablepage3a.Cell(0, 0).SetContent("S.No");
                tablepage3a.Cell(0, 0).SetFont(Fonttablehead);
                foreach (PdfCell pc in tablepage3a.CellRange(0, 0, 0, 0).Cells)
                {
                    pc.RowSpan = 2;
                }
                tablepage3a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                bool Dummy = Convert.ToBoolean(Session["DummyCheck"]);
                if (Dummy == true)
                {
                    tablepage3a.Cell(0, 1).SetContent("Dummy No");
                }
                else
                {
                    tablepage3a.Cell(0, 1).SetContent("Reg No");
                }
                tablepage3a.Cell(0, 1).SetFont(Fonttablehead);
                foreach (PdfCell pc in tablepage3a.CellRange(0, 1, 0, 1).Cells)
                {
                    pc.RowSpan = 2;
                }
                tablepage3a.Columns[0].SetWidth(14);
                tablepage3a.Columns[1].SetWidth(45);

                tablepage3a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                tablepage3a.Cell(0, 2).SetContent("Total Marks");
                tablepage3a.Cell(0, 2).SetFont(Fonttablehead);
                tablepage3a.Columns[2].SetWidth(20);
                foreach (PdfCell pc in tablepage3a.CellRange(0, 2, 0, 2).Cells)
                {
                    pc.RowSpan = 2;
                }

                int intgridstartcolumn = 2;
                if (rbform1.Checked == true)
                {
                    intgridstartcolumn = 3;
                    tablepage3a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepage3a.Cell(0, 3).SetContent("Marks in Words");
                    tablepage3a.Cell(0, 3).SetFont(Fonttablehead);
                    foreach (PdfCell pc in tablepage3a.CellRange(0, 3, 0, 3).Cells)
                    {
                        pc.RowSpan = 2;
                    }
                    tablepage3a.Columns[3].SetWidth(35);
                }
                else
                {
                    tablepage3a.Columns[3].SetWidth(5);
                }

                for (int j = intgridstartcolumn; j < showreport.Rows[0].Cells.Count; j++)
                {
                    string vsa = showreport.HeaderRow.Cells[j].Text;
                    string val = showreport.Rows[0].Cells[j].Text.ToString();
                    tablepage3a.Cell(1, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepage3a.Cell(1, j).SetContent(vsa.ToString());
                    tablepage3a.Cell(0, j).SetFont(Fonttablehead);
                    tablepage3a.Cell(1, j).SetFont(Fonttablehead);
                }

                for (int col = intgridstartcolumn + 1; col < showreport.Rows[0].Cells.Count; col++)
                {
                    tablepage3a.Columns[col].SetWidth(10);
                }
                int rowset = 1;

                strateow = pagno * 16;
                if (strateow == 0)
                {
                    //strateow = 2;
                    //endrow = endrow + 2;
                }
                for (int j = strateow; j < endrow; j++)
                {
                    rowset++;
                    regno = showreport.Rows[j].Cells[1].Text.ToString();
                    string srno = showreport.Rows[j].Cells[0].Text.ToString();
                    tablepage3a.Cell(rowset, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepage3a.Cell(rowset, 1).SetContent(regno.ToString());
                    tablepage3a.Cell(rowset, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepage3a.Cell(rowset, 0).SetContent((srno).ToString());
                }
                //string gettype = da.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + DropCourse.SelectedValue.ToString() + "'");
                string strgetset = "select type,section_name,no_col,value,college_code from tbl_foil_card where type='" + gettype + "' order by section_name";
                DataSet dssetiin = da.select_method_wo_parameter(strgetset, "text");
                dssetiin.Tables[0].DefaultView.RowFilter = "type='" + gettype + "'";
                DataView dtview = dssetiin.Tables[0].DefaultView;
                int start = 4;
                if (rbform2.Checked == true)
                {
                    start = 3;
                }
                for (int h = 0; h < dtview.Count; h++)
                {
                    string sect = dtview[h]["section_name"].ToString();
                    int nod = Convert.ToInt32(dtview[h]["no_col"].ToString());
                    tablepage3a.Cell(0, start).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepage3a.Cell(0, start).SetContent(sect);

                    if (nod > 0)
                    {
                        foreach (PdfCell pc in tablepage3a.CellRange(0, start, 0, start).Cells)
                        {
                            pc.ColSpan = nod;
                        }
                        start = start + nod;
                    }
                }

                coltop = coltop + 30;
                tablepage3a.VisibleHeaders = false;
                Gios.Pdf.PdfTablePage newpdftabpage3 = tablepage3a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 14, coltop, 980, 900));
                mypdfpage.Add(newpdftabpage3);

                if (rbform1.Checked == true)
                {
                    PdfTextArea pdf115 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 560, 595, 50), System.Drawing.ContentAlignment.TopLeft, "SIGNATURE OF THE EXAMINER WITH DATE");
                    mypdfpage.Add(pdf115);

                    PdfTextArea pdf116 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 590, 595, 50), System.Drawing.ContentAlignment.TopLeft, "NAME OF THE EXAMINER");
                    mypdfpage.Add(pdf116);

                    PdfTextArea pdf117 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 830, 590, 595, 50), System.Drawing.ContentAlignment.TopLeft, "CAMP OFFICER");
                    mypdfpage.Add(pdf117);

                    PdfTextArea pdf118 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 590, 595, 50), System.Drawing.ContentAlignment.TopLeft, "CHAIRMAN");
                    mypdfpage.Add(pdf118);
                }
                else
                {
                    PdfTextArea pdf116 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 590, 595, 50), System.Drawing.ContentAlignment.TopLeft, "NAME OF THE EXAMINER");
                    mypdfpage.Add(pdf116);

                    PdfTextArea pdf118 = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 740, 590, 595, 50), System.Drawing.ContentAlignment.TopLeft, "SIGNATURE OF THE EXAMINER WITH DATE");
                    mypdfpage.Add(pdf118);
                }
                mypdfpage.SaveToDocument();
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Foilsheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            Iblerror.Visible = true;
            Iblerror.Text = ex.ToString();
        }
    }
    protected void chkconsolidate_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chkIIIval.Checked == true)
            {
                chkconsolidate.Checked = false;
                showreport.Visible = false;

            }
            if (chkconsolidate.Checked == true)
            {
                ddlpdateexam.Enabled = true;
                DropCourse.Enabled = false;
                dropsubject.Enabled = false;
                Dropbundle.Enabled = false;
                showreport.Visible = false;
                Excel.Visible = false;
                btnpdf.Visible = false;
                DropCourse.ForeColor = Color.Gray;
                txtexcelname.Visible = false;
                IblRpt.Visible = false;

            }
            if (chkconsolidate.Checked == false)
            {
                ddlpdateexam.Enabled = false;
                DropCourse.Enabled = true;
                dropsubject.Enabled = true;
                Dropbundle.Enabled = true;
                DropCourse.ForeColor = Color.Black;
                txtexcelname.Visible = false;
                IblRpt.Visible = false;
                AttSpreadfoil.Visible = false;
                printpdf.Visible = false;
                printexcel.Visible = false;
                Iblerr.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void printpdf_click(object sender, EventArgs e)  ///added by jeyagandhi/////////
    {
        try
        {
            System.Drawing.Font Fon10b = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font Font12b = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Font12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Font10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font font14b = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Font14 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);

            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 22, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font fonttwelve = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Regular);
            System.Drawing.Font Fonttablehead = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            System.Drawing.Font fontboldset = new System.Drawing.Font("Book Antiqua", 18, FontStyle.Bold);


            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14, 8.5));
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();

            DataSet dselect = new DataSet();
            string collinfo = "Select * from collinfo where college_Code='" + collegecode + "'";
            dselect = da.select_method_wo_parameter(collinfo, "text");
            int n = 1;
            string regno1 = "";
            DataSet dsoutput = new DataSet();
            int es = 0;
            int g = 0;
            DataSet dspdf = new DataSet();
            string examdate = ddlpdateexam.SelectedItem.Text.ToString();
            string[] dsplit = examdate.Split('/');
            //string examdate = "21-4-2016";
            //string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            string sessiond = "";
            //if (Dropsession.SelectedItem.Text == "Both")
            //{
            //    sessiond = "";
            //}
            //else
            //{
            //    sessiond = "  and es.ses_sion='" + Dropsession.SelectedItem.Text + "'";
            //}

            //////////////////////Binding spread////////////////
            AttSpreadfoil.SaveChanges();
            string strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dsa = da.select_method_wo_parameter(strquery, "Text");
            string pdf1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,course_name,dp.dept_name,r.batch_year,es.subject_no, es.bundle_no  from registration r,exam_details ed,exam_application ea, exam_appl_details ead,exam_seating as es,degree d,course c,department dp where  ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no  and r.exam_flag<>'Debar' and es.regno=r.Reg_No   and ead.subject_no=es.subject_no   and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and      ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and    dp.dept_code=d.dept_code and      d.college_code=r.college_code       and d.Course_Id = c.Course_Id        and    ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "' and es.edate='" + examdate + "' " + sessiond + "  and r.college_code='" + collegecode + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,course_name,  dp.dept_name,r.batch_year,es.subject_no,es.bundle_no ";
            //  string pdf1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,dp.dept_name,r.batch_year,es.subject_no, es.bundle_no  from registration r,exam_details ed,exam_application ea, exam_appl_details ead,exam_seating as es,degree d,department dp where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no  and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no   and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and    ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and    dp.dept_code=d.dept_code and d.college_code=r.college_code  and    ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "' and es.edate='" + examdate + "' " + sessiond + "  and r.college_code='" + collegecode + "'     group by es.roomno,es.ses_sion,es.edate ,r.degree_code,dp.dept_name,     r.batch_year,es.subject_no,es.bundle_no ";
            // dspdf = da.select_method_wo_parameter(pdf1, "Text");
            string bundleno = "";
            int isval = 0;
            //if (dspdf.Tables[0].Rows.Count > 0)
            //{
            for (int j = 0; j < AttSpreadfoil.Sheets[0].RowCount; j++)
            {
                isval = Convert.ToInt32(AttSpreadfoil.Sheets[0].Cells[j + 1, 1].Value);
                if (isval == 1)
                {
                    if (dselect.Tables[0].Rows.Count > 0)
                    {
                        string gettype = da.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + AttSpreadfoil.Sheets[0].Cells[j + 1, 3].Tag.ToString() + "'");
                        string strgetset = "select type,section_name,no_col,value,college_code from tbl_foil_card where type='" + gettype + "' order by section_name";
                        DataSet dssetiin = da.select_method_wo_parameter(strgetset, "text");
                        dssetiin.Tables[0].DefaultView.RowFilter = "type='" + gettype + "'";
                        DataView dtview = dssetiin.Tables[0].DefaultView;
                        int noda = 0;
                        Iblerr.Visible = false;
                        Iblerr.Text = "";

                        for (int o = 0; o < dssetiin.Tables[0].Rows.Count; o++)
                        {
                            if (noda == 0)
                            {
                                noda = Convert.ToInt32(dssetiin.Tables[0].Rows[o]["no_col"]);
                            }
                            else
                            {

                                noda = noda + Convert.ToInt32(dssetiin.Tables[0].Rows[o]["no_col"]);
                            }
                        }
                        string subject = AttSpreadfoil.Sheets[0].Cells[j + 1, 4].Tag.ToString();// dspdf.Tables[0].Rows[j]["subject_no"].ToString();
                        string sub = "select subject_code,subject_name from subject where subject_no='" + subject + "'";
                        DataSet dssub = da.select_method_wo_parameter(sub, "text");
                        string subname = dssub.Tables[0].Rows[0]["subject_code"].ToString();
                        string sub_name = dssub.Tables[0].Rows[0]["subject_name"].ToString();
                        string room = AttSpreadfoil.Sheets[0].Cells[j + 1, 2].Text.ToString();// dspdf.Tables[0].Rows[j]["roomno"].ToString();
                        string deptaname = AttSpreadfoil.Sheets[0].Cells[j + 1, 3].Text.ToString();// dspdf.Tables[0].Rows[j]["dept_name"].ToString();
                        string degreew = AttSpreadfoil.Sheets[0].Cells[j + 1, 3].Tag.ToString();// dspdf.Tables[0].Rows[j]["degree_code"].ToString();
                        bundleno = AttSpreadfoil.Sheets[0].Cells[j + 1, 6].Text.ToString();//dspdf.Tables[0].Rows[j]["bundle_no"].ToString();
                        string deoptame = AttSpreadfoil.Sheets[0].Cells[j + 1, 3].Text.ToString();// dspdf.Tables[0].Rows[j]["course_name"].ToString();
                        //  deoptame = "";
                        string pdf = "";
                        if (bundleno != "")
                        {
                            pdf = "select es.edate,es.ses_sion,es.roomno,es.subject_no,es.bundle_no,es.regno,es.degree_code,r.Batch_Year,r.roll_no,subject_code,max_ext_marks from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject u where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and  r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and ead.subject_no = u.subject_no   and es.edate='" + examdate + "' " + sessiond + "  and es.degree_code='" + degreew + "'  and roomno='" + room + "' and es.subject_no  ='" + subject + "' and ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "'   and es.bundle_no='" + bundleno + "'  order by es.seat_no";
                        }
                        else
                        {
                            pdf = "select es.edate,es.ses_sion,es.roomno,es.subject_no,es.regno,es.degree_code,r.Batch_Year,r.roll_no,subject_code,max_ext_marks from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject u where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and  r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and ead.subject_no = u.subject_no   and es.edate='" + examdate + "' " + sessiond + "  and es.degree_code='" + degreew + "'   and roomno='" + room + "' and es.subject_no  ='" + subject + "' and ed.Exam_Month='" + DropExammonth.SelectedValue + "' and ed.Exam_year='" + DropExamyear.SelectedItem.Text + "' order by es.seat_no";
                            bundleno = " ";
                        }
                        dsoutput.Clear();
                        dsoutput.Dispose();
                        dsoutput = da.select_method_wo_parameter(pdf, "text");

                        if (dsoutput.Tables[0].Rows.Count > 0)
                        {
                            string maxmarks = dsoutput.Tables[0].Rows[0]["max_ext_marks"].ToString();


                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 450);
                            }
                            string collname = dselect.Tables[0].Rows[0]["collname"].ToString();
                            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 210, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, collname);
                            mypdfpage.Add(ptc);
                            int month = Convert.ToInt32(DropExammonth.SelectedItem.Value.ToString());
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            ptc = new PdfTextArea(fontboldset, System.Drawing.Color.Black, new PdfArea(mydoc, 210, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "SEMESTER EXAMINATIONS" + " " + strMonthName.ToUpper() + " " + DropExamyear.SelectedItem.Text);
                            mypdfpage.Add(ptc);
                            string foilname = "  FOIL SHEET FOR INTERNAL / EXTERNAL";
                            ptc = new PdfTextArea(Fonttablehead, System.Drawing.Color.Black, new PdfArea(mydoc, 210, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, foilname);
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 100, 595, 50), System.Drawing.ContentAlignment.TopLeft, " DEGREE & BRANCH" + " : ");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 65, 200, 80), System.Drawing.ContentAlignment.MiddleLeft, " " + deoptame + "");
                            mypdfpage.Add(ptc);
                            //ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 85, 200, 50), System.Drawing.ContentAlignment.MiddleCenter, "Bsc., - Advanced Zoology And Advanced Bio Technology");
                            //mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 370, 100, 595, 50), System.Drawing.ContentAlignment.TopLeft, "SUBJECT CODE " + " :  " + subname);
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 610, 100, 380, 50), System.Drawing.ContentAlignment.TopLeft, "TITLE OF THE PAPER" + " : ");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 745, 65, 250, 80), System.Drawing.ContentAlignment.MiddleLeft, "" + sub_name + " ");
                            mypdfpage.Add(ptc);

                            //ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "DATE OF THE EXAMNIATION" + " :  " + dropdate.SelectedItem.Text + "   " + "FN/AN" + ":  " + Dropsession.SelectedItem.Text);
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "DATE OF EXAMNIATION" + " :  " + ddlpdateexam.SelectedItem.Text.ToString() + "  " + dsoutput.Tables[0].Rows[0][1].ToString());  // Code added to include session 25-04-2016
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "MAXIMUM MARKS " + " :" + maxmarks);
                            mypdfpage.Add(ptc);

                            //ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 677, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "FOIL/BUNDLE NO" + " :" + bundleno);
                            foilname = "  FOIL / BUNDLE NO. :";
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 677, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, foilname);
                            mypdfpage.Add(ptc);
                            Gios.Pdf.PdfTable tablepage3a = mydoc.NewTable(Fontsmall, dsoutput.Tables[0].Rows.Count + 2, noda + 4, 3);
                            tablepage3a.VisibleHeaders = false;

                            tablepage3a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablepage3a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3a.Cell(0, 0).SetContent("S.No.");
                            tablepage3a.Cell(0, 0).SetFont(Fonttablehead);

                            //tablepage3a.Columns[0].SetWidth(14);
                            //tablepage3a.Columns[1].SetWidth(45);

                            foreach (PdfCell pc in tablepage3a.CellRange(0, 0, 0, 0).Cells)
                            {
                                pc.RowSpan = 2;
                            }
                            tablepage3a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3a.Cell(0, 1).SetContent("Reg. No.");
                            tablepage3a.Cell(0, 1).SetFont(Fonttablehead);

                            foreach (PdfCell pc in tablepage3a.CellRange(0, 1, 0, 1).Cells)
                            {
                                pc.RowSpan = 2;
                            }

                            tablepage3a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3a.Cell(0, 2).SetContent("Total Marks");
                            tablepage3a.Cell(0, 2).SetFont(Fonttablehead);
                            foreach (PdfCell pc in tablepage3a.CellRange(0, 2, 0, 2).Cells)
                            {
                                pc.RowSpan = 2;
                            }
                            tablepage3a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3a.Cell(0, 3).SetContent("Marks in Words");
                            tablepage3a.Cell(0, 3).SetFont(Fonttablehead);
                            foreach (PdfCell pc in tablepage3a.CellRange(0, 3, 0, 3).Cells)
                            {
                                pc.RowSpan = 2;
                            }

                            if (dsoutput.Tables[0].Rows.Count > 0)
                            {
                                int rowset = 1;
                                for (int yr = 0; yr < dsoutput.Tables[0].Rows.Count; yr++)
                                {
                                    regno1 = dsoutput.Tables[0].Rows[yr]["regno"].ToString();
                                    tablepage3a.Cell(yr + 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage3a.Cell(yr + 2, 0).SetContent(rowset);
                                    tablepage3a.Cell(yr + 2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage3a.Cell(yr + 2, 1).SetContent(regno1);
                                    rowset++;

                                }
                                es++;
                                tablepage3a.Columns[0].SetWidth(5);
                                tablepage3a.Columns[1].SetWidth(14);
                                tablepage3a.Columns[2].SetWidth(7);
                                tablepage3a.Columns[3].SetWidth(14);

                                int start = 4;
                                int se = 4;
                                for (int h = 0; h < dssetiin.Tables[0].Rows.Count; h++)
                                {

                                    string sect = dtview[h]["section_name"].ToString();
                                    int nod = Convert.ToInt32(dtview[h]["no_col"].ToString());
                                    tablepage3a.Cell(0, start).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage3a.Cell(0, start).SetContent(sect);

                                    gettype = da.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + degreew + "'");
                                    string value = "select * from tbl_foil_card where type='" + gettype + "' and section_name='" + sect + "'";
                                    DataSet dstype = new DataSet();
                                    dstype.Clear();
                                    dstype.Dispose();
                                    dstype = da.select_method_wo_parameter(value, "text");

                                    string type = dstype.Tables[0].Rows[0]["value"].ToString();
                                    string[] split = type.Split(',');
                                    if (dstype.Tables[0].Rows.Count > 0)
                                    {
                                        for (int r = 0; r < nod; r++)
                                        {
                                            string sw = split[r].ToString();
                                            tablepage3a.Cell(1, se).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tablepage3a.Cell(1, se).SetContent(sw.ToString());
                                            tablepage3a.Cell(0, se).SetFont(Fonttablehead);
                                            //tablepage3a.Columns[se].SetWidth(25);

                                            se++;

                                        }

                                    }

                                    foreach (PdfCell pc in tablepage3a.CellRange(0, start, 0, start).Cells)
                                    {
                                        pc.ColSpan = nod;
                                    }
                                    start = start + nod;
                                }

                                int coltop1 = 10;
                                coltop1 = coltop1 + 30;

                                tablepage3a.VisibleHeaders = false;
                                Gios.Pdf.PdfTablePage newpdftabpage3 = tablepage3a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 14, 150, 980, 900));
                                mypdfpage.Add(newpdftabpage3);

                                //ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 480, 595, 50), System.Drawing.ContentAlignment.BottomLeft, "SIGNATURE OF THE EXAMINER");
                                ptc = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 520, 595, 50), System.Drawing.ContentAlignment.BottomLeft, "SIGNATURE OF THE EXAMINER WITH DATE");
                                mypdfpage.Add(ptc);

                                int coltop = 530;
                                ptc = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 540, 595, 50), System.Drawing.ContentAlignment.BottomLeft, "NAME OF THE EXAMINER");
                                mypdfpage.Add(ptc);

                                // ptc = new PdfTextArea(Fon10b, System.Drawing.Color.Black, new PdfArea(mydoc, 155, 530, 595, 50), System.Drawing.ContentAlignment.BottomCenter, "CHAIRMAN/BOARD");
                                ptc = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 195, 540, 595, 50), System.Drawing.ContentAlignment.BottomCenter, "CHAIRMAN");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(fonttwelve, System.Drawing.Color.Black, new PdfArea(mydoc, 320, 540, 595, 50), System.Drawing.ContentAlignment.BottomRight, "CAMP OFFICER");
                                mypdfpage.Add(ptc);

                                PdfArea pa1 = new PdfArea(mydoc, 14, 12, 980, 580);
                                PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                                mypdfpage.Add(pr3);

                                ///////////////////left1//////////////////////////
                                PdfArea pa4 = new PdfArea(mydoc, 15, 90, 350, 30);
                                PdfRectangle pr4 = new PdfRectangle(mydoc, pa4, Color.Black);
                                mypdfpage.Add(pr4);

                                ////////////////////left2//////////////////////////
                                PdfArea pa2 = new PdfArea(mydoc, 365, 90, 240, 30);
                                PdfRectangle pr1 = new PdfRectangle(mydoc, pa2, Color.Black);
                                mypdfpage.Add(pr1);

                                /////////////////////////left3////////////////////
                                PdfArea pa3 = new PdfArea(mydoc, 605, 90, 390, 30);
                                PdfRectangle pr5 = new PdfRectangle(mydoc, pa3, Color.Black);
                                mypdfpage.Add(pr5);

                                //////////////////LEFT2ND//////////////////////
                                PdfArea pa5 = new PdfArea(mydoc, 15, 120, 422, 30);
                                PdfRectangle pr6 = new PdfRectangle(mydoc, pa5, Color.Black);
                                mypdfpage.Add(pr6);

                                ///////////////////center//////////////////
                                PdfArea pa7 = new PdfArea(mydoc, 437, 120, 240, 30);
                                PdfRectangle pr7 = new PdfRectangle(mydoc, pa7, Color.Black);
                                mypdfpage.Add(pr7);

                                //////////////////////last////////////////////
                                PdfArea pa8 = new PdfArea(mydoc, 677, 120, 317, 30);
                                PdfRectangle pr8 = new PdfRectangle(mydoc, pa8, Color.Black);
                                mypdfpage.Add(pr8);
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                Iblerr.Visible = false;
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Foilsheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                        else
                        {
                            Iblerr.Text = "No Records Found";
                            Iblerr.Visible = true;
                        }
                    }
                }
                else
                {
                    Iblerr.Text = "Please Select AnyOne Record";
                    Iblerr.Visible = true;
                }
            }
            //}
            //else
            //{
            //    Iblerr.Text = "Please Allot Bundle No And Then Proceed";
            //    Iblerr.Visible = true;




            //}
        }
        catch (Exception ex)
        {
        }
    }

    protected void AttSpreadfoil_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Iblerr.Visible = false;
            Iblerr.Text = "";

            if (Convert.ToInt32(AttSpreadfoil.Sheets[0].Cells[0, 1].Value) == 1)
            {

                for (int i = 0; i < AttSpreadfoil.Sheets[0].RowCount; i++)
                {
                    AttSpreadfoil.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < AttSpreadfoil.Sheets[0].RowCount; i++)
                {
                    AttSpreadfoil.Sheets[0].Cells[i, 1].Value = 0;
                }
            }

        }
        catch (Exception ex)
        {
            Iblerr.Text = ex.ToString();
            Iblerr.Visible = true;
        }
    }

    protected void btnprintexcel_click(object sender, EventArgs e)
    {
        try
        {

            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                Iblerr.Text = "";
                Iblerr.Visible = false;
                da.printexcelreport(AttSpreadfoil, strexcelname);
            }
            else
            {
                Iblerr.Text = "Please Enter The Report Name";
                Iblerr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Iblerr.Text = ex.ToString();
            Iblerr.Visible = true;
        }
    }
    protected void Forametchage(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (rbform1.Checked == true)
            {
                loadSubjectName();
                loadbundle();
            }
            else
            {
                Dropbundle.Enabled = false;
                loadSubjectName();
                Dropbundle.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            Iblerr.Text = ex.ToString();
            Iblerr.Visible = true;
        }
    }

    protected void loadmdatesession()
    {
        try
        {
            ddlpdateexam.Items.Clear();

            DateTime dtf = DateTime.Now;
            DateTime dtt = DateTime.Now;

            string hol = "select * from examholiday where examyear ='" + DropExamyear.SelectedValue + "' and exammonth='" + DropExammonth.SelectedItem.Value + "'";
            ds = da.select_method_wo_parameter(hol, "text");
            if (ds.Tables[0].Rows.Count > 0)
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
                    ddlpdateexam.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dt.ToString("dd/MM/yyyy"), dt.ToString("MM/dd/yyyy")));
                }
            }


        }
        catch (Exception ex)
        {


        }
    }

}